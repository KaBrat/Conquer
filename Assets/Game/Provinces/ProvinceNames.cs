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
    "Twilight Sanctuary",
    "Dreadfen",
    "Ebonridge",
    "Icicle Isle",
    "Lunar Hollow",
    "Rustic Haven",
    "Molten Reach",
    "Amethyst Peaks",
    "Sable Sanctum",
    "Sunfire Hold",
    "Emerald Enclave",
    "Shiverstone",
    "Dragon's Gate",
    "Ironholme",
    "Frostfall",
    "Shadow's Embrace",
    "Crimson Hollow",
    "Astral Expanse",
    "Starfall",
    "Fogwatch",
    "Tideholm",
    "Ebonwatch",
    "Hallowed Grounds",
    "Whispering Woods",
    "Sapphire Shore",
    "Venomspire",
    "Thunderpeak",
    "Silver Strand",
    "Obsidian Hollow",
    "Twilight Vale",
    "Mystic Marsh",
    "Copper Haven",
    "Rosewood",
    "Stoneheart",
    "Ironhold",
    "Cerulean Haven",
    "Raven's Nest",
    "Silent Isle",
    "Abyssal Citadel",
    "Golden Glade",
    "Stormhold",
    "Veridian Sanctuary",
    "Dusktide",
    "Fiery Expanse",
    "Elderwood",
    "Frostbite Bluffs",
    "Ironpeak",
    "Shadow's Veil",
    "Celestial Haven",
    "Thornspire",
    "Glimmering Isles",
    "Baneholme",
    "Whispering Peaks",
    "Lunar Citadel",
    "Rustfall",
    "Sable Stronghold",
    "Crimson Keep",
    "Starlight Grove"
};

    }
}