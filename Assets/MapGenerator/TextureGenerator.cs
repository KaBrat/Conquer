using System.IO;
using UnityEngine;

public class TextureGenerator
{
    public static Sprite SaveMap(Color[] pixels, int mapWidth, int mapHeight, string path)
    {
        Texture2D landTexture;
        Sprite sprite;
        SetTexture(pixels, mapWidth, mapHeight, out landTexture, out sprite);

        byte[] pngBytes = landTexture.EncodeToPNG();
        File.WriteAllBytes(path, pngBytes);

        return sprite;
    }

    public static void SetTexture(Color[] pixels, int mapWidth, int mapHeight, out Texture2D landTexture, out Sprite sprite)
    {
        landTexture = new Texture2D(mapWidth, mapHeight);
        landTexture.SetPixels(pixels);
        landTexture.filterMode = FilterMode.Point;
        landTexture.Apply();

        sprite = Sprite.Create(landTexture, new Rect(0, 0, landTexture.width, landTexture.height), Vector2.one * 0.5f);
    }
}

