using FloodSpill;
using FloodSpill.Queues;
using FloodSpill.Utilities;
using System;
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

    public static void PaintProvince(Color32[] image, int imageWidth, int imageHeight, Vector2Int startPosition, Color32[] replacedColors, Color32 targetColor, int provinceMaxPixels, out int pixelsPainted)
    {
        var _positionMarkMatrix = new int[imageWidth, imageHeight];

        var pixelsVisited = 0;

        Predicate<int, int> positionQualifier = (x, y) =>
        {
            var positionColor = ColorArrayHelper.GetColor(image, x, y, imageWidth);
            return ColorHelper.SelectableTerrainColors.Contains(positionColor);
        };

        Predicate<int, int> stopCondition = (x, y) =>
        {
            var distance = CalculateDistance(startPosition.x, startPosition.y, x, y);
            if (pixelsVisited >= provinceMaxPixels)
            {
                return true;
            }
            return false;
        };

        Func<Position, Position, int> comparer = (pos1, pos2) =>
        {
            int value1 = _positionMarkMatrix[pos1.X, pos1.Y];
            int value2 = _positionMarkMatrix[pos2.X, pos2.Y];

            return value1.CompareTo(value2);
        };

        Action<int, int> positionVisitor = (x, y) =>
        {
            SetColor(image, imageWidth, x, y, targetColor);
            pixelsVisited++;
        };

        var floodSpiller = new FloodSpiller();
        var floodParameters = new FloodParameters((int)startPosition.x, (int)startPosition.y)
        {
            SpreadingPositionStopCondition = stopCondition,
            PositionsToVisitQueue = new PriorityPositionQueue(comparer),
            SpreadingPositionVisitor = positionVisitor,
            BoundsRestriction = new FloodBounds(imageWidth, imageHeight),
            NeighbourhoodType = NeighbourhoodType.Four,
            Qualifier = positionQualifier,
        };
        floodSpiller.SpillFlood(floodParameters, _positionMarkMatrix);
        //string representation = MarkMatrixVisualiser.Visualise(_positionMarkMatrix);
        //var filePath = "C:\\Entwicklung\\Conquer\\markmatrix.txt";
        //File.WriteAllText(filePath, representation);

        pixelsPainted = pixelsVisited;
    }

    static double CalculateDistance(int x1, int y1, int x2, int y2)
    {
        return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
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

