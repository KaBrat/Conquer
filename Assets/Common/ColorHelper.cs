using System.Collections.Generic;
using UnityEngine;

public static class ColorHelper
{
    public static bool ColorListContainsColor(List<Color> colorList, Color color)
    {
        // Check if the color is in the list (exact comparison)
        return colorList.Contains(color);
    }
}