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

            if (TotalGameManager.Instance.playerResultSocres.Count < 1)
            {
                Debug.Log("Player ������ �������� ����");
                // TODO: ��Ʈ��ũ ������ palyer ������ ���� ���, ���� ������ ��ü
            }
            else
            {
                var queryDic = TotalGameManager.Instance.playerResultSocres.OrderByDescending(x => x.Value);

                int i = 0;
                foreach(var item in queryDic) 
                {
                    _playerNames[i].text = item.Key;
                    _ratios[i].text = item.Value.ToString("F1");
                    i++;
                }
            }

            _lastSize.text = TotalGameManager.Instance.resultSlimeSize.ToString("F1");
        }

        [Button]
        void Test(float LastSize, Dictionary<string, float> keyValuePairs)
        {
            float lastSize = LastSize;
            IOrderedEnumerable<KeyValuePair<string, float>> nickNameSlimeRatioPair = keyValuePairs.OrderByDescending(x => x.Value);

            Dictionary<string, float> SortDictionary(Dictionary<string, float> dict)
            {
                // ���������� ascending�� descending���� ����
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
