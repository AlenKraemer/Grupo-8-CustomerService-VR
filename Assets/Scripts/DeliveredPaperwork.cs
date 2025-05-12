using System;
using UnityEngine;


public class DeliveredPaperwork : MonoBehaviour
{
    public static Action onPaperworkDelivered;
    private void OnTriggerEnter(Collider other)
    {
        var paperwork = other.GetComponent<PaperworkBase>();
        if(paperwork.paperworkType == LevelManager.Instance.CurrentPaperwork && paperwork.isDone)
        {
            //sumar puntos
            Debug.Log("sume puntos");
        }
        else
        {
            //restar puntos
            Debug.Log("reste puntos");

        }
        LevelManager.Instance.NewCustomer();
        onPaperworkDelivered?.Invoke();
    }
}
