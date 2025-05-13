using System;
using UnityEngine;

public class PaperUpdaterPosition : MonoBehaviour
{
    [SerializeField] private Transform paperPos;
    [SerializeField]private Vector3 initialOffset;
    private Vector3 initialPos;
    private Renderer myRenderer;

    private void Start()
    {
        myRenderer = GetComponent<Renderer>();
        initialPos = this.transform.position;
        GameManager.Instance.questManager.onButtonPressed += Retreat;
    }


    private void Update()
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
