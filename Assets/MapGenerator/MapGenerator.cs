using System;
using System.Collections.Generic;
using System.IO;
using Unity.Properties;
using UnityEngine;
using FloodSpill;
using FloodSpill.Utilities;

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

    private Sprite terrain;
    private Sprite states;

    private Color[] Terrain;

    public void GenerateMap()
    {
        var (Terrain, States) = GeneratePixels();
        this.terrain = TextureGenerator.SaveMap(Terrain, mapWidth, mapHeight, Application.dataPath + "/GeneratedMaps/Terrain.png");
        this.states = TextureGenerator.SaveMap(States, mapWidth, mapHeight, Application.dataPath + "/GeneratedMaps/Terrain.png");
        ShowTerrain();
    }

    public void ShowTerrain()
    {
        GetComponent<SpriteRenderer>().sprite = this.terrain;
    }

    public void ShowStates()
    {
        GetComponent<SpriteRenderer>().sprite = this.states;
    }

    private (Color[] Terrain, Color[] States) GeneratePixels()
    {
        var generator = new LandAndWaterGenerator(this.mapWidth, this.mapHeight, this.noiseScale, this.random, this.outerBoundaryXSize, this.outerBoundaryYSize);
        var noiseMap = generator.GenerateNoiseMap();
        this.Terrain = generator.GenerateLandAndWater(noiseMap, this.waterThreshold, this.beachThreshold, this.grassThreshold, this.mountainThreshold);

        var states = GenerateStates(this.Terrain);

        return (this.Terrain, states);
    }

    private Color[] GenerateStates(Color[] Terrain)
    {
        var states = new Color[Terrain.Length];
        Array.Copy(Terrain, states, Terrain.Length);

        var found = true;
        var startingPosition = new Vector2();

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
                var size = UnityEngine.Random.Range(40, 120);
                var colorsToReplace = new Color[] { Color.green, Color.yellow };
                PaintHelper.FloodPaint(states, this.mapWidth, this.mapHeight, startingPosition, colorsToReplace, PaintHelper.GenerateRandomColor(), size);
            }
        }

        return states;
    }
}