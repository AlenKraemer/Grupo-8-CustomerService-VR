using UnityEngine;

public class IAController : MonoBehaviour
{
    [SerializeField]private ChatBubble chatBubble;
    [SerializeField] private IAData dataTest;
    private IAModel _iAModel;

    private void Awake()
    {
        _iAModel = GetComponent<IAModel>();
    }

    public void Initialize(IAData data)
    {
        _iAModel.SetData(data);
        chatBubble.SetText(_iAModel.paperwork.paperworkText);
        GameManager.Instance.questManager.AddQuestToQueue(_iAModel.id, _iAModel.paperwork.paperworkType);
        GameManager.Instance.questManager.onButtonPressedCustomer += Retreat;
    }

    private void Retreat()
    {
        GameManager.Instance.customerSpawn.FinishedCustomer(this);

    }


}
