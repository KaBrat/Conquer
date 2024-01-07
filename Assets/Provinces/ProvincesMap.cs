using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ProvincesMap : MonoBehaviour
{
    public MapManager MapManager;
    public IProvinceDisplayer ProvinceDisplayer;

    public HashSet<Province> Provinces = new HashSet<Province>();
    public Province selectedProvince;
    public Province hoveredProvince;

    void Start()
    {
        ProvinceDisplayer = GameObject.FindObjectsOfType<InGameUI>().FirstOrDefault();
        InitProvinces();
    }

    private void InitProvinces()
    {
        var provincePixels = this.MapManager.ProvincesSprite.texture.GetPixels32();

        var provinceNames = GetRandomProvinceNames();
        var provinceColors = new HashSet<Color32>();
        var borderPixels = ColorHelper.ExtractColorsWithPositions(provincePixels, this.MapManager.mapSize.x, this.MapManager.mapSize.y, (color) => color.a == ColorHelper.borderAlpha);

        for (var x = 0; x < this.MapManager.mapSize.x; x++)
        {
            for (var y = 0; y < this.MapManager.mapSize.y; y++)
            {
                var pixelColor = provincePixels[y * this.MapManager.mapSize.x + x];

                var pixelIsUnselectableTerrain = ColorHelper.UnselectableTerrainColors.Any(tc => tc.Equals(pixelColor));
                if (pixelIsUnselectableTerrain)
                    continue;

                var pixelIsBorder = pixelColor.a == ColorHelper.borderAlpha;
                if (pixelIsBorder)
                    continue;

                var provinceAlreadyAdded = provinceColors.Contains(pixelColor);
                if (provinceAlreadyAdded)
                    continue;

                provinceColors.Add(pixelColor);
                var randomProvinceName = GetRandomProvinceName(provinceNames);
                var province = new Province(randomProvinceName, pixelColor);
                province.BorderPixels = borderPixels.Where(bp => ColorHelper.AreColorsEqualIgnoringAlpha(bp.Color, pixelColor)).ToList();
                this.Provinces.Add(province);
            }
        }

    }

    private List<string> GetRandomProvinceNames ()
    {
        return new List<string>
        {
            "Eldrathia",
            "Silverholm",
            "Misthaven",
            "Emberwold",
            "Frostspire",
            "Celestial Reach",
            "Shadowmere",
            "Stormwatch",
            "Crimson Valley",
            "Ironpeak",
            "Whispering Woods",
            "Serpent's Coil",
            "Golden Plains",
            "Moonshroud",
            "Thunderhelm",
            "Starfallen Reach",
            "Azure Haven",
            "Wyrmguard",
            "Obsidian Marches",
            "Sylvanheart"
        };
    }

    private string GetRandomProvinceName(List<string> provinceNames)
    {
        // Choose a random index
        var random = Random.Range(0, provinceNames.Count());

        // Get and remove the random fantasy province name
        var randomProvince = provinceNames[random];
        provinceNames.RemoveAt(random);

        return randomProvince;
    }

    public Province GetProvince()
    {
        var provinceColor = ColorHelper.GetColor(Camera.main);
        return Provinces.Where(p => p.Color.Equals(provinceColor)).FirstOrDefault();
    }
}