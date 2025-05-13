using UnityEngine;
using Score;

public class GameManager : MonoBehaviour
{

    public QuestManager questManager;
    public CustomerSpawn customerSpawn;
    public SignedPaperSpawner signedPaperSpawn;
    public Printer stampSpawn;
    public ScoreManager scoreManager;
    public PaperUpdaterPosition paperUpdater;
    public PaperworkBase paperworkBase;
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

}
