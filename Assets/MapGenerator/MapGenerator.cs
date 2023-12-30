using System;
using System.Collections.Generic;
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

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var clickedProvince = ColorHelper.GetColor(mainCamera, Input.mousePosition);
            //Debug.Log(clickedProvince);
            var isGray = clickedProvince.Equals(ColorHelper.gray);
            Debug.Log(isGray);
            Debug.Log(Color.gray);
        }
    }

    public void GenerateMap()
    {
        var (Terrain, Provinces) = GeneratePixels();

        ImageHelper.SaveTerrainPixels(Terrain);
        ImageHelper.SaveProvincesPixels(Provinces);

        ShowTerrain();
    }

    public void ShowTerrain()
    {
        var sprite = ImageHelper.LoadImageFromDisk(this.mapWidth, this.mapHeight, Application.dataPath + "/GeneratedMaps/Terrain.png");
        GetComponent<SpriteRenderer>().sprite = sprite;
    }

    public void ShowProvinces()
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

        //AddStateBordersToTerrain(terrain, states);

        return (terrain, states);
    }

    private void AddStateBordersToTerrain(Color32[] terrain, Color32[] states)
    {
        for (var x = 0; x < this.mapWidth; x++)
        {
            for (var y = 0; y < this.mapHeight; y++)
            {
                var pixelColor = states[y * mapWidth + x];

                if (ColorHelper.ColorListContainsColor(ColorHelper.ColorsUsedInTerrain, pixelColor))
                {
                    continue;
                }

                var neighbours = GetNeighbours(y * mapWidth + x, this.mapWidth, this.mapHeight);
                foreach (var neighbour in neighbours)
                {
                    var neighbourColor = states[neighbour];
                    if (neighbourColor.Equals(ColorHelper.blue) || !pixelColor.Equals(neighbourColor) && (!ColorHelper.ColorListContainsColor(ColorHelper.ColorsUsedInTerrain, neighbourColor)))
                    {
                        terrain[y * mapWidth + x] = Color.black;
                    }
                }
            }
        }
    }

    public static List<int> GetNeighbours(int pixelIndex, int mapWidth, int mapHeight)
    {
        List<int> neighbours = new List<int>();

        int x = pixelIndex % mapWidth;
        int y = pixelIndex / mapWidth;

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                int neighborX = x + i;
                int neighborY = y + j;

                // Check if the neighbor is within bounds
                if (neighborX >= 0 && neighborX < mapWidth && neighborY >= 0 && neighborY < mapHeight)
                {
                    int neighborIndex = neighborY * mapWidth + neighborX;
                    neighbours.Add(neighborIndex);
                }
            }
        }

        return neighbours;
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
        } while (ColorHelper.ColorListContainsColor(colorList, randomColor) || ColorHelper.ColorListContainsColor(ColorHelper.ColorsUsedInTerrain, randomColor));

        // Add the new color to the list if needed
        colorList.Add(randomColor);

        return randomColor;
    }
}