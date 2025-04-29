using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StampReceiver : MonoBehaviour
{
    public void HandleStamp(StampData data)
    {
        GameObject stamp = new GameObject("Stamp");
        
        // Set the stamp slightly above the surface to prevent z-fighting
        Vector3 position = data.position;
        position.z = transform.position.z - 0.01f; // Slight offset from paper surface
        
        // Align rotation to be flat against the paper (assuming paper is flat on the XY plane)
        Quaternion rotation = Quaternion.identity;
        rotation.eulerAngles = new Vector3(0, 0, data.rotation.eulerAngles.z); // Only preserve Z rotation for 2D
        
        stamp.transform.SetPositionAndRotation(position, rotation);

        var spriteRenderer = stamp.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = Sprite.Create(data.stampSO.stampTexture,
            new Rect(0, 0, data.stampSO.stampTexture.width, data.stampSO.stampTexture.height),
            new Vector2(0.5f, 0.5f));

        spriteRenderer.color = GetColorFromInk(data.inkColor);
        stamp.transform.localScale = new Vector3(data.stampSize.x, data.stampSize.y, 1);
        stamp.transform.SetParent(this.transform, true); // Set worldPositionStays to true
        
        // Ensure sorting order is appropriate (higher than the paper)
        spriteRenderer.sortingOrder = 1;
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