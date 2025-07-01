using Custom;
using Interfaces;
using Managers;
using UnityEngine;

public class PaperUpdaterPosition : MonoBehaviour, IUpdatable
{
    [SerializeField] private Transform paperPos;
    [SerializeField]private Vector3 initialOffset;
    private Vector3 initialPos;
    private Renderer myRenderer;

    private void Start()
    {
        // Add null checks to prevent null reference exceptions
        if (CustomUpdateManager.Instance == null)
        {
            Debug.LogError("CustomUpdateManager.Instance is null. Make sure CustomUpdateManager is initialized before PaperUpdaterPosition.");
            return;
        }
        
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager.Instance is null. Make sure GameManager is initialized before PaperUpdaterPosition.");
            return;
        }
        
        if (GameManager.Instance.questManager == null)
        {
            Debug.LogError("GameManager.Instance.questManager is null.");
            return;
        }

        CustomUpdateManager.Instance.Subscribe(this);
        myRenderer = GetComponent<Renderer>();
        
        if (myRenderer == null)
        {
            Debug.LogWarning("No Renderer component found on PaperUpdaterPosition GameObject.");
        }
        
        initialPos = this.transform.position;
        GameManager.Instance.questManager.onButtonPressed += Retreat;
    }


    public void OnUpdate()
    {
        if (paperPos == null) return;
        paperPos.position = this.transform.position + initialOffset ;
        paperPos.rotation = this.transform.rotation;
    }

    public void SetPaper(Transform paperPos)
    {
        myRenderer.enabled = true;
        this.paperPos = paperPos;
        this.transform.position = initialPos;
    }
    private void Retreat()
    {
        myRenderer.enabled = false;
    }

   
}
