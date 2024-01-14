using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;

public class ProvincesManager
{
    public MapManager MapManager;
    public IProvinceDisplayer ProvinceDisplayer;

    public HashSet<Province> Provinces = new();
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

    private Dictionary<Color32, (HashSet<int> provincePixels, HashSet<int> borderPixels)> GetProvinceAndBorderPixelsDict()
    {
        var provincePixels = this.MapManager.ProvinceMap.GetPixels32();
        var tmpProvinces = new Dictionary<Color32, (HashSet<int> provincePixels, HashSet<int> borderPixels)>();

        var lockObject = new object();

        Parallel.For(0, this.MapManager.mapSize.x, x =>
        {
            for (var y = 0; y < this.MapManager.mapSize.y; y++)
            {
                var colorIndex = y * this.MapManager.mapSize.x + x;
                var pixelColor = provincePixels[colorIndex];

                if (ColorHelper.UnselectableTerrainColors.Contains(pixelColor))
                    continue;

                var pixelIsBorder = pixelColor.a == ColorHelper.borderAlpha;
                var provinceColor = ColorHelper.GetOriginalProvinceColor(pixelColor);


                lock (lockObject)
                {
                    if (!tmpProvinces.TryGetValue(provinceColor, out var provinceInfo))
                    {
                        provinceInfo = (new HashSet<int>(), new HashSet<int>());
                        tmpProvinces.Add(provinceColor, provinceInfo);
                    }

                    if (pixelIsBorder)
                        provinceInfo.borderPixels.Add(colorIndex);
                    else
                        provinceInfo.provincePixels.Add(colorIndex);
                }
            }
        });

        return tmpProvinces;
    }

    public Province GetProvince()
    {
        Vector2 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var provinceColor = ColorHelper.GetColor(this.MapManager.ProvinceMap, clickPosition);
        return Provinces.FirstOrDefault(p => p.Color.Equals(provinceColor));
    }
}