using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator
{
    public int mapWidth, mapHeight;
    public float noiseScale;
    public float random;
    public int outerXRange, outerYRange;

    public TerrainGenerator(int mapWidth, int mapHeight, float noiseScale, float random, int outerXRange, int outerYRange)
    {
        this.mapWidth = mapWidth;
        this.mapHeight = mapHeight;
        this.noiseScale = noiseScale;
        this.random = random;
        this.outerXRange = outerXRange;
        this.outerYRange = outerYRange;
    }
    public float[,] GenerateNoiseMap()
    {
        var offsetX = UnityEngine.Random.Range(-this.random, this.random);
        var offsetY = UnityEngine.Random.Range(-this.random, this.random);

        var noiseMap = new float[this.mapWidth, this.mapHeight];
        for (var x = 0; x < mapWidth; x++)
        {
            for (var y = 0; y < mapHeight; y++)
            {
                var sampleX = (((float)x / (float)mapWidth) * this.noiseScale) + offsetX;
                var sampleY = (((float)y / (float)mapHeight) * this.noiseScale) + offsetY;

                var outerBoundarySmoothFactor = GetOuterBoundarySmoothFactor(this.mapWidth, this.mapHeight, this.outerXRange, this.outerYRange, y, x);
                var noiseValue = Mathf.PerlinNoise(sampleX, sampleY) * outerBoundarySmoothFactor;

                noiseMap[x, y] = noiseValue;
            }
        }
        return noiseMap;
    }

    public Color32[] GenerateTerrain(float[,] noiseMap, float deepSeaThreshold, float seaThreshold, float shallowSeaThreshold, float beachThreshold, float grassThreshold, float mountainThreshold)
    {
        var pixels = new Color32[mapWidth * mapHeight];

        for (var x = 0; x < mapWidth; x++)
        {
            for (var y = 0; y < mapHeight; y++)
            {
                ;
                var noiseValue = noiseMap[x, y];

                // deep sea
                if (noiseValue <= deepSeaThreshold)
                {
                    pixels[y * mapWidth + x] = ColorHelper.deepSeaBlue;
                }

                // sea
                if (noiseValue > deepSeaThreshold && noiseValue <= shallowSeaThreshold)
                {
                    pixels[y * mapWidth + x] = ColorHelper.seaBlue;
                }

                // shore
                if (noiseValue > seaThreshold && noiseValue <= beachThreshold)
                {
                    pixels[y * mapWidth + x] = ColorHelper.shallowSeaBlue;
                }

                // beach
                if (noiseValue > shallowSeaThreshold && noiseValue <= beachThreshold)
                {
                    pixels[y * mapWidth + x] = ColorHelper.sandYellow;
                }

                // land
                if (noiseValue > beachThreshold && noiseValue <= grassThreshold)
                {
                    pixels[y * mapWidth + x] = ColorHelper.grassGreen;
                }

                // mountain
                if (noiseValue > grassThreshold && noiseValue <= mountainThreshold)
                {
                    pixels[y * mapWidth + x] = ColorHelper.mountainGray;
                }

                // snow
                if (noiseValue > mountainThreshold)
                {
                    pixels[y * mapWidth + x] = Color.white;
                }
            }
        }
        return pixels;
    }

    private float GetOuterBoundarySmoothFactor(int mapWidth, int mapHeight, int outerXRange, int outerYRange, int y, int x)
    {
        bool isOuterXPixel = IsOnOuterBoundaries(x, outerXRange, mapWidth);
        bool isOuterYPixel = IsOnOuterBoundaries(y, outerYRange, mapHeight);

        if (!isOuterXPixel && !isOuterYPixel)
            return 1f;

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
        var smoothMore = 0.5f;
        return (1f - relation) * smoothMore;
    }
}