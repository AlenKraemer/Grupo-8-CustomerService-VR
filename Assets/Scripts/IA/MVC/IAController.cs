using Managers;
using UnityEngine;

namespace IA.MVC
{
    public class IAController : MonoBehaviour
    {
        [SerializeField] private ChatBubble _chatBubble;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private MeshFilter _meshFilter;
    
        private IAModel _iAModel;

        private void Awake()
        {
            _iAModel = new IAModel(_meshRenderer, _meshFilter);
        }

        public void Initialize(IAData data)
        {
            _iAModel.SetData(data);
            _chatBubble.SetText(_iAModel.paperwork.paperworkText);
            GameManager.Instance.questManager.AddQuestToQueue(_iAModel.id, _iAModel.paperwork.paperworkType);
            GameManager.Instance.questManager.onButtonPressedCustomer += Retreat;
        }

        private void Retreat()
        {
            GameManager.Instance.FinishedCustomer(this);
            GameManager.Instance.questManager.onButtonPressedCustomer -= Retreat;
        }
    }
}
