using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Vector3 = UnityEngine.Vector3;
using UnityEngine.U2D;

public class ProvincesMap : MonoBehaviour
{
    public IProvinceDisplayer ProvinceDisplayer;

    public HashSet<Province> Provinces = new HashSet<Province>();

    private Texture2D mapImage;

    public Province selectedProvince;

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
            var texture = GetComponent<SpriteRenderer>().sprite.texture;
            var pixels = texture.GetPixels32();
            if (clickedProvince == null)
            {
                if (this.selectedProvince == null)
                {
                    return;
                }

                pixels = this.selectedProvince.Deselect(pixels, new Vector2(texture.width, texture.height));
                texture.SetPixels32(pixels);
                texture.Apply();
                this.selectedProvince = null;
                return;
            }

            if (this.selectedProvince != null)
            {
                pixels = this.selectedProvince.Deselect(pixels, new Vector2(texture.width, texture.height));
                texture.SetPixels32(pixels);
                texture.Apply();
            }

            pixels = clickedProvince.Select(pixels, new Vector2(texture.width, texture.height));
            texture.SetPixels32(pixels);
            texture.Apply();
            this.selectedProvince = clickedProvince;
            this.ProvinceDisplayer.DisplayProvince(clickedProvince);
        }
    }

    private List<MapPixel> FindProvinceBorders(Color32[] provinceMap, Vector2 mapDimension, Province province)
    {
        var borderPixel = new List<MapPixel>();

        for (var x = 0; x < mapDimension.x; x++)
        {
            for (var y = 0; y < mapDimension.y; y++)
            {
                var pixelColor = provinceMap[y * (int)mapDimension.x + x];
                if (!pixelColor.Equals(province.Color))
                {
                    continue;
                }

                if (ColorHelper.ColorListContainsColor(ColorHelper.ColorsUsedInTerrain, pixelColor))
                {
                    continue;
                }

                var neighbours = GetNeighbours(y * (int)mapDimension.x + x, (int)mapDimension.x, (int)mapDimension.y);
                foreach (var neighbour in neighbours)
                {
                    var neighbourColor = provinceMap[neighbour];
                    if (neighbourColor.Equals(ColorHelper.blue) || !pixelColor.Equals(neighbourColor) && (!ColorHelper.ColorListContainsColor(ColorHelper.ColorsUsedInTerrain, neighbourColor)))
                    {
                        borderPixel.Add(new MapPixel()
                        {
                            Color = Color.black,
                            Position = new Vector2(x, y)
                        });
                    }
                }
            }
        }
        return borderPixel;
    }

    public static List<int> GetNeighbours(int pixelIndex, int mapWidth, int mapHeight)
    {
        List<int> neighbours = new List<int>();

        int x = pixelIndex % mapWidth;
        int y = pixelIndex / mapWidth;

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                int neighborX = x + i;
                int neighborY = y + j;

                // Check if the neighbor is within bounds
                if (neighborX >= 0 && neighborX < mapWidth && neighborY >= 0 && neighborY < mapHeight)
                {
                    int neighborIndex = neighborY * mapWidth + neighborX;
                    neighbours.Add(neighborIndex);
                }
            }
        }

        return neighbours;
    }

    public Province GetProvince(Vector3 mousePosition)
    {
        var provinceColor = ColorHelper.GetColor(Camera.main, mousePosition);
        return Provinces.Where(p => p.Color.Equals(provinceColor)).FirstOrDefault();
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

        foreach (var province in this.Provinces)
        {
            province.BorderPixels = FindProvinceBorders(this.mapImage.GetPixels32(), new Vector2(this.mapImage.width, this.mapImage.height), province);
        }

    }
}