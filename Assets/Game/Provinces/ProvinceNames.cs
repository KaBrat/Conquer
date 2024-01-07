using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ProvinceNames
{
    public static string GetRandomProvinceNameAndRemove(List<string> provinceNames)
    {
        // Choose a random index
        var random = Random.Range(0, provinceNames.Count());

        // Get and remove the random fantasy province name
        var randomProvince = provinceNames[random];
        provinceNames.RemoveAt(random);

        return randomProvince;
    }
    public static List<string> GetRandomProvinceNames()
    {
        return new List<string>
{
    "Eldoria",
    "Frostholme",
    "Dreadmoor",
    "Silverpeak",
    "Raven's Hollow",
    "Stormwatch",
    "Ironhaven",
    "Celestial Reach",
    "Shadowfen",
    "Dragon's Roost",
    "Whisperwind",
    "Sylvan Glade",
    "Emberfall",
    "Mystic Vale",
    "Crystalline Shores",
    "Scarlet Citadel",
    "Oakenheart",
    "Ebonhold",
    "Glimmerstone",
    "Crimson Peak",
    "Aetheria",
    "Lunar Haven",
    "Forgelight",
    "Obsidian Dominion",
    "Thornvale",
    "Windsong",
    "Blazestone",
    "Ivory Tower",
    "Duskwood",
    "Azure Isle",
    "Radiant Fields",
    "Brimstone Wastes",
    "Verdant Haven",
    "Frostfire Keep",
    "Silvershade",
    "Ironwood Stronghold",
    "Abyssal Depths",
    "Golden Plains",
    "Stormspire",
    "Rusthaven",
    "Whirlwind Peaks",
    "Moonlit Marsh",
    "Cerulean Citadel",
    "Enigma Expanse",
    "Blacksand Atoll",
    "Thundertop",
    "Copperfold",
    "Twilight Sanctuary"
};
    }
}