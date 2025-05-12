using UnityEngine;

[CreateAssetMenu(fileName = "IAData", menuName ="ScriptableObjects/IAData")]
public class IAData : ScriptableObject
{
    public Mesh bodyType;
    public Material skinColor;
    public Paperwork paperwork;
}

[System.Serializable]
public struct Paperwork
{
    public PaperworkType paperworkType;
    public string paperworkText;
}