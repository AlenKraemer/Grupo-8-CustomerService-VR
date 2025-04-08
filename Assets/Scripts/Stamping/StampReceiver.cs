using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StampReceiver : MonoBehaviour
{
    public void HandleStamp(StampData data)
    {
        GameObject stamp = new GameObject("Stamp");
        stamp.transform.SetPositionAndRotation(data.position, data.rotation);

        var spriteRenderer = stamp.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = Sprite.Create(data.stampSO.stampTexture,
            new Rect(0, 0, data.stampSO.stampTexture.width, data.stampSO.stampTexture.height),
            new Vector2(0.5f, 0.5f));

        spriteRenderer.color = GetColorFromInk(data.inkColor);
        stamp.transform.localScale = new Vector3(data.stampSize.x, data.stampSize.y, 1);
        stamp.transform.SetParent(this.transform);
    }

    private Color GetColorFromInk(StampInkColor ink)
    {
        return ink switch
        {
            StampInkColor.Red => Color.red,
            StampInkColor.Green => Color.green,
            StampInkColor.Black => Color.black,
            _ => Color.gray
        };
    }
}
