using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ProvincesMap : MonoBehaviour
{
    public List<Province> Provinces = new List<Province>(){
        new Province("Eastwatch", "b5e61d"),
        new Province("Northguard","2ea7e8"),
        new Province("Summershore","e86c2e")
    };

    public Texture2D mapImage;
    private static int mapDimension = 80;
    public HashSet<Color> provinceColors;
    private Vector3 size = new Vector3(mapDimension, mapDimension, 0);
    public IProvinceDisplayer ProvinceDisplayer;

    void Start()
    {
        ProvinceDisplayer = GameObject.FindObjectsOfType<InGameUI>().FirstOrDefault();
        var pixels = mapImage.GetPixels();
        provinceColors = new HashSet<Color>(pixels);
        provinceColors.Remove(Color.black);
        provinceColors.Remove(Color.white);
        Debug.Log(provinceColors.Count);

        foreach (var province in Provinces)
        {
            var match = provinceColors.First(p => String.Equals(ColorUtility.ToHtmlStringRGB(p), province.Colorhex, StringComparison.OrdinalIgnoreCase));
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
        return Provinces.FirstOrDefault(p => String.Equals(ColorUtility.ToHtmlStringRGB(color), p.Colorhex, StringComparison.OrdinalIgnoreCase));
    }
}