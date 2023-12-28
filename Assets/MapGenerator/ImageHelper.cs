using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class ImageHelper
{
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
            Texture2D texture = new Texture2D(width, height); // Adjust the size as needed
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

    public static Texture2D CreateTexture2D (Color[] pixels, int width, int height)
    {
        var texture = new Texture2D(width, height);
        texture.SetPixels(pixels);
        texture.filterMode = FilterMode.Point;
        texture.Apply();
        return texture;
    }
}

