using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

public class ProvincesGenerator
{
    public Color32[] TerrainMap;
    public ProvincesGenerator(Color32[] terrainMap)
    {
        this.TerrainMap = terrainMap;
    }

    public (Color32[] provinces, HashSet<Color32> provinceColors) GenerateProvinces(Vector2Int mapSize, int provincesMaxSize)
    {
        var provincesMap = new Color32[this.TerrainMap.Length];
        Array.Copy(this.TerrainMap, provincesMap, this.TerrainMap.Length);

        var provinceColors = new HashSet<Color32>();

        while (FindStartingPosition(provincesMap, mapSize, out Vector2Int startingPosition))
        {
            var size = UnityEngine.Random.Range(provincesMaxSize / 4, provincesMaxSize);
            var colorsToReplace = ColorHelper.SelectableTerrainColors.ToArray();
            var stateColor = ColorHelper.AddNewRandomColorToList(provinceColors);
            PaintHelper.FloodPaint(provincesMap, mapSize.x, mapSize.y, startingPosition, colorsToReplace, stateColor, size);
        }

        return (provincesMap, provinceColors);
    }

    private bool FindStartingPosition(Color32[] provincesMap, Vector2Int mapSize, out Vector2Int startingPosition)
    {
        var randomStartX = UnityEngine.Random.Range(0, mapSize.x - 1);
        var randomStartY = UnityEngine.Random.Range(0, mapSize.y - 1);

        for (var x = randomStartX; x < mapSize.x; x++)
        {
            if (findStartingPositionOnX(provincesMap, x, randomStartY, mapSize, out startingPosition))
                return true;
        }

        for (var x = 0; x < randomStartX; x++)
        {
            if (findStartingPositionOnX(provincesMap, x, randomStartY, mapSize, out startingPosition))
                return true;
        }

        startingPosition = Vector2Int.zero;
        return false;
    }

    public bool findStartingPositionOnX(Color32[] provincesMap, int x, int randomStartY, Vector2Int mapSize, out Vector2Int startingPosition)
    {
        for (var y = randomStartY; y < mapSize.y; y++)
        {
            var pixelColor = provincesMap[y * mapSize.x + x];
            if (ColorHelper.SelectableTerrainColors.Contains(pixelColor))
            {
                startingPosition = new Vector2Int(x, y);
                return true;
            }
        }
        for (var y = 0; y < randomStartY; y++)
        {
            var pixelColor = provincesMap[y * mapSize.x + x];
            if (ColorHelper.SelectableTerrainColors.Contains(pixelColor))
            {
                startingPosition = new Vector2Int(x, y);
                return true;
            }
        }

        startingPosition = Vector2Int.zero;
        return false;
    }
}