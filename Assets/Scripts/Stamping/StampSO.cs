using UnityEngine;

[CreateAssetMenu(fileName = "StampTool", menuName = "UADE/Stamp/Tool")]
public class StampSO : ScriptableObject
{
    public string toolName;
    public Texture2D stampTexture;
}
