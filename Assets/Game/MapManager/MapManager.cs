using UnityEngine;
using UnityEngine.EventSystems;

public class MapManager : MonoBehaviour, IPointerClickHandler
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
                this.ProvincesMap.hoveredProvince.UnHighlightTerrain(this.TerrainSprite.texture, this.ProvincesSprite.texture);
            this.ProvincesMap.hoveredProvince = null;
            return;
        }

        if (this.ProvincesMap.hoveredProvince != null)
        {
            if (this.ProvincesMap.hoveredProvince != ProvincesMap.selectedProvince)
                this.ProvincesMap.hoveredProvince.UnHighlightTerrain(this.TerrainSprite.texture, this.ProvincesSprite.texture);
        }
        if (!hoveredProvince.highLighted)
            hoveredProvince.HighlightTerrain(this.TerrainSprite.texture, this.ProvincesSprite.texture);
        this.ProvincesMap.hoveredProvince = hoveredProvince;
    }

    public void ShowTerrain()
    {
        this.GetComponent<SpriteRenderer>().sprite = this.TerrainSprite;
    }

    public void ShowProvinces()
    {
        this.GetComponent<SpriteRenderer>().sprite = this.ProvincesSprite;
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