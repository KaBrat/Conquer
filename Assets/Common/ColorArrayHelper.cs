using Unity.Mathematics;
using UnityEngine;

public static class ColorArrayHelper
{
    public static int GetIndex(Vector2Int position, int mapWidth)
    {
        return position.y * mapWidth + position.x;
    }

    public static int GetIndex(int x, int y, int mapWidth)
    {
        return y * mapWidth + x;
    }

    public static Color32 GetColor(Color32[] ColorArray, int x, int y, int mapWidth)
    {
        return ColorArray[GetIndex(x, y, mapWidth)];
    }

    public static Vector2Int GetPosition(int index, int width)
    {
        int x = index % width;
        int y = index / width;
        return new Vector2Int(x, y);
    }
}