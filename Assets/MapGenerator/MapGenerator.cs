using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Header("Map Generation Settings")]
    [SerializeField, Range(0.1f, 10f)] private float noiseScale = 4f; 
    [SerializeField, Range(0f, 1f)] private float threshold = 0.45f;
    [SerializeField, Range(0f, 500f)] private float random = 50f;
    [SerializeField, Range(100, 3000)] private int mapWidth = 500;
    [SerializeField, Range(100, 3000)] private int mapHeight = 300;
    [SerializeField, Range(0, 50)] private int outerBoundaryXSize = 10;
    [SerializeField, Range(0, 50)] private int outerBoundaryYSize = 10;

    public void GenerateMap()
    {
        Color[] pixels = GeneratePixels(mapWidth, mapHeight, outerBoundaryXSize, outerBoundaryYSize, noiseScale, random, threshold);
        var map = TextureGenerator.SaveMap(pixels, mapWidth, mapHeight, Application.dataPath + "/GeneratedMaps/LandMap.png");
        GetComponent<SpriteRenderer>().sprite = map;
    }

    private Color[] GeneratePixels(int mapWidth, int mapHeight, int outerXRange, int outerYRange, float noiseScale, float random, float threshold)
    {
        var pixels = GenerateLandAndWater(mapWidth, mapHeight, outerXRange, outerYRange, noiseScale, random, threshold);
        pixels = GenerateStates(pixels);

        return pixels;
    }

    private Color[] GenerateLandAndWater(int mapWidth, int mapHeight, int outerXRange, int outerYRange, float noiseScale, float random, float threshold)
    {
        var offsetX = UnityEngine.Random.Range(-random, random);
        var offsetY = UnityEngine.Random.Range(-random, random);

        var pixels = new Color[mapWidth * mapHeight];

        for (var y = 0; y < mapHeight; y++)
        {
            for (var x = 0; x < mapWidth; x++)
            {
                var outerBoundarySmoothFactor = GetOuterBoundarySmoothFactor(mapWidth, mapHeight, outerXRange, outerYRange, y, x);
                var sampleX = x * noiseScale / mapWidth + offsetX;
                var sampleY = y * noiseScale / mapHeight + offsetY;
                var noiseValue = Mathf.PerlinNoise(sampleX, sampleY) * outerBoundarySmoothFactor;
                pixels[y * mapWidth + x] = noiseValue >= threshold ? Color.green : Color.blue;
            }
        }

        return pixels;
    }

    private Color[] GenerateStates(Color[] pixels)
    {
        // Create a list of state colors
        List<Color> stateColors = new List<Color>();

        // Create a dictionary to store the centroids of each state color
        Dictionary<Color, Vector2> stateCentroids = new Dictionary<Color, Vector2>();

        // Create a spatial grid to group nearby pixels
        int gridSize = 30; // Adjust the grid size based on your map and performance requirements
        int gridWidth = Mathf.CeilToInt(mapWidth / (float)gridSize);
        int gridHeight = Mathf.CeilToInt(mapHeight / (float)gridSize);
        Dictionary<Vector2Int, Color> spatialGrid = new Dictionary<Vector2Int, Color>();

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                // Skip water pixels
                if (pixels[y * mapWidth + x] != Color.green)
                    continue;

                // Find the nearest state color using the spatial grid
                Color nearestColor = Color.blue;
                float nearestDistance = float.MaxValue;

                Vector2Int gridPosition = new Vector2Int(x / gridSize, y / gridSize);
                for (int dx = -1; dx <= 1; dx++)
                {
                    for (int dy = -1; dy <= 1; dy++)
                    {
                        Vector2Int neighborGridPosition = gridPosition + new Vector2Int(dx, dy);
                        if (spatialGrid.TryGetValue(neighborGridPosition, out Color stateColor))
                        {
                            float distance = Vector2.Distance(new Vector2(x, y), stateCentroids[stateColor]);
                            if (distance < nearestDistance)
                            {
                                nearestDistance = distance;
                                nearestColor = stateColor;
                            }
                        }
                    }
                }

                // Assign the pixel to the nearest state color
                pixels[y * mapWidth + x] = nearestColor;

                // If the nearest color was not found within a certain range, create a new state
                var randomValue = UnityEngine.Random.Range(60, 150);
                if (nearestDistance > randomValue)
                {
                    Color randomColor = RandomColor();
                    stateColors.Add(randomColor);
                    stateCentroids[randomColor] = new Vector2(x, y);
                    spatialGrid[gridPosition] = randomColor;
                }
            }
        }

        return pixels;
    }



    private Vector2 FindStateCentroid(Color stateColor, Color[] pixels)
    {
        List<Vector2> points = new List<Vector2>();
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                if (pixels[y * mapWidth + x] == stateColor)
                    points.Add(new Vector2(x, y));
            }
        }

        if (points.Count == 0)
            return Vector2.zero;

        Vector2 centroid = Vector2.zero;
        foreach (Vector2 point in points)
        {
            centroid += point;
        }

        centroid /= points.Count;
        return centroid;
    }

    private Color RandomColor()
    {
        return new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
    }

    private float GetOuterBoundarySmoothFactor(int mapWidth, int mapHeight, int outerXRange, int outerYRange, int y, int x)
    {
        bool isOuterXPixel = IsOnOuterBoundaries(x, outerXRange, mapWidth);
        bool isOuterYPixel = IsOnOuterBoundaries(y, outerYRange, mapHeight);

        if (!isOuterXPixel && !isOuterYPixel)
            return 1f;

        int xDistanceToMaxInner = CalculateDistanceToMaxInnerBoundary(x, outerXRange, mapWidth);
        int yDistanceToMaxInner = CalculateDistanceToMaxInnerBoundary(y, outerYRange, mapHeight);

        if (isOuterXPixel && isOuterYPixel)
        {
            return xDistanceToMaxInner >= yDistanceToMaxInner ?
                CalculateOuterBoundarySmoothFactor(xDistanceToMaxInner, outerXRange, mapWidth) :
                CalculateOuterBoundarySmoothFactor(yDistanceToMaxInner, outerYRange, mapHeight);
        }

        return isOuterXPixel ?
            CalculateOuterBoundarySmoothFactor(xDistanceToMaxInner, outerXRange, mapWidth) :
            CalculateOuterBoundarySmoothFactor(yDistanceToMaxInner, outerYRange, mapHeight);
    }

    private bool IsOnOuterBoundaries(int pixelValue, int outerRange, int max)
    {
        return outerRange != 0 && (pixelValue <= (1f / outerRange) * max || pixelValue >= max - (1f / outerRange) * max);
    }

    private int CalculateDistanceToMaxInnerBoundary(int pixelValue, int outerRange, int max)
    {
        int maxInner = outerRange == 0 ? max / 2 : pixelValue < max / 2 ? (int)(1f / outerRange * max) : (int)(max - 1f / outerRange * max);
        return Mathf.Abs(pixelValue - maxInner);
    }

    private float CalculateOuterBoundarySmoothFactor(int distanceToMaxInner, int outerRange, int max)
    {
        var maxDistance = (1f / outerRange) * max;
        var relation = distanceToMaxInner / maxDistance;
        return 1f - relation;
    }
}