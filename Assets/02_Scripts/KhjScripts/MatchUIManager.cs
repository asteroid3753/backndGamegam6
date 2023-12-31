using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace khj
{
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
            MatchManager.Instance.Initialize();
        }

        void ShowReadyPanel()
        {
            matchPanel.SetActive(false);
            readyPanel.SetActive(true);

        }

        private void OnDisable()
        {
            TotalGameManager.OnGameReady -= ShowReadyPanel;
        }
    }
}