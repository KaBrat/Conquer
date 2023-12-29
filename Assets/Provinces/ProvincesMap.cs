using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.EventSystems;

public class ProvincesMap : MonoBehaviour
{
    public List<Province> Provinces;
    public Texture2D mapImage;
    public HashSet<Color> provinceColors = new HashSet<Color>();

    public IProvinceDisplayer ProvinceDisplayer;

    void Start()
    {
        mainCamera = Camera.main;
        //ColorUtility.TryParseHtmlString("#" + "2ea7e8", out Color lightBlue);
        //ColorUtility.TryParseHtmlString("#" + "b5e61d", out Color lightGreen);
        //ColorUtility.TryParseHtmlString("#" + "e86c2e", out Color orange);
        //Provinces = new List<Province>(){
        //    new Province("Eastwatch", lightGreen),
        //    new Province("Northguard", lightBlue),
        //    new Province("Summershore", orange)
        //};

        ProvinceDisplayer = GameObject.FindObjectsOfType<InGameUI>().FirstOrDefault();
        var pixels = this.mapImage.GetPixels();

        Provinces = new List<Province>();
        var terrainColors = new HashSet<Color>() { Color.blue, Color.gray, Color.white};

        for (var x = 0; x < this.mapImage.width; x++)
        {
            for (var y = 0; y < this.mapImage.height; y++)
            {
                var pixelColor = pixels[y * this.mapImage.width + x];
                if (!terrainColors.Any(tc => tc.Equals(pixelColor)))
                {
                    provinceColors.Add(pixelColor);
                }
            }
        }

        //provinceColors = new HashSet<Color>(pixels);
        //provinceColors.Remove(Color.black);
        //provinceColors.Remove(Color.white);

        //foreach (var province in Provinces)
        //{
        //    var match = provinceColors.First(pC => String.Equals(ColorUtility.ToHtmlStringRGB(pC), ColorUtility.ToHtmlStringRGB(province.Color), StringComparison.OrdinalIgnoreCase));
        //    province.Color = match;
        //}
    }

    private Camera mainCamera;
    void Update()
    {
        if (Input.anyKeyDown)
        {
            Vector2 clickPosition = this.mainCamera.ScreenToWorldPoint(Input.mousePosition);

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
                }
            }

            //var pixel = GetRGBA();
            //Debug.Log(pixel);
            //var clickedProvince = GetProvince(Input.mousePosition);
            //ProvinceDisplayer.DisplayProvince(clickedProvince);
        }
    }

    //public Province GetProvince(Vector3 mouseposition)
    //{
    //    var color = GetRGBA();
    //    return Provinces.FirstOrDefault(p => String.Equals(ColorUtility.ToHtmlStringRGB(color), ColorUtility.ToHtmlStringRGB(p.Color), StringComparison.OrdinalIgnoreCase));
    //}

    //public Province GetProvince(Vector3 mouseposition)
    //{
    //    var color = GetRGBA();
    //    return Provinces.FirstOrDefault(p => String.Equals(ColorUtility.ToHtmlStringRGB(color), ColorUtility.ToHtmlStringRGB(p.Color), StringComparison.OrdinalIgnoreCase));
    //}

    private static int mapDimension = 80;
    private Vector3 size = new Vector3(mapDimension, mapDimension, 0);

    private Color GetRGBA()
    {
        var screenPosition = Input.mousePosition;
        screenPosition.z = Camera.main.nearClipPlane + 1;
        var worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        var mapPosition = new Vector3(worldPosition.x * 100 + 0.5f * mapDimension, worldPosition.y * 100 + 0.5f * mapDimension, 0);

        int x = Mathf.FloorToInt(mapPosition.x / size.x * mapImage.width);
        int y = Mathf.FloorToInt(mapPosition.y / size.y * mapImage.height);
        var pixel = mapImage.GetPixel(x, y);
        return pixel;
    }
}