using UnityEngine;

// Helper class to store both color and position
public class ColorWithPosition
{
    public Color32 Color { get; private set; }
    public Vector2Int Position { get; private set; }

    public ColorWithPosition(Color32 color, Vector2Int position)
    {
        Color = color;
        Position = position;
    }
}