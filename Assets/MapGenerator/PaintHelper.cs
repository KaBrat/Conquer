using FloodSpill;
using FloodSpill.Utilities;
using System.Linq;
using UnityEngine;

public static class PaintHelper
{
    public static void BucketFillImage(Color32[] image, int imageWidth, int imageHeight, int floodStartX, int floodStartY, Color32 replacedColor, Color32 targetColor)
    {
        var floodSpiller = new FloodSpiller();
        var floodParameters = new FloodParameters(floodStartX, floodStartY)
        {
            BoundsRestriction = new FloodBounds(imageWidth, imageHeight),
            NeighbourhoodType = NeighbourhoodType.Four,
            Qualifier = (x, y) => GetColor(image, imageWidth, x, y).Equals(replacedColor),
            NeighbourProcessor = (x, y, mark) => SetColor(image, imageWidth, x, y, targetColor),
            ProcessStartAsFirstNeighbour = true
        };

        var _positionMarkMatrix = new int[imageWidth, imageHeight];
        floodSpiller.SpillFlood(floodParameters, _positionMarkMatrix);
    }

    public static void FloodPaint(Color32[] image, int imageWidth, int imageHeight, Vector2 startPosition, Color32[] replacedColors, Color32 targetColor, int size)
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

    private static void SetColor(Color32[] image, int imageWidth, int x, int y, Color32 targetColor)
    {
        image[y * imageWidth + x] = targetColor;
    }

    private static Color32 GetColor(Color32[] image, int imageWidth, int x, int y)
    {
        return image[y * imageWidth + x];
    }

    public static Color32 GenerateRandomColor()
    {
        return new Color32(
            (byte)Random.Range(0, 256),  // Random red component (0 to 255)
            (byte)Random.Range(0, 256),  // Random green component (0 to 255)
            (byte)Random.Range(0, 256),  // Random blue component (0 to 255)
            255                           // Fully opaque alpha component (255)
        );
    }
}

