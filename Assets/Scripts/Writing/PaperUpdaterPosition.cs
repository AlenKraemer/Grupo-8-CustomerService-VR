using UnityEngine;

public class PaperUpdaterPosition : MonoBehaviour, IUpdatable
{
    [SerializeField] private Transform paperPos;
    [SerializeField]private Vector3 initialOffset;
    private Vector3 initialPos;
    private Renderer myRenderer;

    private void Start()
    {
        CustomUpdateManager.Instance.Suscribe(this);
        myRenderer = GetComponent<Renderer>();
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
