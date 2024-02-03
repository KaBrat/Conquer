using UnityEngine;

public class Map
{
    private readonly Sprite sprite;
    private Vector2Int mapSize;

    private Color32[] pixels;
    public readonly object lockObject = new(); // Lock object for synchronization
    public bool PixelsChanged;

    public Map(Sprite sprite)
    {
        this.sprite = sprite;
        this.pixels = sprite.texture.GetPixels32();
        this.mapSize = new Vector2Int(this.sprite.texture.width, this.sprite.texture.height);
    }

    public void UpdateTextureIfPixelsChanged()
    {
        lock (lockObject)
        {
            if (!this.PixelsChanged)
                return;

            this.ApplyChanges();
            this.PixelsChanged = false;
        }
    }

    private void ApplyChanges()
    {
        this.sprite.texture.SetPixels32(this.pixels);
        this.sprite.texture.Apply();
    }

    public Vector2Int GetMapSize()
    {
        return this.mapSize;
    }

    public Color32[] GetPixels32()
    {
        lock (lockObject)
        {
            return this.pixels;
        }
    }

    public Color32 GetPixel(int x, int y)
    {
        lock (lockObject)
        {
            return this.pixels[y * this.mapSize.x + x];
        }
    }

    public void SetPixels(Color32[] pixels)
    {
        lock (lockObject)
        {
            this.pixels = pixels;
            this.PixelsChanged = true;
        }
    }

    public Sprite GetSprite()
    {
        lock (lockObject)
        {
            return this.sprite;
        }
    }
}
