using JES;
using KSY;
using MorningBird.Sound;
using UnityEngine;

namespace KSY
{
    public class GameEndUI : MonoBehaviour
    {
        [SerializeField]
        AudioStorage endSound;

        [SerializeField]
        GameObject gameEndPanel;
        // Start is called before the first frame update
        void Start()
        {    
            BackEndManager.Instance.Parsing.EndGameEvent += GameEndEventFunc;
        }

        private void GameEndEventFunc()
        {
            SoundManager.Instance.RequestPlayClip(endSound);
            gameEndPanel.SetActive(true);
            BackEndManager.Instance.Parsing.EndGameEvent -= GameEndEventFunc;
        }
    }

}