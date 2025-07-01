using UnityEngine;

namespace IA
{
    [CreateAssetMenu(fileName = "IAData", menuName ="ScriptableObjects/IAData")]
    public class IAData : ScriptableObject
    {
        public Mesh bodyType;
        public Material skinColor;
        public Paperwork paperwork;
        public string id;
    }

    [System.Serializable]
    public struct Paperwork
    {
        public PaperworkType paperworkType;
        public string paperworkText;
    }
}