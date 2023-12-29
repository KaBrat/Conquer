using System;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
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

    private Color[] Terrain;

    private List<Color> ColorsUsedInTerrain = new List<Color>() { Color.green, Color.blue, Color.white, Color.gray, Color.yellow };

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
    }

    public void ShowStates()
    {
        var sprite = ImageHelper.LoadImageFromDisk(this.mapWidth, this.mapHeight, Application.dataPath + "/GeneratedMaps/States.png");
        GetComponent<SpriteRenderer>().sprite = sprite;
    }

    private (Color[] Terrain, Color[] States) GeneratePixels()
    {
        var generator = new TerrainGenerator(this.mapWidth, this.mapHeight, this.noiseScale, this.random, this.outerBoundaryXSize, this.outerBoundaryYSize);
        var noiseMap = generator.GenerateNoiseMap();
        this.Terrain = generator.GenerateTerrain(noiseMap, this.waterThreshold, this.beachThreshold, this.grassThreshold, this.mountainThreshold);

        var states = GenerateStates(this.Terrain);

        return (this.Terrain, states);
    }

    private Color[] GenerateStates(Color[] Terrain)
    {
        var states = new Color[Terrain.Length];
        Array.Copy(Terrain, states, Terrain.Length);

        var found = true;
        var startingPosition = new Vector2();

        var stateColors = new List<Color>();

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
                var colorsToReplace = new Color[] { Color.green, Color.yellow };
                var stateColor = AddNewRandomColorToList(stateColors);
                PaintHelper.FloodPaint(states, this.mapWidth, this.mapHeight, startingPosition, colorsToReplace, stateColor, size);
            }
        }

        return states;
    }

    Color AddNewRandomColorToList(List<Color> colorList)
    {
        Color randomColor;

        do
        {
            // Generate a random color
            randomColor = new Color(Random.value, Random.value, Random.value);

            // Check if the color is already in the list
        } while ( ColorHelper.ColorListContainsColor(colorList, randomColor) || ColorHelper.ColorListContainsColor(this.ColorsUsedInTerrain, randomColor));

        // Add the new color to the list if needed
        colorList.Add(randomColor);

        return randomColor;
    }
}