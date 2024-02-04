using FloodSpill;
using FloodSpill.Queues;
using FloodSpill.Utilities;
using System;
using System.IO;
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

    public static void FloodPaint(Color32[] image, int imageWidth, int imageHeight, Vector2Int startPosition, Color32[] replacedColors, Color32 targetColor, int size)
    {
        var _positionMarkMatrix = new int[imageWidth, imageHeight];

        var pixelsLeft = size;

        Predicate<int, int> positionQualifier = (x, y) =>
        {
            var positionColor = GetColor(image, imageWidth, x, y);
            return replacedColors.Any(c => c.Equals(positionColor));
        };

        Predicate<int, int> neighbourStopCondition = (x, y) =>
        {
            if (pixelsLeft <= 0)
            {
                Debug.Log($"{x}, {y} causing stop!");
                return true;
            }
            return false;
        };

        System.Action<int, int, int> neighbourProcessor = (x, y, mark) =>
        {
            SetColor(image, imageWidth, x, y, targetColor);
            pixelsLeft--;
        };

        Func<Position, Position, int> comparer = (pos1, pos2) =>
        {
            int randomValue = UnityEngine.Random.Range(1, 3); // 1-3
            if (randomValue <= 1)
            {
                int value1 = _positionMarkMatrix[pos1.X, pos1.Y];
                int value2 = _positionMarkMatrix[pos2.X, pos2.Y];

                return value1.CompareTo(value2);
            }
            if (randomValue <= 2)
            {
                var distance1 = CalculateDistance(pos1.X, pos1.Y, startPosition.x, startPosition.y);
                var distance2 = CalculateDistance(pos2.X, pos2.Y, startPosition.x, startPosition.y);

                return distance1.CompareTo(distance2); // Compare based on distance to start position
            }
            else
            {
                return 0;
            }
        };

        static double CalculateDistance(int x1, int y1, int x2, int y2)
        {
            return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }

        var floodSpiller = new FloodSpiller();
        var floodParameters = new FloodParameters((int)startPosition.x, (int)startPosition.y)
        {
            PositionsToVisitQueue = new PriorityPositionQueue(comparer),
            BoundsRestriction = new FloodBounds(imageWidth, imageHeight),
            NeighbourhoodType = NeighbourhoodType.Four,
            Qualifier = positionQualifier,
            NeighbourProcessor = neighbourProcessor,
            NeighbourStopCondition = neighbourStopCondition,
            ProcessStartAsFirstNeighbour = true,
        };
        floodSpiller.SpillFlood(floodParameters, _positionMarkMatrix);
        //string representation = MarkMatrixVisualiser.Visualise(_positionMarkMatrix);
        //var filePath = "C:\\Entwicklung\\Conquer\\markmatrix.txt";
        //File.WriteAllText(filePath, representation);
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
            (byte)UnityEngine.Random.Range(0, 256),  // Random red component (0 to 255)
            (byte)UnityEngine.Random.Range(0, 256),  // Random green component (0 to 255)
            (byte)UnityEngine.Random.Range(0, 256),  // Random blue component (0 to 255)
            255                           // Fully opaque alpha component (255)
        );
    }
}

