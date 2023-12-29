using System.Collections.Generic;
using UnityEngine;

public static class ColorHelper
{
    public static bool ColorListContainsColor(List<Color> colorList, Color color)
    {
        // Check if the color is in the list (exact comparison)
        return colorList.Contains(color);
    }

    public static Color GetColor(Camera camera, Vector3 position)
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
                Color pixelColor = texture.GetPixel(x, y);

                // Extract RGB values
                int r = Mathf.RoundToInt(pixelColor.r * 255);
                int g = Mathf.RoundToInt(pixelColor.g * 255);
                int b = Mathf.RoundToInt(pixelColor.b * 255);

                // Print RGB values
                Debug.Log("Clicked on RGB: (" + r + ", " + g + ", " + b + ")");
                return pixelColor;
            }
        }
        return new Color();
    }
}