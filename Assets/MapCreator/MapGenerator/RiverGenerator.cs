using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RiverGenerator
{
    private Color32[] mapPixels;
    private Vector2Int mapSize;
    float[,] heightMap;
    float[,] riverHeightMap;

    public RiverGenerator(float[,] heightMap, Color32[] mapPixels, Vector2Int mapSize)
    {
        this.heightMap = heightMap;
        this.mapPixels = mapPixels;
        this.mapSize = mapSize;
        this.riverHeightMap = CopyHeightMap(this.heightMap);
    }

    public void DrawRivers()
    {
        var highTerrain = this.mapPixels
            .Select((color, index) => new ColorWithPosition(color, ColorArrayHelper.GetPosition(index, this.mapSize.x)))
            .Where(cp => cp.Color.Equals(ColorHelper.mountainGray))
            .ToList();
        if (highTerrain.Count < 0)
            return;

        int randomHighPixelRandom;
        ColorWithPosition randomHighPixel;

        for (int i = 1; i <= 30; i++)
        {
            randomHighPixelRandom = UnityEngine.Random.Range(0, highTerrain.Count - 1);
            randomHighPixel = highTerrain[randomHighPixelRandom];
            DrawRiver(randomHighPixel.Position);
        }
    }

    private void DrawRiver(Vector2Int StartingPoint)
    {
        this.mapPixels[ColorArrayHelper.GetIndex(StartingPoint, this.mapSize.x)] = ColorHelper.riverBlue;

        var lowestNeighbor = FindWeightedRandomLowestNeighbor(StartingPoint);
        this.mapPixels[ColorArrayHelper.GetIndex(lowestNeighbor, this.mapSize.x)] = ColorHelper.riverBlue;

        var reachedEnd = false;
        while (!reachedEnd)
        {
            lowestNeighbor = FindWeightedRandomLowestNeighbor(lowestNeighbor);
            if (lowestNeighbor == Vector2Int.zero)
                reachedEnd = true;

            if (ColorHelper.WaterColors.Contains(this.mapPixels[ColorArrayHelper.GetIndex(lowestNeighbor, this.mapSize.x)]))
                reachedEnd = true;
            this.mapPixels[ColorArrayHelper.GetIndex(lowestNeighbor, this.mapSize.x)] = ColorHelper.riverBlue;
        }
    }

    public Vector2Int FindWeightedRandomLowestNeighbor(Vector2Int position)
    {
        int mapWidth = this.heightMap.GetLength(0);
        int mapHeight = this.heightMap.GetLength(1);

        float currentHeight = this.heightMap[position.x, position.y];
        List<Vector2Int> lowerNeighbors = new List<Vector2Int>();
        List<float> weights = new List<float>();

        for (int xOffset = -1; xOffset <= 1; xOffset++)
        {
            for (int yOffset = -1; yOffset <= 1; yOffset++)
            {
                // Skip the center position (current position)
                if (xOffset == 0 && yOffset == 0)
                    continue;

                int neighborX = position.x + xOffset;
                int neighborY = position.y + yOffset;

                // Check if the neighbor is within bounds
                if (neighborX >= 0 && neighborX < mapWidth && neighborY >= 0 && neighborY < mapHeight)
                {
                    float neighborHeight = this.heightMap[neighborX, neighborY];

                    // Check if the neighbor is lower than the current position
                    if (neighborHeight < currentHeight)
                    {
                        float weight = Mathf.Pow(currentHeight - neighborHeight, 2); // You can adjust the exponent for different weighting
                        lowerNeighbors.Add(new Vector2Int(neighborX, neighborY));
                        weights.Add(weight);
                    }
                }
            }
        }

        // Randomly choose one neighbor from the list of lower neighbors based on weights
        if (lowerNeighbors.Count > 0)
        {
            float totalWeight = weights.Sum();
            float randomValue = UnityEngine.Random.Range(0f, totalWeight);

            float cumulativeWeight = 0f;
            for (int i = 0; i < lowerNeighbors.Count; i++)
            {
                cumulativeWeight += weights[i];
                if (randomValue <= cumulativeWeight)
                {
                    return lowerNeighbors[i];
                }
            }
        }

        // If no lower neighbors are found, return the current position
        return position;
    }

    public float[,] CopyHeightMap(float[,] source)
    {
        int width = source.GetLength(0);
        int height = source.GetLength(1);

        float[,] destination = new float[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                destination[i, j] = source[i, j];
            }
        }

        return destination;
    }
}