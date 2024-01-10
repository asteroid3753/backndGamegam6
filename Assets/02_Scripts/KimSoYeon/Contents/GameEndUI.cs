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
            BackEndManager.Instance.Parsing.TotalScoreEvent -= GameEndEventFunc;
            BackEndManager.Instance.Parsing.TotalScoreEvent += GameEndEventFunc;
        }

        private void GameEndEventFunc(float[] obj)
        {
            Debug.Log("TotalScore 이벤트 호출 GameEnd");
            //BackEndManager.Instance.Parsing.TotalScoreEvent -= Parsing_TotalScoreEvent;
            gameEndPanel.SetActive(true);

            SoundManager.Instance.RequestPlayClip(endSound);
        }
    }

}