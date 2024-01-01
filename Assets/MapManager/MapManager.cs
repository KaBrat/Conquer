using UnityEngine;
using UnityEngine.EventSystems;

public class MapManager : MonoBehaviour
{
    public ProvincesMap ProvincesMap;

    public Sprite TerrainSprite;
    public Sprite ProvincesSprite;

    public Vector2Int mapSize;

    void Start()
    {
        this.TerrainSprite = ImageHelper.LoadImageFromDisk(1, 1, ImageHelper.TerrainMapPath);
        this.mapSize = new Vector2Int(TerrainSprite.texture.width, TerrainSprite.texture.height);
        this.ProvincesSprite = ImageHelper.LoadImageFromDisk(mapSize.x, mapSize.y, ImageHelper.ProvincesMapPath);
        this.ShowTerrain();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            var clickedProvince = ProvincesMap.GetProvince();
            this.ProvincesMap.ProvinceDisplayer.DisplayProvince(clickedProvince);

            if (clickedProvince == null)
            {
                if (this.ProvincesMap.selectedProvince == null)
                {
                    return;
                }

                this.ProvincesMap.selectedProvince.Deselect(this.TerrainSprite.texture, this.ProvincesSprite.texture);
                this.ProvincesMap.selectedProvince = null;
                return;
            }

            if (this.ProvincesMap.selectedProvince != null)
            {
                this.ProvincesMap.selectedProvince.Deselect(this.TerrainSprite.texture, this.ProvincesSprite.texture);
            }

            clickedProvince.Select(this.TerrainSprite.texture, this.ProvincesSprite.texture);
            this.ProvincesMap.selectedProvince = clickedProvince;
        }
    }

    public Color32[] GetTerrainPixels()
    {
        return this.TerrainSprite.texture.GetPixels32();
    }

    public void SetTerrainPixels(Color32[] pixels)
    {
        this.TerrainSprite.texture.SetPixels32(pixels);
        this.TerrainSprite.texture.Apply();
    }

    public Color32[] GetProvincesPixels()
    {
        return this.ProvincesSprite.texture.GetPixels32();
    }

    public void SetProvincesPixels(Color32[] pixels)
    {
        this.ProvincesSprite.texture.SetPixels32(pixels);
        this.ProvincesSprite.texture.Apply();
    }

    public void ShowTerrain()
    {
        this.GetComponent<SpriteRenderer>().sprite = this.TerrainSprite;
    }

    public void ShowProvinces()
    {
        this.GetComponent<SpriteRenderer>().sprite = this.ProvincesSprite;
    }
}