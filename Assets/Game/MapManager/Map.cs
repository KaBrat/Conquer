using UnityEngine;

public class Map
{
    private readonly Sprite sprite;
    private Color32[] pixels;
    private Vector2Int mapSize;

    public Map(Sprite sprite)
    {
        this.sprite = sprite;
        this.pixels = sprite.texture.GetPixels32();
        this.mapSize = new Vector2Int(this.sprite.texture.width, this.sprite.texture.height);
    }

    public void updateSprite(Color32[] pixels)
    {
        this.pixels = pixels;
        this.sprite.texture.SetPixels32(this.pixels);
        this.sprite.texture.Apply();
    }

    public Vector2Int GetMapSize() => this.mapSize;
    public Color32[] GetPixels32() => this.pixels;
    public Color32 GetPixel(int x, int y) => this.pixels[y * this.mapSize.x + x];
    public void SetPixels(Color32[] pixels) => this.pixels = pixels;
    public Sprite GetSprite() => this.sprite;
}