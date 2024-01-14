using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ProvincesManager
{
    public MapManager MapManager;
    public IProvinceDisplayer ProvinceDisplayer;

    public HashSet<Province> Provinces = new HashSet<Province>();
    public Province selectedProvince;
    public Province hoveredProvince;

    public ProvincesManager(MapManager mapManager)
    {
        this.MapManager = mapManager;
    }

    public void LoadAndSetProvincesByPixelColor()
    {
        var provinceNames = ProvinceNames.GetRandomProvinceNames();
        var provinceAndBorderPixelsDict = GetProvinceAndBorderPixelsDict();
        foreach (var entry in provinceAndBorderPixelsDict)
        {
            var provinceName = ProvinceNames.GetRandomProvinceNameAndRemove(provinceNames);

            var province = new Province(provinceName, entry.Key, entry.Value.provincePixels.ToArray(), entry.Value.borderPixels.ToArray());
            this.Provinces.Add(province);
        }
    }

    private Dictionary<Color32, (List<int> provincePixels, List<int> borderPixels)> GetProvinceAndBorderPixelsDict()
    {
        var provincePixels = this.MapManager.ProvinceMap.GetPixels32();
        var tmpProvinces = new Dictionary<Color32, (List<int> provincePixels, List<int> borderPixelss)>();

        for (var x = 0; x < this.MapManager.mapSize.x; x++)
        {
            for (var y = 0; y < this.MapManager.mapSize.y; y++)
            {
                var colorIndex = y * this.MapManager.mapSize.x + x;
                var pixelColor = provincePixels[colorIndex];

                var pixelIsUnselectableTerrain = ColorHelper.UnselectableTerrainColors.Any(tc => tc.Equals(pixelColor));
                if (pixelIsUnselectableTerrain)
                    continue;

                var pixelIsBorder = pixelColor.a == ColorHelper.borderAlpha;
                var provinceColor = ColorHelper.GetOriginalProvinceColor(pixelColor);

                var provinceNotAddedYet = !tmpProvinces.ContainsKey(provinceColor);
                if (provinceNotAddedYet)
                    tmpProvinces.Add(provinceColor, (new List<int>(), new List<int>()));

                if (pixelIsBorder)
                    tmpProvinces[provinceColor].borderPixelss.Add(colorIndex);
                else
                    tmpProvinces[provinceColor].provincePixels.Add(colorIndex);
            }
        }
        return tmpProvinces;
    }

    public Province GetProvince()
    {
        Vector2 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var provinceColor = ColorHelper.GetColor(this.MapManager.ProvinceMap, clickPosition);
        return Provinces.Where(p => p.Color.Equals(provinceColor)).FirstOrDefault();
    }
}