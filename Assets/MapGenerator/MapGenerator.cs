using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.Intrinsics;
using UnityEngine;
using static UnityEditor.VersionControl.Asset;
using Color = UnityEngine.Color;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour
{
    [Header("Map Generation Settings")]
    // Noisescale is like a "zoom" on the perlin noise
    // high => far away, low => close
    [SerializeField, Range(0.1f, 50f)] private float noiseScale = 4f;
    [SerializeField, Range(0f, 1f)] private float waterThreshold = 0.45f;
    [SerializeField, Range(0f, 1f)] private float beachThreshold = 0.48f;
    [SerializeField, Range(0f, 1f)] private float grassThreshold = 0.8f;
    [SerializeField, Range(0f, 1f)] private float mountainThreshold = 0.9f;
    [SerializeField, Range(0f, 500f)] private float random = 50f;
    [SerializeField, Range(100, 3000)] private int mapWidth = 500;
    [SerializeField, Range(100, 3000)] private int mapHeight = 300;
    [SerializeField, Range(0, 50)] private int outerBoundaryXSize = 10;
    [SerializeField, Range(0, 50)] private int outerBoundaryYSize = 10;
    [SerializeField, Range(0, 300)] private int Statesize = 170;

    private List<Color32> ColorsUsedInTerrain = new List<Color32>() { Color.green, Color.blue, Color.white, Color.gray, Color.yellow };

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var provinceColor = ColorHelper.GetColor(mainCamera, Input.mousePosition);
        }
    }

    public void GenerateMap()
    {
        var (Terrain, States) = GeneratePixels();

        var terrainTexture = ImageHelper.CreateTexture2D(Terrain, this.mapWidth, this.mapHeight);
        ImageHelper.SaveMap(terrainTexture, Application.dataPath + "/GeneratedMaps/Terrain.png");
        var statesTexture = ImageHelper.CreateTexture2D(States, this.mapWidth, this.mapHeight);
        ImageHelper.SaveMap(statesTexture, Application.dataPath + "/GeneratedMaps/States.png");

        ShowTerrain();
    }

    public void ShowTerrain()
    {
        var sprite = ImageHelper.LoadImageFromDisk(this.mapWidth, this.mapHeight, Application.dataPath + "/GeneratedMaps/Terrain.png");
        GetComponent<SpriteRenderer>().sprite = sprite;
        sprite.texture.filterMode = FilterMode.Point;
        sprite.texture.Apply();
    }

    public void ShowStates()
    {
        var sprite = ImageHelper.LoadImageFromDisk(this.mapWidth, this.mapHeight, Application.dataPath + "/GeneratedMaps/States.png");
        GetComponent<SpriteRenderer>().sprite = sprite;
    }

    private (Color32[] Terrain, Color32[] States) GeneratePixels()
    {
        var generator = new TerrainGenerator(this.mapWidth, this.mapHeight, this.noiseScale, this.random, this.outerBoundaryXSize, this.outerBoundaryYSize);
        var noiseMap = generator.GenerateNoiseMap();
        var terrain = generator.GenerateTerrain(noiseMap, this.waterThreshold, this.beachThreshold, this.grassThreshold, this.mountainThreshold);

        var states = GenerateStates(terrain);

        return (terrain, states);
    }

    private Color32[] GenerateStates(Color32[] Terrain)
    {
        var states = new Color32[Terrain.Length];
        Array.Copy(Terrain, states, Terrain.Length);

        var found = true;
        var startingPosition = new Vector2();

        var stateColors = new List<Color32>();

        while (found)
        {
            found = false;

            for (var x = 0; x < this.mapWidth; x++)
            {
                for (var y = 0; y < this.mapHeight; y++)
                {
                    var pixelColor = states[y * this.mapWidth + x];
                    if (pixelColor == Color.green)
                    {
                        found = true;
                        startingPosition.x = x;
                        startingPosition.y = y;
                    }
                }
            }

            if (found)
            {
                var size = UnityEngine.Random.Range(this.Statesize / 2, this.Statesize);
                var colorsToReplace = new Color32[] { Color.green, Color.yellow };
                var stateColor = AddNewRandomColorToList(stateColors);
                PaintHelper.FloodPaint(states, this.mapWidth, this.mapHeight, startingPosition, colorsToReplace, stateColor, size);
            }
        }

        return states;
    }

    Color32 AddNewRandomColorToList(List<Color32> colorList)
    {
        Color32 randomColor;

        do
        {
            // Generate a random color
            randomColor = new Color32(
    (byte)Random.Range(0, 256),  // Random red component (0 to 255)
    (byte)Random.Range(0, 256),  // Random green component (0 to 255)
    (byte)Random.Range(0, 256),  // Random blue component (0 to 255)
    255                           // Fully opaque alpha component (255)
);

            // Check if the color is already in the list
        } while (ColorHelper.ColorListContainsColor(colorList, randomColor) || ColorHelper.ColorListContainsColor(this.ColorsUsedInTerrain, randomColor));

        // Add the new color to the list if needed
        colorList.Add(randomColor);

        return randomColor;
    }
}