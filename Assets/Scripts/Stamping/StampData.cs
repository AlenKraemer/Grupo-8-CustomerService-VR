using UnityEngine;

namespace Stamping
{
    [System.Serializable]
    public class StampData
    {
        public Vector3 position;
        public Quaternion rotation;
        public StampSO stampSO;
        public StampInkColor inkColor;
        public Vector3 stampSize;

        public StampData(Vector3 position, Quaternion rotation, StampSO stampSO, StampInkColor inkColor, Vector3 stampSize)
        {
            this.position = position;
            this.rotation = rotation;
            this.stampSO = stampSO;
            this.inkColor = inkColor;
            this.stampSize = stampSize;
        }
    }
}