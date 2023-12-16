using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchUIManager : MonoBehaviour
{
    [SerializeField] GameObject matchPanel;
    [SerializeField] GameObject readyPanel;
    [SerializeField] GameObject matchManager;

    void Awake()
    {
        matchPanel.SetActive(true);
        readyPanel.SetActive(false);
    }

    private void Start()
    {
        TotalGameManager.OnGameReady += ShowReadyPanel;
    }

    void ShowReadyPanel()
    {
        matchPanel.SetActive(false);
        readyPanel.SetActive(true);

    }
}
