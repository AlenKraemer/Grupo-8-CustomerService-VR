using Managers;
using UnityEngine;


public class DeliveredPaperwork : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var paperwork = other.GetComponent<PaperworkBase>();
        if (paperwork == null) paperwork = other.GetComponentInParent<PaperworkBase>();
        if (paperwork == null) paperwork = GameManager.Instance.paperworkBase;
        var currentPaperwork = GameManager.Instance.questManager.GetQuestStatus();
        if (paperwork.paperworkType == currentPaperwork.paperworkType && paperwork.isDone)
        {
            GameManager.Instance.questManager.isObjectiveCompleted = true;
        }
        else
        {
            GameManager.Instance.questManager.isObjectiveCompleted = false;
        }

    }
}
