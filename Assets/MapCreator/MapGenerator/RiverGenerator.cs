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

    public void DrawRivers(int amount)
    {
        var highTerrain = this.mapPixels
            .Select((color, index) => new ColorWithPosition(color, ColorArrayHelper.GetPosition(index, this.mapSize.x)))
            .Where(cp => cp.Color.Equals(ColorHelper.mountainGray))
            .ToList();
        if (highTerrain.Count <= 0)
            return;

        int randomHighPixelRandom;
        ColorWithPosition randomHighPixel;

        for (int i = 1; i <= amount; i++)
        {
            randomHighPixelRandom = UnityEngine.Random.Range(0, highTerrain.Count - 1);
            randomHighPixel = highTerrain[randomHighPixelRandom];
            DrawRiver(randomHighPixel.Position);
        }
    }

    public void DrawRiver(Vector2Int startPosition)
    {
        mapPixels[ColorArrayHelper.GetIndex(startPosition, this.mapSize.x)] = ColorHelper.riverBlue;
        var currentPosition = startPosition;

        var endCondition = false;
        bool lowerNeighbourFound;

        while (!endCondition)
        {

            (lowerNeighbourFound, currentPosition) = TryFindLowestNeighborPosition(currentPosition);

            if (!lowerNeighbourFound)
            {
                endCondition = true;
                break;
            }

            var pixelColor = mapPixels[ColorArrayHelper.GetIndex(currentPosition, this.mapSize.x)];
            var isSeaPixel = ColorHelper.SeaColors.Contains(pixelColor);

            if (isSeaPixel)
            {
                endCondition = true;
                break;
            }

            mapPixels[ColorArrayHelper.GetIndex(currentPosition, this.mapSize.x)] = ColorHelper.riverBlue;
        }
    }

    public (bool found, Vector2Int position) TryFindLowestNeighborPosition(Vector2Int position)
    {
        int x = position.x;
        int y = position.y;

        int sizeX = heightMap.GetLength(0);
        int sizeY = heightMap.GetLength(1);

        float lowestHeight = float.MaxValue;
        Vector2Int lowestNeighborPosition = Vector2Int.zero;
        bool lowerNeighborFound = false;

        for (int i = x - 1; i <= x + 1; i++)
        {
            for (int j = y - 1; j <= y + 1; j++)
            {
                // Check if the neighbor is within the heightmap bounds
                if (i >= 0 && i < sizeX && j >= 0 && j < sizeY)
                {
                    // Skip the center point (the original point)
                    if (i == x && j == y)
                        continue;

                    float neighborHeight = heightMap[i, j];

                    // Update lowestHeight and lowestNeighborPosition if the current neighbor is lower
                    if (neighborHeight < lowestHeight)
                    {
                        lowestHeight = neighborHeight;
                        lowestNeighborPosition = new Vector2Int(i, j);
                        lowerNeighborFound = true;
                    }
                }
            }
        }

        return (lowerNeighborFound, lowestNeighborPosition);
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