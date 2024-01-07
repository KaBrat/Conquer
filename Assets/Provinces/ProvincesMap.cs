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
        var provincePixels = this.MapManager.GetProvincesPixels();
        var terrainColors = new HashSet<Color32>() { ColorHelper.blue, ColorHelper.gray, Color.white };

        var provinceNames = new List<string>
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

        var provinceColors = new HashSet<Color32>();
        var borderPixels = ColorHelper.ExtractColorsWithPositions(provincePixels, this.MapManager.mapSize.x, this.MapManager.mapSize.y, (color) => color.a == 200);

        for (var x = 0; x < this.MapManager.mapSize.x; x++)
        {
            for (var y = 0; y < this.MapManager.mapSize.y; y++)
            {
                var pixelColor = provincePixels[y * this.MapManager.mapSize.x + x];
                if (pixelColor.a == 200)
                {
                    continue;
                }
                if (!terrainColors.Any(tc => tc.Equals(pixelColor)))
                {
                    if (!provinceColors.Contains(pixelColor))
                    {
                        provinceColors.Add(pixelColor);

                        // Choose a random index
                        var random = Random.Range(0, provinceNames.Count());

                        // Get and remove the random fantasy province name
                        string randomProvince = provinceNames[random];
                        provinceNames.RemoveAt(random);

                        var province = new Province(randomProvince, pixelColor);
                        province.BorderPixels = borderPixels.Where(bp => ColorHelper.AreColorsEqualIgnoringAlpha(bp.Color, pixelColor)).ToList();
                        this.Provinces.Add(province);

                    }
                }
            }
        }
    }

    public Province GetProvince()
    {
        var provinceColor = ColorHelper.GetColor(Camera.main);
        return Provinces.Where(p => p.Color.Equals(provinceColor)).FirstOrDefault();
    }
}