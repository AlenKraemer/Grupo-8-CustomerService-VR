using UnityEngine;

namespace IA.MVC
{
    [System.Serializable]
    public class IAModel
    {
        private MeshRenderer meshRenderer;
        private MeshFilter meshFilter;
        public Paperwork paperwork { get; private set; }
        public string id { get; private set; }

        public IAModel(MeshRenderer meshRenderer, MeshFilter meshFilter)
        {
            this.meshRenderer = meshRenderer;
            this.meshFilter = meshFilter;
        }

        public void SetData(IAData data)
        {
            meshFilter.mesh = data.bodyType;
            meshRenderer.material = data.skinColor;
            paperwork = data.paperwork;
            id = data.id;
        }
    }
}
