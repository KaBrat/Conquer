//using System.Collections.Generic;
//using UnityEngine;

//private Color[] GenerateStates(Color[] pixels)
//{
//    // Create a list of state colors
//    List<Color> stateColors = new List<Color>();

//    // Create a dictionary to store the centroids of each state color
//    Dictionary<Color, Vector2> stateCentroids = new Dictionary<Color, Vector2>();

//    // Create a spatial grid to group nearby pixels
//    int gridSize = 30; // Adjust the grid size based on your map and performance requirements
//    int gridWidth = Mathf.CeilToInt(mapWidth / (float)gridSize);
//    int gridHeight = Mathf.CeilToInt(mapHeight / (float)gridSize);
//    Dictionary<Vector2Int, Color> spatialGrid = new Dictionary<Vector2Int, Color>();

//    for (int y = 0; y < mapHeight; y++)
//    {
//        for (int x = 0; x < mapWidth; x++)
//        {
//            // Skip water pixels
//            if (pixels[y * mapWidth + x] != Color.green)
//                continue;

//            // Find the nearest state color using the spatial grid
//            Color nearestColor = ColorHelper.blue;
//            float nearestDistance = float.MaxValue;

//            Vector2Int gridPosition = new Vector2Int(x / gridSize, y / gridSize);
//            for (int dx = -1; dx <= 1; dx++)
//            {
//                for (int dy = -1; dy <= 1; dy++)
//                {
//                    Vector2Int neighborGridPosition = gridPosition + new Vector2Int(dx, dy);
//                    if (spatialGrid.TryGetValue(neighborGridPosition, out Color stateColor))
//                    {
//                        float distance = Vector2.Distance(new Vector2(x, y), stateCentroids[stateColor]);
//                        if (distance < nearestDistance)
//                        {
//                            nearestDistance = distance;
//                            nearestColor = stateColor;
//                        }
//                    }
//                }
//            }

//            // Assign the pixel to the nearest state color
//            pixels[y * mapWidth + x] = nearestColor;

//            // If the nearest color was not found within a certain range, create a new state
//            var randomValue = UnityEngine.Random.Range(60, 150);
//            if (nearestDistance > randomValue)
//            {
//                Color randomColor = RandomColor();
//                stateColors.Add(randomColor);
//                stateCentroids[randomColor] = new Vector2(x, y);
//                spatialGrid[gridPosition] = randomColor;
//            }
//        }
//    }

//    return pixels;
//}



//private Vector2 FindStateCentroid(Color stateColor, Color[] pixels)
//{
//    List<Vector2> points = new List<Vector2>();
//    for (int y = 0; y < mapHeight; y++)
//    {
//        for (int x = 0; x < mapWidth; x++)
//        {
//            if (pixels[y * mapWidth + x] == stateColor)
//                points.Add(new Vector2(x, y));
//        }
//    }

//    if (points.Count == 0)
//        return Vector2.zero;

//    Vector2 centroid = Vector2.zero;
//    foreach (Vector2 point in points)
//    {
//        centroid += point;
//    }

//    centroid /= points.Count;
//    return centroid;
//}

//private Color RandomColor()
//{
//    return new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
//}