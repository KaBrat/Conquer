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

    private Color[] Terrain;

    public void GenerateMap()
    {
        Terrain = GeneratePixels(outerBoundaryXSize, outerBoundaryYSize);
        var map = TextureGenerator.SaveMap(Terrain, mapWidth, mapHeight, Application.dataPath + "/GeneratedMaps/LandMap.png");
        GetComponent<SpriteRenderer>().sprite = map;
    }

    private Color[] GeneratePixels(int outerXRange, int outerYRange)
    {
        var generator = new LandAndWaterGenerator(this.mapWidth, this.mapHeight, this.noiseScale, this.random, this.outerBoundaryXSize, this.outerBoundaryYSize);
        this.Terrain = generator.GenerateLandAndWater(this.waterThreshold, this.beachThreshold, this.grassThreshold, this.mountainThreshold);

        var found = true;
        var startingPosition = new Vector2();

        while (found)
        {
            found = false;

            for (var x = 0; x < mapWidth; x++)
            {
                for (var y = 0; y < mapHeight; y++)
                {
                    var pixelColor = this.Terrain[y * mapWidth + x];
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
                PaintHelper.FloodPaint(Terrain, mapWidth, mapHeight, startingPosition, Color.green, PaintHelper.GenerateRandomColor(), size);
            }
        }

        return this.Terrain;
    }
}