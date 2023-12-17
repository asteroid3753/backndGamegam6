using MorningBird.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;
using Sirenix.OdinInspector;
using BackEnd;
using khj;

namespace JES
{
    public class ResultSceneManager : SerializedMonoBehaviour
    {
        [SerializeField] TMPro.TextMeshProUGUI[] _playerNames;
        [SerializeField] TMPro.TextMeshProUGUI[] _ratios;
        [SerializeField] TMPro.TextMeshProUGUI _lastSize;

        private void Start()
        {
            float lastSize = ShowingResultCommunicator.Instance.LastSize;
            IOrderedEnumerable<KeyValuePair<string, float>> nickNameSlimeRatioPair = ShowingResultCommunicator.Instance.NickNameSlimeSizeRatioPair.OrderByDescending(x => x.Value);

            Dictionary<string, float> SortDictionary(Dictionary<string, float> dict)
            {
                // 내림차순은 ascending을 descending으로 변경
                var sortVar = from item in dict
                              orderby item.Value descending
                              select item;

                return sortVar.ToDictionary(x => x.Key, x => x.Value);
            }

            int t_order = 0;

            foreach (var item in nickNameSlimeRatioPair)
            {
                _playerNames[t_order].text = item.Key;
                _ratios[t_order].text = item.Value.ToString();

                t_order++;
            }

            _lastSize.text = lastSize.ToString();

        }

        [Button]
        void Test(float LastSize, Dictionary<string, float> keyValuePairs)
        {
            float lastSize = LastSize;
            IOrderedEnumerable<KeyValuePair<string, float>> nickNameSlimeRatioPair = keyValuePairs.OrderByDescending(x => x.Value);

            Dictionary<string, float> SortDictionary(Dictionary<string, float> dict)
            {
                // 내림차순은 ascending을 descending으로 변경
                var sortVar = from item in dict
                              orderby item.Value descending
                              select item;

                return sortVar.ToDictionary(x => x.Key, x => x.Value);
            }

            int t_order = 0;

            foreach (var item in nickNameSlimeRatioPair)
            {
                _playerNames[t_order].text = item.Key;
                _ratios[t_order].text = item.Value.ToString();

                t_order++;
            }

            _lastSize.text = lastSize.ToString();
        }

        public void BackToLogin()
        {
            var bro = Backend.BMember.Logout();
            TotalGameManager.Instance.ChangeState(TotalGameManager.GameState.Login);
            GameSceneLoadManager.Instance.UnLoadAllScenes();
            GameSceneLoadManager.Instance.LoadSceneAsync("Login");
        }

        public void Quit()
        {
            Debug.LogWarning("Quiting Application");
            Application.Quit();
        }
    } 
}
