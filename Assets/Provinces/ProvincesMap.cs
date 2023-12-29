using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.EventSystems;

public class ProvincesMap : MonoBehaviour
{
    public List<Province> Provinces;
    public Texture2D mapImage;
    public HashSet<Color> provinceColors = new HashSet<Color>();

    public IProvinceDisplayer ProvinceDisplayer;

    void Start()
    {
        mainCamera = Camera.main;
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

        Provinces = new List<Province>();
        var terrainColors = new HashSet<Color>() { Color.blue, Color.gray, Color.white};

        for (var x = 0; x < this.mapImage.width; x++)
        {
            for (var y = 0; y < this.mapImage.height; y++)
            {
                var pixelColor = pixels[y * this.mapImage.width + x];
                if (!terrainColors.Any(tc => tc.Equals(pixelColor)))
                {
                    provinceColors.Add(pixelColor);
                }
            }
        }

        //provinceColors = new HashSet<Color>(pixels);
        //provinceColors.Remove(Color.black);
        //provinceColors.Remove(Color.white);

        //foreach (var province in Provinces)
        //{
        //    var match = provinceColors.First(pC => String.Equals(ColorUtility.ToHtmlStringRGB(pC), ColorUtility.ToHtmlStringRGB(province.Color), StringComparison.OrdinalIgnoreCase));
        //    province.Color = match;
        //}
    }

    private Camera mainCamera;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ColorHelper.GetColor(mainCamera, Input.mousePosition);

            //var pixel = GetRGBA();
            //Debug.Log(pixel);
            //var clickedProvince = GetProvince(Input.mousePosition);
            //ProvinceDisplayer.DisplayProvince(clickedProvince);
        }

        MoveCamera();
    }

    public float cameraMoveSpeed = 5f;

    void MoveCamera()
    {
        // Move the camera based on key input
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(horizontalInput, verticalInput, 0f);
        Vector3 moveAmount = moveDirection * cameraMoveSpeed * Time.deltaTime;

        mainCamera.transform.Translate(moveAmount);
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