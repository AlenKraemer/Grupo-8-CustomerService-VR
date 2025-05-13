using UnityEngine;

public class PaperUpdaterPosition : MonoBehaviour
{
    [SerializeField] private Transform paperPos;
    private Vector3 offset;
    private Vector3 initialPos;

    private void Start()
    {
        initialPos = this.transform.position;
        offset = paperPos.position - this.transform.position;
    }

    private void Update()
    {
        paperPos.position = this.transform.position + offset ;
        paperPos.rotation = this.transform.rotation;
    }

    public void SetPaper(Transform paperPos)
    {
        this.paperPos = paperPos;
        this.transform.position = initialPos;
    }
}
