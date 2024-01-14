using System.IO;
using UnityEngine;

public class ImageHelper
{
    public static string TerrainMapPath = Application.dataPath + "/GeneratedMaps/Terrain.png";
    public static string ProvincesMapPath = Application.dataPath + "/GeneratedMaps/States.png";

    public static Color32[] LoadTerrainPixels()
    {
        var sprite = LoadImageFromDisk(1, 1, TerrainMapPath);
        return sprite.texture.GetPixels32();
    }

    public static void SaveTerrainPixels(Color32[] pixels, Vector2Int mapSize)
    {
        Texture2D texture = new(mapSize.x, mapSize.y);
        texture.SetPixels32(pixels);
        texture.Apply();
        SaveMap(texture, TerrainMapPath);
    }

    public static Color32[] LoadProvincesPixels()
    {
        var sprite = LoadImageFromDisk(1, 1, ProvincesMapPath);
        return sprite.texture.GetPixels32();
    }

    public static void SaveProvincesPixels(Color32[] pixels, Vector2Int mapSize)
    {
        Texture2D texture = new(mapSize.x, mapSize.y);
        texture.SetPixels32(pixels);
        texture.Apply();
        SaveMap(texture, ProvincesMapPath);
    }

    public static void SaveMap(Texture2D texture, string path)
    {
        byte[] pngBytes = texture.EncodeToPNG();
        File.WriteAllBytes(path, pngBytes);
    }

    public static Sprite LoadImageFromDisk(int width, int height, string path)
    {
        // Check if the file exists
        if (System.IO.File.Exists(path))
        {
            // Read the bytes from the file
            byte[] fileData = System.IO.File.ReadAllBytes(path);

            // Create a new Texture2D and load the image data
            Texture2D texture = new(width, height); // Adjust the size as needed
            texture.LoadImage(fileData);

            // Create a Sprite using the loaded texture
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
        }
        else
        {
            Debug.LogError($"File not found: {path}");
            return null;
        }
    }

    public static Sprite CreateSprite(Texture2D texture)
    {
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
    }

    public static Texture2D CreateTexture2D (Color32[] pixels, int width, int height)
    {
        var texture = new Texture2D(width, height);
        texture.filterMode = FilterMode.Point;
        texture.SetPixels32(pixels);
        texture.Apply();
        return texture;
    }
}

