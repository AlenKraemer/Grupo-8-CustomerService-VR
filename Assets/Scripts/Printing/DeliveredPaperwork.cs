using Managers;
using UnityEngine;


public class DeliveredPaperwork : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var paperwork = other.GetComponent<PaperworkBase>();
        if (paperwork == null) paperwork = other.GetComponentInParent<PaperworkBase>();
        if (paperwork == null) paperwork = GameManager.Instance.PaperworkBase;
        var currentPaperwork = GameManager.Instance.QuestManager.GetQuestStatus();
        if (paperwork.paperworkType == currentPaperwork.paperworkType && paperwork.isDone)
        {
            GameManager.Instance.QuestManager.isObjectiveCompleted = true;
        }
        else
        {
            GameManager.Instance.QuestManager.isObjectiveCompleted = false;
        }

    }
}
