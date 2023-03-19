using System.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bitmapLoader : MonoBehaviour
{
    public Texture2D regionsMap;
    private static int mapDimension = 80;
    private Vector3 size = new Vector3(mapDimension, mapDimension, 0);

    // Update is called once per frame
    void Update()
    {
        // if (Input.anyKeyDown)
        // {
        var screenPosition = Input.mousePosition;
        screenPosition.z = Camera.main.nearClipPlane + 1;
        var worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        var mapPosition = new Vector3(worldPosition.x * 100 + 0.5f * mapDimension, worldPosition.y * 100 + 0.5f * mapDimension, 0);

        int x = Mathf.FloorToInt(mapPosition.x / size.x * regionsMap.width);
        int y = Mathf.FloorToInt(mapPosition.y / size.y * regionsMap.height);
        var pixel = regionsMap.GetPixel(x, y);
        Debug.Log("RGBA:" + pixel);
        // }
    }
}