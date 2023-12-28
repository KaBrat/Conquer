using System.IO;
using UnityEngine;

public class ImageHelper
{
    public static void SaveMap(Texture2D texture, string path)
    {
        byte[] pngBytes = texture.EncodeToPNG();
        File.WriteAllBytes(path, pngBytes);
    }

    public static Sprite CreateSprite(Texture2D texture)
    {
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
    }

    public static Texture2D CreateTexture2D (Color[] pixels, int width, int height)
    {
        var texture = new Texture2D(width, height);
        texture.SetPixels(pixels);
        texture.filterMode = FilterMode.Point;
        texture.Apply();
        return texture;
    }
}

