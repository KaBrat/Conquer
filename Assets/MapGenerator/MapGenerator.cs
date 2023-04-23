using System;
using System.IO;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{

    [Header("Map Generation Settings")]
    [SerializeField, Range(0.1f, 1f)] private float noiseScale = 0.4f;
    [SerializeField, Range(0f, 1f)] private float threshold = 0.3f;
    [SerializeField, Range(0, 100)] private int erosionIterations = 0;
    [SerializeField, Range(0f, 500f)] private float random = 20f;
    [SerializeField, Range(1, 3)] private int smoothing = 2;
    [SerializeField, Range(100, 3000)] private int mapWidth = 500;
    [SerializeField, Range(100, 3000)] private int mapHeight = 300;
    [SerializeField, Range(0, 50)] private int outerBoundaryXSize = 10;
    [SerializeField, Range(0, 50)] private int outerBoundaryYSize = 10;

    public void GenerateMap()
    {
        Color[] pixels = GeneratePixels(mapWidth, mapHeight, outerBoundaryXSize, outerBoundaryYSize, noiseScale, random, threshold, erosionIterations, smoothing);
        GenerateTexture(pixels);
    }

    private void GenerateTexture(Color[] pixels)
    {
        var landTexture = new Texture2D(mapWidth, mapHeight);
        landTexture.SetPixels(pixels);
        landTexture.Apply();

        var sprite = Sprite.Create(landTexture, new Rect(0, 0, landTexture.width, landTexture.height), Vector2.one * 0.5f);
        GetComponent<SpriteRenderer>().sprite = sprite;

        byte[] pngBytes = landTexture.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/GeneratedMaps/LandMap.png", pngBytes);
    }

    private Color[] GeneratePixels(int mapWidth, int mapHeight, int outerXRange, int outerYRange, float noiseScale, float random, float threshold, int erosionIterations, int smoothing)
    {
        var offsetX = UnityEngine.Random.Range(-random, random);
        var offsetY = UnityEngine.Random.Range(-random, random);

        Color[] pixels = new Color[mapWidth * mapHeight];

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                float outerBoundarySmoothFactor = GetOuterBoundarySmoothFactor(mapWidth, mapHeight, outerXRange, outerYRange, y, x);

                float sampleX = (float)x / mapWidth * noiseScale;
                float sampleY = (float)y / mapHeight * noiseScale;
                float noiseValue = Mathf.PerlinNoise(sampleX + offsetX, sampleY + offsetY);
                noiseValue *= outerBoundarySmoothFactor;
                pixels[y * mapWidth + x] = noiseValue >= threshold ? Color.green : Color.blue;
            }
        }

        for (int i = 0; i < erosionIterations; i++)
        {
            pixels = Erode(pixels, mapWidth, mapHeight);
        }

        return pixels;
    }

    private float GetOuterBoundarySmoothFactor(int mapWidth, int mapHeight, int outerXRange, int outerYRange, int y, int x)
    {
        bool isOuterXPixel = IsOnOuterBoundaries(x, outerXRange, mapWidth);
        bool isOuterYPixel = IsOnOuterBoundaries(y, outerYRange, mapHeight);

        if (!isOuterXPixel && !isOuterYPixel)
        {
            return 1f;
        }

        int xDistanceToMaxInner = CalculateDistanceToMaxInnerBoundary(x, outerXRange, mapWidth);
        int yDistanceToMaxInner = CalculateDistanceToMaxInnerBoundary(y, outerYRange, mapHeight);

        if (isOuterXPixel && isOuterYPixel)
        {
            return xDistanceToMaxInner >= yDistanceToMaxInner ?
                CalculateOuterBoundarySmoothFactor(xDistanceToMaxInner, outerXRange, mapWidth) :
                CalculateOuterBoundarySmoothFactor(yDistanceToMaxInner, outerYRange, mapHeight);
        }

        return isOuterXPixel ?
            CalculateOuterBoundarySmoothFactor(xDistanceToMaxInner, outerXRange, mapWidth) :
            CalculateOuterBoundarySmoothFactor(yDistanceToMaxInner, outerYRange, mapHeight);
    }

    private bool IsOnOuterBoundaries(int pixelValue, int outerRange, int max)
    {
        return outerRange != 0 && (pixelValue <= (1f / outerRange) * max || pixelValue >= max - (1f / outerRange) * max);
    }

    private int CalculateDistanceToMaxInnerBoundary(int pixelValue, int outerRange, int max)
    {
        int maxInner = outerRange == 0 ? max / 2 : pixelValue < max / 2 ? (int)(1f / outerRange * max) : (int)(max - 1f / outerRange * max);
        return Mathf.Abs(pixelValue - maxInner);
    }

    private float CalculateOuterBoundarySmoothFactor(int distanceToMaxInner, int outerRange, int max)
    {
        var maxDistance = (1f / outerRange) * max;
        var relation = distanceToMaxInner / maxDistance;
        return 1f - relation;
    }


    Color[] Erode(Color[] pixels, int mapWidth, int mapHeight)
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



