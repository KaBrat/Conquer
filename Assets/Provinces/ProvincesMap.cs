using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Vector3 = UnityEngine.Vector3;

public class ProvincesMap : MonoBehaviour
{
    public IProvinceDisplayer ProvinceDisplayer;
    
    public HashSet<Province> Provinces = new HashSet<Province>();
    
    private Texture2D mapImage;

    public void ShowTerrain()
    {
        var sprite = ImageHelper.LoadImageFromDisk(this.mapImage.width, this.mapImage.height, Application.dataPath + "/GeneratedMaps/Terrain.png");
        this.GetComponent<SpriteRenderer>().sprite = sprite;
    }

    public void ShowProvinces()
    {
        var sprite = ImageHelper.LoadImageFromDisk(this.mapImage.width, this.mapImage.height, Application.dataPath + "/GeneratedMaps/States.png");
        this.GetComponent<SpriteRenderer>().sprite = sprite;
    }

    void Start()
    {
        this.mapImage = GetComponent<SpriteRenderer>().sprite.texture;
        ProvinceDisplayer = GameObject.FindObjectsOfType<InGameUI>().FirstOrDefault();
        InitProvinces();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var clickedProvince = GetProvince(Input.mousePosition);
            //Debug.Log(clickedProvince.Name);
            this.ProvinceDisplayer.DisplayProvince(clickedProvince);
        }
    }

    public Province GetProvince(Vector3 mousePosition)
    {
        var provinceColor = ColorHelper.GetColor(Camera.main, mousePosition);
        return Provinces.Where(p => p.Color == provinceColor).FirstOrDefault();
    }

    private void InitProvinces()
    {
        var pixels = this.mapImage.GetPixels();
        var terrainColors = new HashSet<Color32>() { ColorHelper.blue, Color.gray, Color.white };

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

        for (var x = 0; x < this.mapImage.width; x++)
        {
            for (var y = 0; y < this.mapImage.height; y++)
            {
                var pixelColor = pixels[y * this.mapImage.width + x];
                if (!terrainColors.Any(tc => tc == pixelColor))
                {
                    if (!provinceColors.Contains(pixelColor))
                    {
                        provinceColors.Add(pixelColor);

                        // Choose a random index
                        var random = Random.Range(0, provinceNames.Count());

                        // Get and remove the random fantasy province name
                        string randomProvince = provinceNames[random];
                        provinceNames.RemoveAt(random);

                        this.Provinces.Add(new Province(randomProvince, pixelColor));
                    }
                }
            }
        }
    }
}