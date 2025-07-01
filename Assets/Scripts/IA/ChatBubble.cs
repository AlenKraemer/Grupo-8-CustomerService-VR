using UnityEngine;
using TMPro;

public class ChatBubble : MonoBehaviour
{
    [SerializeField] private SpriteRenderer backgroundSpriteRenderer;
    [SerializeField] private TextMeshPro textMeshPro;
    [SerializeField] private Vector2 textOffset;

    private Vector2 _textSize;


    public void SetText(string text)
    {
        textMeshPro.SetText(text);
        textMeshPro.ForceMeshUpdate();
        _textSize = textMeshPro.GetRenderedValues(false);

        backgroundSpriteRenderer.size = _textSize + textOffset;
    }
}
