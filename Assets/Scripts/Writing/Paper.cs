using System;
using UnityEngine;

public class Paper : PaperworkBase
{
    public Texture2D texture;
    public Renderer thisRenderer;
    public Vector2 textureSize = new Vector2(2048,2048);

    void Start()
    {
        paperworkType = PaperworkType.signature;
        isDone = false;
        texture = new Texture2D((int)textureSize.x, (int)textureSize.y);
        thisRenderer.material.mainTexture = texture;
        GameManager.Instance.questManager.onButtonPressed += Retreat;
    }

    public void InitializePaper()
    {
        paperworkType = PaperworkType.signature;
        isDone = false;
        texture = new Texture2D((int)textureSize.x, (int)textureSize.y);
        thisRenderer.material.mainTexture = texture;
    }

    private void Retreat()
    {
        if (!isDone) return;
        GameManager.Instance.signedPaperSpawn.Finished(this);
    }

    private void OnDisable()
    {
        paperworkType = PaperworkType.signature;
        isDone = false;
    }

}
