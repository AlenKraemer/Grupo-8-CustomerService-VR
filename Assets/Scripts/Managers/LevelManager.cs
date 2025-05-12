using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    public PaperworkType CurrentPaperwork { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    public void NewCustomer()
    {
        //instanciar e inicializar al nuevo customer
    }

    public void SetPaperwork(PaperworkType paperworkType)
    {
        CurrentPaperwork = paperworkType;
    }
}
