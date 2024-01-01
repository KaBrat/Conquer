using System.Collections.Generic;
using UnityEngine;

public static class ColorHelper
{
    public static Color32 gray = new Color32(128, 128, 128, 255);
    public static Color32 blue = new Color32(0, 128, 255, 255);
    public static Color32 borderColor = new Color32(59, 57, 43, 200);
    public static Color32 selectedBorderColor = new Color32(184, 78, 37, 200);

    public static List<Color32> ColorsUsedInTerrain = new List<Color32>() {Color.green, ColorHelper.blue, Color.white, ColorHelper.gray, Color.yellow };
    public static List<Color32> TerrainObstacleColors = new List<Color32>() { ColorHelper.blue, ColorHelper.gray};
    public static bool ColorListContainsColor(List<Color32> colorList, Color32 color)
    {
        // Check if the color is in the list (exact comparison)
        return colorList.Contains(color);
    }

    public static Color32 GetColor(Camera camera)
    {
        Vector2 clickPosition = camera.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(clickPosition, Vector2.zero);

        if (hit.collider != null)
        {
            // Check if the clicked object has a SpriteRenderer
            SpriteRenderer spriteRenderer = hit.collider.GetComponent<SpriteRenderer>();

            if (spriteRenderer != null)
            {
                // Get the texture from the sprite
                Texture2D texture = spriteRenderer.sprite.texture;

                texture = ImageHelper.LoadImageFromDisk(texture.width, texture.height, ImageHelper.ProvincesMapPath).texture;

                // Calculate UV coordinates manually
                Vector2 localPoint = hit.transform.InverseTransformPoint(hit.point);
                Vector2 uv = new Vector2(
                    Mathf.InverseLerp(hit.collider.bounds.min.x, hit.collider.bounds.max.x, localPoint.x),
                    Mathf.InverseLerp(hit.collider.bounds.min.y, hit.collider.bounds.max.y, localPoint.y)
                );

                // Convert UV coordinates to pixel coordinates
                int x = Mathf.RoundToInt(uv.x * texture.width);
                int y = Mathf.RoundToInt(uv.y * texture.height);

                // Get the color of the clicked pixel
                Color32 pixelColor = texture.GetPixel(x, y);

                // Print RGB values
                Debug.Log("Clicked on RGB: (" + pixelColor.r + ", " + pixelColor.g + ", " + pixelColor.b + ")");
                return pixelColor;
            }
        }
        return new Color32();
    }

    static int GetIndex(int x, int y, int mapWidth)
    {
        return y * mapWidth + x;
    }

    public static List<ColorWithPosition> ExtractColorsWithPositions(Color32[] image, int mapWidth, int mapHeight, System.Func<Color32, bool> criteria)
    {
        var colorsWithPositions = new List<ColorWithPosition>();

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                int index = GetIndex(x, y, mapWidth);
                var currentColor = image[index];

                if (criteria(currentColor))
                {
                    colorsWithPositions.Add(new ColorWithPosition(currentColor, new Vector2Int(x, y)));
                }
            }
        }

        return colorsWithPositions;
    }

    public static bool AreColorsEqualIgnoringAlpha(Color32 color1, Color32 color2)
    {
        return color1.r == color2.r && color1.g == color2.g && color1.b == color2.b;
    }
}

//private Color GetRGBA()
//{
//    var screenPosition = Input.mousePosition;
//    screenPosition.z = Camera.main.nearClipPlane + 1;
//    var worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
//    var mapPosition = new Vector3(worldPosition.x * 100 + 0.5f * mapDimension, worldPosition.y * 100 + 0.5f * mapDimension, 0);

//    int x = Mathf.FloorToInt(mapPosition.x / size.x * mapImage.width);
//    int y = Mathf.FloorToInt(mapPosition.y / size.y * mapImage.height);
//    var pixel = mapImage.GetPixel(x, y);
//    return pixel;
//}