using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ProvincesMap : MonoBehaviour
{
    public List<Province> Provinces;
    public Texture2D mapImage;
    private static int mapDimension = 80;
    public HashSet<Color> provinceColors;
    private Vector3 size = new Vector3(mapDimension, mapDimension, 0);
    public IProvinceDisplayer ProvinceDisplayer;

    void Start()
    {
        ColorUtility.TryParseHtmlString("#" + "2ea7e8", out Color lightBlue);
        ColorUtility.TryParseHtmlString("#" + "b5e61d", out Color lightGreen);
        ColorUtility.TryParseHtmlString("#" + "e86c2e", out Color orange);
        Provinces = new List<Province>(){
            new Province("Eastwatch", lightGreen),
            new Province("Northguard", lightBlue),
            new Province("Summershore", orange)
        };

        ProvinceDisplayer = GameObject.FindObjectsOfType<InGameUI>().FirstOrDefault();
        var pixels = mapImage.GetPixels();
        provinceColors = new HashSet<Color>(pixels);
        provinceColors.Remove(Color.black);
        provinceColors.Remove(Color.white);

        foreach (var province in Provinces)
        {
            var match = provinceColors.First(pC => String.Equals(ColorUtility.ToHtmlStringRGB(pC), ColorUtility.ToHtmlStringRGB(province.Color), StringComparison.OrdinalIgnoreCase));
            province.Color = match;
        }
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            var clickedProvince = GetProvince(Input.mousePosition);
            ProvinceDisplayer.DisplayProvince(clickedProvince);
        }
    }

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

    public Province GetProvince(Vector3 mouseposition)
    {
        var color = GetRGBA();
        return Provinces.FirstOrDefault(p => String.Equals(ColorUtility.ToHtmlStringRGB(color), ColorUtility.ToHtmlStringRGB(p.Color), StringComparison.OrdinalIgnoreCase));
    }
}