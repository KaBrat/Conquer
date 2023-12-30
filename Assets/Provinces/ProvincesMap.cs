using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Vector3 = UnityEngine.Vector3;

public class ProvincesMap : MonoBehaviour
{
    public HashSet<Province> Provinces = new HashSet<Province>();
    public Texture2D mapImage;
    public HashSet<Color32> provinceColors = new HashSet<Color32>();
    public SpriteRenderer spriteRenderer;

    public IProvinceDisplayer ProvinceDisplayer;

    void Start()
    {
        mainCamera = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();
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

        var terrainColors = new HashSet<Color32>() { Color.blue, Color.gray, Color.white };

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

    private Camera mainCamera;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var provinceColor = ColorHelper.GetColor(mainCamera, Input.mousePosition);
            var clickedProvince = Provinces.Where(p => p.Color == provinceColor).FirstOrDefault();
            //Debug.Log(clickedProvince.Name);

            this.ProvinceDisplayer.DisplayProvince(clickedProvince);
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