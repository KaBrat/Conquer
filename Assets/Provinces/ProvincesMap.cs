using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Vector3 = UnityEngine.Vector3;
using UnityEngine.U2D;

public class ProvincesMap : MonoBehaviour
{
    public IProvinceDisplayer ProvinceDisplayer;

    public HashSet<Province> Provinces = new HashSet<Province>();

    public MapManager MapManager;

    public Province selectedProvince;

    void Start()
    {
        ProvinceDisplayer = GameObject.FindObjectsOfType<InGameUI>().FirstOrDefault();
        InitProvinces();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //var texture = GetComponent<SpriteRenderer>().sprite.texture;
            //if (clickedProvince == null)
            //{
            //    if (this.selectedProvince == null)
            //    {
            //        return;
            //    }

            //    this.selectedProvince.Deselect(new Vector2(texture.width, texture.height));
            //    this.selectedProvince = null;
            //    return;
            //}

            //if (this.selectedProvince != null)
            //{
            //    this.selectedProvince.Deselect(new Vector2(texture.width, texture.height));
            //}

            //clickedProvince.Select(new Vector2(texture.width, texture.height));
            //this.selectedProvince = clickedProvince;
            //this.ProvinceDisplayer.DisplayProvince(clickedProvince);
        }
    }

    public Province GetProvince()
    {
        var provinceColor = ColorHelper.GetColor(Camera.main);
        return Provinces.Where(p => p.Color.Equals(provinceColor)).FirstOrDefault();
    }

    private void InitProvinces()
    {
        var pixels = this.MapManager.GetProvincesPixels();
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
        var borderPixels = ColorHelper.ExtractColorsWithPositions(pixels, this.MapManager.mapSize.width, this.MapManager.mapSize.height, (color) => color.a == 200);

        for (var x = 0; x < this.MapManager.mapSize.width; x++)
        {
            for (var y = 0; y < this.MapManager.mapSize.height; y++)
            {
                var pixelColor = pixels[y * this.MapManager.mapSize.width + x];
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

        //var terrainMap = ImageHelper.LoadTerrainPixels();
        //foreach (var province in this.Provinces)
        //{
        //    province.BorderPixels = FindProvinceBorders(this.mapImage.GetPixels32(), new Vector2(this.mapImage.width, this.mapImage.height), province);
        //    foreach (var borderPixel in province.BorderPixels)
        //    {
        //        terrainMap[(int)borderPixel.Position.y * this.mapImage.width + (int)borderPixel.Position.x] = new Color32(0, 0, 0,255);
        //    }
        //}
        //ImageHelper.SaveTerrainPixels(terrainMap);

    }
}