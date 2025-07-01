using UnityEngine;

namespace Customers
{
    public class ShowUI : MonoBehaviour
    {

        public GameObject UI;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                UI.SetActive(true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                UI.SetActive(false);
            }
        }
    }
}
