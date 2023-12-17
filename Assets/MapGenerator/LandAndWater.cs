using UnityEngine;

public class LandAndWaterGenerator
{
    public Color[] GenerateLandAndWater(int mapWidth, int mapHeight, int outerXRange, int outerYRange, float noiseScale, float random, float threshold)
    {
        var offsetX = UnityEngine.Random.Range(-random, random);
        var offsetY = UnityEngine.Random.Range(-random, random);

        var pixels = new Color[mapWidth * mapHeight];

        for (var y = 0; y < mapHeight; y++)
        {
            for (var x = 0; x < mapWidth; x++)
            {
                var sampleX = (((float)x/(float)mapWidth) * noiseScale) + offsetX;
                var sampleY = (((float)y/(float)mapHeight) * noiseScale) + offsetY;

                var outerBoundarySmoothFactor = GetOuterBoundarySmoothFactor(mapWidth, mapHeight, outerXRange, outerYRange, y, x);
                var noiseValue = Mathf.PerlinNoise(sampleX, sampleY) * outerBoundarySmoothFactor;

                var mountainThreshold = 0.75f;

                if (noiseValue <= threshold)
                {
                    pixels[y * mapWidth + x] = Color.blue;
                }
                if (noiseValue >= threshold && noiseValue < mountainThreshold)
                {
                    pixels[y * mapWidth + x] = Color.green;
                }
                if (noiseValue >= mountainThreshold)
                {
                    pixels[y * mapWidth + x] = Color.gray;
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
        return 1f - relation;
    }
}