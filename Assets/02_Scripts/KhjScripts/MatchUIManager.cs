using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchUIManager : MonoBehaviour
{
    [SerializeField] GameObject loadingPanel;
    [SerializeField] GameObject matchPanel;
    [SerializeField] GameObject matchManager;

    void Awake()
    {
        loadingPanel.SetActive(true);
        matchPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
