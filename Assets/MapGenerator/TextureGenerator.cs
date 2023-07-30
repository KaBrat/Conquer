using System.IO;
using UnityEngine;

public class TextureGenerator
{
    public static Sprite SaveMap(Color[] pixels, int mapWidth, int mapHeight, string path)
    {
        var landTexture = new Texture2D(mapWidth, mapHeight);
        landTexture.SetPixels(pixels);
        landTexture.Apply();

        var sprite = Sprite.Create(landTexture, new Rect(0, 0, landTexture.width, landTexture.height), Vector2.one * 0.5f);

        byte[] pngBytes = landTexture.EncodeToPNG();
        File.WriteAllBytes(path, pngBytes);

        return sprite;
    }
}

