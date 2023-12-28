using FloodSpill;
using FloodSpill.Utilities;
using System.Linq;
using UnityEngine;

public static class PaintHelper
{
    public static void BucketFillImage(Color[] image, int imageWidth, int imageHeight, int floodStartX, int floodStartY, Color replacedColor, Color targetColor)
    {
        var floodSpiller = new FloodSpiller();
        var floodParameters = new FloodParameters(floodStartX, floodStartY)
        {
            BoundsRestriction = new FloodBounds(imageWidth, imageHeight),
            NeighbourhoodType = NeighbourhoodType.Four,
            Qualifier = (x, y) => GetColor(image, imageWidth, x, y) == replacedColor,
            NeighbourProcessor = (x, y, mark) => SetColor(image, imageWidth, x, y, targetColor),
            ProcessStartAsFirstNeighbour = true
        };

        var _positionMarkMatrix = new int[imageWidth, imageHeight];
        floodSpiller.SpillFlood(floodParameters, _positionMarkMatrix);
    }

    public static void FloodPaint(Color[] image, int imageWidth, int imageHeight, Vector2 startPosition, Color[] replacedColors, Color targetColor, int size)
    {
        Predicate<int, int> positionQualifier = (x, y) =>
        {
            var positionColor = GetColor(image, imageWidth, x, y);
            return replacedColors.Any(c => c.Equals(positionColor));
        };

        Predicate<int, int> neighbourStopCondition = (x, y) =>
        {
            var currentPosition = new Vector2(x, y);
            var distance = Vector2.Distance(startPosition, currentPosition);
            if (distance >= size)
            {
                Debug.Log($"{x}, {y} causing stop!");
                return true;
            }
            return false;
        };

        var floodSpiller = new FloodSpiller();
        var floodParameters = new FloodParameters((int)startPosition.x, (int)startPosition.y)
        {
            BoundsRestriction = new FloodBounds(imageWidth, imageHeight),
            NeighbourhoodType = NeighbourhoodType.Four,
            //Qualifier = (x, y) => GetColor(image, imageWidth, x, y) == replacedColor,
            Qualifier = positionQualifier,
            NeighbourProcessor = (x, y, mark) => SetColor(image, imageWidth, x, y, targetColor),
            NeighbourStopCondition = neighbourStopCondition,
            ProcessStartAsFirstNeighbour = true,
        };

        var _positionMarkMatrix = new int[imageWidth, imageHeight];
        floodSpiller.SpillFlood(floodParameters, _positionMarkMatrix);
    }

    private static void SetColor(Color[] image, int imageWidth, int x, int y, Color targetColor)
    {
        image[y * imageWidth + x] = targetColor;
    }

    private static Color GetColor(Color[] image, int imageWidth, int x, int y)
    {
        return image[y * imageWidth + x];
    }

    public static Color GenerateRandomColor()
    {
        // Generate random values for each color component
        float red = Random.Range(0f, 1f);
        float green = Random.Range(0f, 1f);
        float blue = Random.Range(0f, 1f);
        float alpha = Random.Range(0f, 1f); // Optional: include alpha for transparency

        // Create and return the random color
        return new Color(red, green, blue, alpha);
    }
}

