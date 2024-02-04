using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

public class ProvincesGenerator
{
    public Color32[] TerrainMap;
    public Vector2Int MapSize;
    public ProvincesGenerator(Color32[] terrainMap, Vector2Int mapSize)
    {
        this.TerrainMap = terrainMap;
        this.MapSize = mapSize;
    }

    public (Color32[] provinces, HashSet<Color32> provinceColors) GenerateProvinces(int provincesMaxSize)
    {
        var provincesMap = new Color32[this.TerrainMap.Length];
        Array.Copy(this.TerrainMap, provincesMap, this.TerrainMap.Length);

        var provinceColors = new HashSet<Color32>();

        while (FindStartingPosition(provincesMap, out Vector2Int startingPosition))
        {
            var size = UnityEngine.Random.Range(provincesMaxSize / 4, provincesMaxSize);
            var colorsToReplace = ColorHelper.SelectableTerrainColors.ToArray();
            var stateColor = ColorHelper.AddNewRandomColorToList(provinceColors);
            PaintHelper.PaintProvince(provincesMap, this.MapSize.x, this.MapSize.y, startingPosition, colorsToReplace, stateColor, size, out int pixelsPainted);
            if (pixelsPainted < 100)
            {
                var closestProvinceColor = FindClosestDifferentColor(provincesMap, startingPosition, stateColor, ColorHelper.UnselectableTerrainColors);

                for (int x = 0; x < this.MapSize.x; x++)
                {
                    for (int y = 0; y < this.MapSize.y; y++)
                    {
                        if (provincesMap[ColorArrayHelper.GetIndex(new Vector2Int(x, y), this.MapSize.x)].Equals(stateColor))
                        {
                            provincesMap[ColorArrayHelper.GetIndex(new Vector2Int(x, y), this.MapSize.x)] = closestProvinceColor;
                        }
                    }
                }
            }
        }

        return (provincesMap, provinceColors);
    }

    private bool FindStartingPosition(Color32[] provincesMap, out Vector2Int startingPosition)
    {
        var randomStartX = UnityEngine.Random.Range(0, this.MapSize.x - 1);
        var randomStartY = UnityEngine.Random.Range(0, this.MapSize.y - 1);

        for (var x = randomStartX; x < this.MapSize.x; x++)
        {
            if (findStartingPositionOnX(provincesMap, x, randomStartY, out startingPosition))
                return true;
        }

        for (var x = 0; x < randomStartX; x++)
        {
            if (findStartingPositionOnX(provincesMap, x, randomStartY, out startingPosition))
                return true;
        }

        startingPosition = Vector2Int.zero;
        return false;
    }

    public bool findStartingPositionOnX(Color32[] provincesMap, int x, int randomStartY, out Vector2Int startingPosition)
    {
        for (var y = randomStartY; y < this.MapSize.y; y++)
        {
            var pixelColor = provincesMap[y * this.MapSize.x + x];
            if (ColorHelper.SelectableTerrainColors.Contains(pixelColor))
            {
                startingPosition = new Vector2Int(x, y);
                return true;
            }
        }
        for (var y = 0; y < randomStartY; y++)
        {
            var pixelColor = provincesMap[y * this.MapSize.x + x];
            if (ColorHelper.SelectableTerrainColors.Contains(pixelColor))
            {
                startingPosition = new Vector2Int(x, y);
                return true;
            }
        }

        startingPosition = Vector2Int.zero;
        return false;
    }

    public Color32 FindClosestDifferentColor(Color32[] provincesMap, Vector2Int targetPosition, Color32 targetColor, HashSet<Color32> excludedColors)
    {
        int closestIndex = -1;
        float closestDistance = float.MaxValue;

        for (int y = 0; y < this.MapSize.y; y++)
        {
            for (int x = 0; x < this.MapSize.x; x++)
            {
                int currentIndex = ColorArrayHelper.GetIndex(new Vector2Int(x, y), this.MapSize.x);
                Color32 currentColor = provincesMap[currentIndex];

                // Check if the current pixel has a different color
                if (!currentColor.Equals(targetColor) && !excludedColors.Contains(currentColor))
                {
                    // Calculate distance squared (squared distance is enough for comparison)
                    float distanceSquared = (targetPosition - new Vector2Int(x, y)).sqrMagnitude;

                    // Update closest pixel if the current one is closer
                    if (distanceSquared < closestDistance)
                    {
                        closestDistance = distanceSquared;
                        closestIndex = currentIndex;
                    }
                }
            }
        }

        // Convert the index back to 2D coordinates
        int closestX = closestIndex % this.MapSize.x;
        int closestY = closestIndex / this.MapSize.x;

        var position = new Vector2Int(closestX, closestY);
        return provincesMap[ColorArrayHelper.GetIndex(position, this.MapSize.x)];
    }

}