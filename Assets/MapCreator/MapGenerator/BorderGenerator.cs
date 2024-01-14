using System.Collections.Generic;
using UnityEngine;

public class BorderGenerator
{
    public int mapWidth;
    public int mapHeight;

    public byte borderAlpha = 200;

    public BorderGenerator(int mapWidth, int mapHeight)
    {
        this.mapWidth = mapWidth;
        this.mapHeight = mapHeight;
    }
    public void AddStateBordersToTerrain(Color32[] terrain, Color32[] provinces, HashSet<Color32> provinceColors)
    {
        foreach (var provinceColor in provinceColors)
        {
            var borderPixels = FindProvinceBorders(provinces, provinceColor);
            foreach (var borderPixel in borderPixels)
            {
                terrain[borderPixel.Position.y * mapWidth + borderPixel.Position.x] = ColorHelper.borderColor;
                provinces[borderPixel.Position.y * mapWidth + borderPixel.Position.x] = new Color32(provinceColor.r, provinceColor.g, provinceColor.b, this.borderAlpha);
            }
        }
    }

    public List<ColorWithPosition> FindProvinceBorders(Color32[] provincesMap, Color32 ProvinceColor)
    {
        var borderPixels = new List<ColorWithPosition>();
        for (var x = 0; x < mapWidth; x++)
        {
            for (var y = 0; y < mapHeight; y++)
            {
                var pixelColor = provincesMap[y * mapWidth + x];
                var isProvince = pixelColor.Equals(ProvinceColor);

                if (!isProvince)
                {
                    continue;
                }

                var neighbours = GetNeighbours(y * mapWidth + x);
                foreach (var neighbour in neighbours)
                {
                    var neighbourColor = provincesMap[neighbour];
                    var isPoliticalBorder = isProvince && !neighbourColor.Equals(ProvinceColor);

                    if (isPoliticalBorder)
                    {
                        borderPixels.Add(new ColorWithPosition(pixelColor, new Vector2Int(x, y)));
                    }
                }
            }
        }
        return borderPixels;
    }

    public List<int> GetNeighbours(int pixelIndex)
    {
        List<int> neighbours = new();

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
}