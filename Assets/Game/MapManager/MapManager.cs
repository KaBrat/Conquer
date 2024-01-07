using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapManager : MonoBehaviour, IPointerClickHandler
{
    public ProvincesManager ProvincesManager;

    public Map TerrainMap;
    public Map ProvinceMap;
    public IProvinceDisplayer ProvinceDisplayer;
    public Vector2Int mapSize;

    void Start()
    {
        this.ProvincesManager = new ProvincesManager(this);
        this.ProvinceDisplayer = GameObject.FindObjectsOfType<InGameUI>().FirstOrDefault();

        var TerrainSprite = ImageHelper.LoadImageFromDisk(1, 1, ImageHelper.TerrainMapPath);
        this.mapSize = new Vector2Int(TerrainSprite.texture.width, TerrainSprite.texture.height);
        this.TerrainMap = new Map(TerrainSprite);

        var ProvincesSprite = ImageHelper.LoadImageFromDisk(mapSize.x, mapSize.y, ImageHelper.ProvincesMapPath);
        this.ProvinceMap = new Map(ProvincesSprite);

        this.GetComponent<BoxCollider2D>().size = ProvincesSprite.bounds.size;
        this.ShowTerrain();
        this.ProvincesManager.LoadAndSetProvincesByPixelColor();
    }

    void FixedUpdate()
    {
        Hover();
    }

    private void Hover()
    {
        var hoveredProvince = this.ProvincesManager.GetProvince();

        if (hoveredProvince == null)
        {
            if (this.ProvincesManager.hoveredProvince == null)
                return;

            if (this.ProvincesManager.hoveredProvince != ProvincesManager.selectedProvince)
                this.ProvincesManager.hoveredProvince.UnHighlightTerrain(this.TerrainMap, this.ProvinceMap);
            this.ProvincesManager.hoveredProvince = null;
            return;
        }

        if (this.ProvincesManager.hoveredProvince != null)
        {
            if (this.ProvincesManager.hoveredProvince != ProvincesManager.selectedProvince)
                this.ProvincesManager.hoveredProvince.UnHighlightTerrain(this.TerrainMap, this.ProvinceMap);
        }
        if (!hoveredProvince.highLighted)
            hoveredProvince.HighlightTerrain(this.TerrainMap, this.ProvinceMap);
        this.ProvincesManager.hoveredProvince = hoveredProvince;
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
        var clickedProvince = ProvincesManager.GetProvince();
        this.ProvinceDisplayer.DisplayProvince(clickedProvince);

        if (clickedProvince == null)
        {
            if (this.ProvincesManager.selectedProvince == null)
                return;

            this.ProvincesManager.selectedProvince.Deselect(this.TerrainMap, this.ProvinceMap);
            this.ProvincesManager.selectedProvince = null;
            return;
        }

        if (this.ProvincesManager.selectedProvince != null)
            this.ProvincesManager.selectedProvince.Deselect(this.TerrainMap, this.ProvinceMap);

        clickedProvince.Select(this.TerrainMap, this.ProvinceMap);
        this.ProvincesManager.selectedProvince = clickedProvince;
    }
}