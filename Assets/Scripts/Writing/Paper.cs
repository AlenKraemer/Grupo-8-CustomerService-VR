using UnityEngine;

public class Paper : PaperworkBase
{
    public Texture2D texture;
    public Vector2 textureSize = new Vector2(2048,2048);
    void Start()
    {
        paperworkType = PaperworkType.signature;
        var renderer = GetComponent<Renderer>();
        texture = new Texture2D((int)textureSize.x, (int)textureSize.y);
        renderer.material.mainTexture = texture;
    }

    
}
