using UnityEngine;
using UnityEngine.EventSystems;

public class MapManager : MonoBehaviour, IPointerClickHandler
{
    public ProvincesMap ProvincesMap;

    public Map TerrainMap;
    public Map ProvinceMap;
    public Vector2Int mapSize;
    //public Sprite TerrainSprite;
    //public Sprite ProvincesSprite;

    void Start()
    {
        var TerrainSprite = ImageHelper.LoadImageFromDisk(1, 1, ImageHelper.TerrainMapPath);
        this.mapSize = new Vector2Int(TerrainSprite.texture.width, TerrainSprite.texture.height);
        this.TerrainMap = new Map(TerrainSprite);

        var ProvincesSprite = ImageHelper.LoadImageFromDisk(mapSize.x, mapSize.y, ImageHelper.ProvincesMapPath);
        this.ProvinceMap = new Map(ProvincesSprite);

        this.GetComponent<BoxCollider2D>().size = ProvincesSprite.bounds.size;
        this.ShowTerrain();
        this.ProvincesMap.InitProvinces();
    }

    void FixedUpdate()
    {
        Hover();
    }

    private void Hover()
    {
        var hoveredProvince = this.ProvincesMap.GetProvince();

        if (hoveredProvince == null)
        {
            if (this.ProvincesMap.hoveredProvince == null)
                return;

            if (this.ProvincesMap.hoveredProvince != ProvincesMap.selectedProvince)
                this.ProvincesMap.hoveredProvince.UnHighlightTerrain(this.TerrainMap, this.ProvinceMap);
            this.ProvincesMap.hoveredProvince = null;
            return;
        }

        if (this.ProvincesMap.hoveredProvince != null)
        {
            if (this.ProvincesMap.hoveredProvince != ProvincesMap.selectedProvince)
                this.ProvincesMap.hoveredProvince.UnHighlightTerrain(this.TerrainMap, this.ProvinceMap);
        }
        if (!hoveredProvince.highLighted)
            hoveredProvince.HighlightTerrain(this.TerrainMap, this.ProvinceMap);
        this.ProvincesMap.hoveredProvince = hoveredProvince;
    }

    public void ShowTerrain()
    {
        this.GetComponent<SpriteRenderer>().sprite = this.TerrainMap.GetSprite();
    }

    public void ShowProvinces()
    {
        this.GetComponent<SpriteRenderer>().sprite = this.ProvinceMap.GetSprite();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        var clickedProvince = ProvincesMap.GetProvince();
        this.ProvincesMap.ProvinceDisplayer.DisplayProvince(clickedProvince);

        if (clickedProvince == null)
        {
            if (this.ProvincesMap.selectedProvince == null)
            {
                return;
            }

            this.ProvincesMap.selectedProvince.Deselect(this.TerrainMap, this.ProvinceMap);
            this.ProvincesMap.selectedProvince = null;
            return;
        }

        if (this.ProvincesMap.selectedProvince != null)
        {
            this.ProvincesMap.selectedProvince.Deselect(this.TerrainMap, this.ProvinceMap);
        }

        clickedProvince.Select(this.TerrainMap, this.ProvinceMap);
        this.ProvincesMap.selectedProvince = clickedProvince;
    }
}