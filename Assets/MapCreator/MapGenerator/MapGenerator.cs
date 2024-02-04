using System;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Header("Map Generation Settings")]
    // Noisescale is like a "zoom" on the perlin noise
    // high => far away, low => close
    [SerializeField, Range(0.1f, 50f)] private float noiseScale = 12f;
    [SerializeField, Range(0f, 1f)] private float deepSeaThreshold = 0.05f;
    [SerializeField, Range(0f, 1f)] private float seaThreshold = 0.06f;
    [SerializeField, Range(0f, 1f)] private float shallowSeaThreshold = 0.08f;
    [SerializeField, Range(0f, 1f)] private float beachThreshold = 0.1f;
    [SerializeField, Range(0f, 1f)] private float grassThreshold = 0.8f;
    [SerializeField, Range(0f, 1f)] private float mountainThreshold = 0.9f;
    [SerializeField, Range(0f, 500f)] private float random = 50f;
    [SerializeField, Range(100, 3000)] private int mapWidth = 500;
    [SerializeField, Range(100, 3000)] private int mapHeight = 300;
    [SerializeField, Range(0, 50)] private int outerBoundaryXSize = 10;
    [SerializeField, Range(0, 50)] private int outerBoundaryYSize = 10;
    [SerializeField, Range(0, 50000)] private int ProvincesMaxSize = 3000;
    [SerializeField, Range(0, 300)] private int AmountOfRivers = 2;

    public void GenerateMap()
    {
        var (Terrain, Provinces) = GeneratePixels();

        ImageHelper.SaveTerrainPixels(Terrain, new Vector2Int(this.mapWidth, this.mapHeight));
        ImageHelper.SaveProvincesPixels(Provinces, new Vector2Int(this.mapWidth, this.mapHeight));

        ShowTerrain();
    }

    public void ShowTerrain()
    {
        var sprite = ImageHelper.LoadImageFromDisk(this.mapWidth, this.mapHeight, Application.dataPath + "/GeneratedMaps/Terrain.png");
        GetComponent<SpriteRenderer>().sprite = sprite;
    }

    public void ShowProvinces()
    {
        var sprite = ImageHelper.LoadImageFromDisk(this.mapWidth, this.mapHeight, Application.dataPath + "/GeneratedMaps/States.png");
        GetComponent<SpriteRenderer>().sprite = sprite;
    }

    private (Color32[] Terrain, Color32[] States) GeneratePixels()
    {
        var generator = new TerrainGenerator(this.mapWidth, this.mapHeight, this.noiseScale, this.random, this.outerBoundaryXSize, this.outerBoundaryYSize);
        var noiseMap = generator.GenerateNoiseMap();
        var terrain = generator.GenerateTerrain(noiseMap, this.deepSeaThreshold, this.seaThreshold, this.shallowSeaThreshold, this.beachThreshold, this.grassThreshold, this.mountainThreshold);

        //var terrainWithRivers = 
        new RiverGenerator(noiseMap, terrain, new Vector2Int(this.mapWidth, this.mapHeight)).DrawRivers(AmountOfRivers);

        var generatedProvinces = new ProvincesGenerator().GenerateProvinces(terrain, new Vector2Int(this.mapWidth, this.mapHeight), this.ProvincesMaxSize);

        new BorderGenerator(this.mapWidth, this.mapHeight).AddStateBordersToTerrain(terrain, generatedProvinces.provinces, generatedProvinces.provinceColors);

        return (terrain, generatedProvinces.provinces);
    }
}