using UnityEngine;
public class IAModel : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    public MeshFilter meshFilter;
    public Paperwork paperwork;
    public string id;

    public void SetData(IAData data)
    {
        meshFilter.mesh = data.bodyType;
        meshRenderer.material = data.skinColor;
        paperwork = data.paperwork;
        id = data.id;
    }

}
