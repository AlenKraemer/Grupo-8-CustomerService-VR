using UnityEngine;

public class IAController : MonoBehaviour
{
    [SerializeField]private ChatBubble chatBubble;
    [SerializeField] private IAData dataTest;
    private IAModel _iAModel;

    private void Start()
    {
        _iAModel = GetComponent<IAModel>();
        _iAModel.SetData(dataTest);
        chatBubble.SetText(_iAModel.paperwork.paperworkText);
        LevelManager.Instance.SetPaperwork(_iAModel.paperwork.paperworkType);
        DeliveredPaperwork.onPaperworkDelivered += Retreat;

    }

    public void Initialize(IAData data)
    {
        _iAModel = GetComponent<IAModel>();
        _iAModel.SetData(data);
        chatBubble.SetText(_iAModel.paperwork.paperworkText);
        LevelManager.Instance.SetPaperwork(_iAModel.paperwork.paperworkType);
    }

    private void Retreat()
    {
        //cambiar esto a que se apague con un pool
        Destroy(this.gameObject);
    }
}
