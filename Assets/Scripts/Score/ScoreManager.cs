using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private BasicScore _tramitesCompletados;
    
    // Method to get direct access to the TramitesCompletados score component
    public BasicScore TramitesCompletadosScore()
    {
        return _tramitesCompletados;
    }
}