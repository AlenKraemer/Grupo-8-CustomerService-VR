using UnityEngine;

public class ShowUI : MonoBehaviour
{

    public GameObject UI;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            UI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            UI.SetActive(false);
        }
    }
}
