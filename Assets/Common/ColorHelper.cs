using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ColorHelper
{
    public static Color32 mountainGray = new(169, 169, 169, 255);
    public static Color32 riverBlue = new(41, 105, 255, 255);
    public static Color32 deepSeaBlue = new(0, 128, 255, 255);
    public static Color32 seaBlue = new(59, 154, 247, 255);
    public static Color32 shallowSeaBlue = new(119, 183, 247, 255);
    public static Color32 sandYellow = new(240, 230, 140, 255);
    public static Color32 snowWhite = new(255, 255, 255, 255);
    public static Color32 grassGreen = Color.green; //new Color32(0, 128, 0, 255);

    public static byte borderAlpha = 200;
    public static byte highlightedAlpha = 180;
    public static Color32 borderColor = new(59, 57, 43, 200);
    public static Color32 selectedBorderColor = new(184, 78, 37, 200);

    public static HashSet<Color32> PlayerColors = new()
    {
        new Color32(23, 0, 115, 255), // Darkish Sea Blue
        new Color32(25, 79, 30, 255), // Dark Forest Green
    };

    public static HashSet<Color32> RiverColors = new() { ColorHelper.riverBlue };
    public static HashSet<Color32> SeaColors = new() { ColorHelper.deepSeaBlue, ColorHelper.seaBlue, ColorHelper.shallowSeaBlue };
    public static HashSet<Color32> WaterColors = new(ColorHelper.SeaColors.Concat(ColorHelper.RiverColors));
    public static HashSet<Color32> MountainColors = new() { ColorHelper.mountainGray, ColorHelper.snowWhite };
    public static HashSet<Color32> SelectableTerrainColors = new() { ColorHelper.grassGreen, ColorHelper.sandYellow };

    public static HashSet<Color32> ColorsUsedInTerrain = new(ColorHelper.SelectableTerrainColors.Concat(ColorHelper.WaterColors).Concat(ColorHelper.MountainColors));
    public static HashSet<Color32> UnselectableTerrainColors = new(ColorHelper.MountainColors.Concat(ColorHelper.WaterColors));
    public static HashSet<Color32> TerrainObstacleColors = new(ColorHelper.MountainColors.Concat(ColorHelper.WaterColors));
    public static bool ColorListContainsColor(HashSet<Color32> colorList, Color32 color)
    {
        // Check if the color is in the list (exact comparison)
        return colorList.Contains(color);
    }

    public static Color32 GetColor(Map map, Vector2 mousePosition)
    {
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

        if (hit.collider == null)
            return new Color32();

        // Check if the clicked object has a SpriteRenderer
        SpriteRenderer spriteRenderer = hit.collider.GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
            return new Color32();

        // Calculate UV coordinates manually
        Vector2 localPoint = hit.transform.InverseTransformPoint(hit.point);
        Vector2 uv = new(
            Mathf.InverseLerp(hit.collider.bounds.min.x, hit.collider.bounds.max.x, localPoint.x),
            Mathf.InverseLerp(hit.collider.bounds.min.y, hit.collider.bounds.max.y, localPoint.y)
        );

        // Convert UV coordinates to pixel coordinates
        var mapSize = map.GetMapSize();
        int x = Mathf.RoundToInt(uv.x * mapSize.x);
        int y = Mathf.RoundToInt(uv.y * mapSize.y);

        // Get the color of the clicked pixel
        Color32 pixelColor = map.GetPixel(x, y);

        // Print RGB values
        //Debug.Log("Mouse over RGB: (" + pixelColor.r + ", " + pixelColor.g + ", " + pixelColor.b + ")");
        return pixelColor;
    }

    static int GetIndex(int x, int y, int mapWidth)
    {
        return y * mapWidth + x;
    }

    public static HashSet<ColorWithPosition> ExtractColorsWithPositions(Color32[] image, int mapWidth, int mapHeight, System.Func<Color32, bool> criteria)
    {
        var colorsWithPositions = new HashSet<ColorWithPosition>();

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

    public static Color32 GetBorderColor(Color32 color)
    {
        return new Color32(color.r, color.g, color.b, ColorHelper.borderAlpha);
    }

    public static Color32 GetHighlightedColor(Color32 color)
    {
        return new Color32(color.r, color.g, color.b, ColorHelper.highlightedAlpha);
    }

    public static Color32 GetOriginalProvinceColor(Color32 color)
    {
        return new Color32(color.r, color.g, color.b, 255);
    }

    public static Color32 AddNewRandomColorToList(HashSet<Color32> colorList)
    {
        Color32 randomColor;

        do
        {
            // Generate a random color
            randomColor = new Color32(
    (byte)Random.Range(0, 256),  // Random red component (0 to 255)
    (byte)Random.Range(0, 256),  // Random green component (0 to 255)
    (byte)Random.Range(0, 256),  // Random blue component (0 to 255)
    255                           // Fully opaque alpha component (255)
);

            // Check if the color is already in the list
        } while (ColorHelper.ColorListContainsColor(colorList, randomColor) || ColorHelper.ColorListContainsColor(ColorHelper.ColorsUsedInTerrain, randomColor));

        // Add the new color to the list if needed
        colorList.Add(randomColor);

        return randomColor;
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