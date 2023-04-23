using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{

    public float noiseScale = 0.4f;
    public float threshold = 0.3f;
    public int erosionIterations = 0;
    public float random = 0f;
    public int smoothing = 2;
    public int mapWidth = 500;
    public int mapHeight = 300;
    public int outerXRange = 50;
    public int outerYRange = 0;

    public void GenerateMap()
    {
        Color[] pixels = CalculatePixels(mapWidth, mapHeight, outerXRange, outerYRange, noiseScale, random);

        var landTexture = new Texture2D(mapWidth, mapHeight);
        landTexture.SetPixels(pixels);
        landTexture.Apply();

        byte[] pngBytes = landTexture.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/GeneratedMaps/LandMap.png", pngBytes);

        var sr = this.GetComponent<SpriteRenderer>();
        Sprite sprite = Sprite.Create(landTexture, new Rect(0, 0, landTexture.width, landTexture.height), Vector2.one * 0.5f);        //this.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(Application.dataPath + "/GeneratedMaps/LandMap.png");
        sr.sprite = sprite;
    }

    private Color[] CalculatePixels(int mapWidth, int mapHeight, int outerXRange, int outerYRange, float noiseScale, float random)
    {
        var offsetX = UnityEngine.Random.Range(-random, random);
        var offsetY = UnityEngine.Random.Range(-random, random);

        Color[] pixels = new Color[mapWidth * mapHeight];
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {

                float sampleX = (float)x / mapWidth * noiseScale;
                float sampleY = (float)y / mapHeight * noiseScale;
                float noiseValue = Mathf.PerlinNoise(sampleX + offsetX, sampleY + offsetY);

                var isOuterXPixel = isOuterPixel(x, outerXRange, mapWidth);
                var isOuterYPixel = isOuterPixel(y, outerYRange, mapHeight);

                float outerBoundarySmoothFactor = 1f;

                switch (isOuterXPixel)
                {
                    case true when isOuterYPixel:
                        var xHasBiggerDistance = CalculateDistanceToMaxInnerBoundary(x, outerXRange, mapWidth) >= CalculateDistanceToMaxInnerBoundary(y, outerYRange, mapHeight);
                        if (xHasBiggerDistance)
                            outerBoundarySmoothFactor = this.CalculateOuterBoundarySmoothFactor(x, outerXRange, mapWidth);
                        else
                        {
                            outerBoundarySmoothFactor = this.CalculateOuterBoundarySmoothFactor(y, outerYRange, mapHeight);
                        }
                        break;
                    case true when !isOuterYPixel:
                        outerBoundarySmoothFactor = this.CalculateOuterBoundarySmoothFactor(x, outerXRange, mapWidth);
                        break;
                    case false when isOuterYPixel:
                        outerBoundarySmoothFactor = this.CalculateOuterBoundarySmoothFactor(y, outerYRange, mapHeight);
                        break;
                }
                noiseValue = noiseValue * outerBoundarySmoothFactor;
                pixels[y * mapWidth + x] = noiseValue >= threshold ? Color.green : Color.blue;
            }
        }

        // Erode the land texture
        for (int i = 0; i < erosionIterations; i++)
        {
            pixels = Erode(pixels);
        }

        return pixels;
    }

    private bool isOuterPixel(int pixelValue, int outerRange, int max)
    {
        var isOuterPixel = false;
        if (outerRange != 0)
        {
            isOuterPixel = pixelValue <= (1f / outerRange) * max || pixelValue >= max - (1f / outerRange) * max;
        }
        return isOuterPixel;
    }

    private int CalculateDistanceToMaxInnerBoundary(int pixelValue, int outerRange, int max)
    {
        var isPixelValueLowerThanMaxHalf = (pixelValue < max / 2);
        int maxInner;
        if (isPixelValueLowerThanMaxHalf)
        {
            maxInner = (int)(1f / outerRange * max);
        }
        else
        {
            maxInner = (int)(max - 1f / outerRange * max);
        }
        return Mathf.Abs(pixelValue - maxInner);
    }

    private float CalculateOuterBoundarySmoothFactor(int pixelValue, int outerRange, int max)
    {
        var distance = CalculateDistanceToMaxInnerBoundary(pixelValue, outerRange, max);
        var maxDistance = (1f / outerRange) * max;
        var relation = distance / maxDistance;
        return 1f - relation;
    }


    Color[] Erode(Color[] pixels)
    {
        Color[] erodedPixels = new Color[pixels.Length];
        for (int y = 1; y < mapHeight - 1; y++)
        {
            for (int x = 1; x < mapWidth - 1; x++)
            {
                // Get the current pixel and its neighbors
                Color currentPixel = pixels[y * mapWidth + x];
                Color leftPixel = pixels[y * mapWidth + (x - 1)];
                Color rightPixel = pixels[y * mapWidth + (x + 1)];
                Color topPixel = pixels[(y - 1) * mapWidth + x];
                Color bottomPixel = pixels[(y + 1) * mapWidth + x];

                // Count the number of water pixels surrounding the current pixel
                int waterPixelCount = 0;
                if (leftPixel == Color.blue) waterPixelCount++;
                if (rightPixel == Color.blue) waterPixelCount++;
                if (topPixel == Color.blue) waterPixelCount++;
                if (bottomPixel == Color.blue) waterPixelCount++;

                if (waterPixelCount <= 1)
                    erodedPixels[y * mapWidth + x] = Color.green;

                // Check if the current pixel is land surrounded by water, with a majority of water pixels
                if (currentPixel == Color.green && waterPixelCount >= smoothing)
                {
                    erodedPixels[y * mapWidth + x] = Color.blue;
                }
                else
                {
                    erodedPixels[y * mapWidth + x] = currentPixel;
                }
            }
        }

        return erodedPixels;
    }
}



