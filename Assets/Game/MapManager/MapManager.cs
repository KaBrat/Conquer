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
        this.TerrainMap.UpdateTextureIfPixelsChanged();
        this.ProvinceMap.UpdateTextureIfPixelsChanged();
    }

    private void Hover()
    {
        var mouseOverProvince = this.ProvincesManager.GetProvince();

        if (mouseOverProvince == null)
        {
            // currently not hovering over a province

            if (this.ProvincesManager.hoveredProvince == null)
                // also not hovering over a province before
                return;

            if (this.ProvincesManager.hoveredProvince != ProvincesManager.selectedProvince)
            {
                // the province we hovered over before was not the selected province
                this.ProvincesManager.hoveredProvince.UnHighlightTerrain(this.TerrainMap, this.ProvinceMap);
            }

            // set the currently hovered province to null, regardless if previously hovered is selected or not
            this.ProvincesManager.hoveredProvince = null;
            return;
        }

        // we are hovering over a province

        if (this.ProvincesManager.hoveredProvince != null)
        {
            // we were hovering over a province before
            if (this.ProvincesManager.hoveredProvince != ProvincesManager.selectedProvince && mouseOverProvince != this.ProvincesManager.hoveredProvince)
                // the province we were hovering over was not the selected province and we are not currently hovering over the previously hovered province
                this.ProvincesManager.hoveredProvince.UnHighlightTerrain(this.TerrainMap, this.ProvinceMap);
        }

        // we were or were not hovering over a province before
        if (!mouseOverProvince.highLighted)
            mouseOverProvince.HighlightTerrain(this.TerrainMap, this.ProvinceMap);
        this.ProvincesManager.hoveredProvince = mouseOverProvince;
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

        this.ProvincesManager.selectedProvince?.Deselect(this.TerrainMap, this.ProvinceMap);

        clickedProvince.Select(this.TerrainMap, this.ProvinceMap);
        this.ProvincesManager.selectedProvince = clickedProvince;
    }
}