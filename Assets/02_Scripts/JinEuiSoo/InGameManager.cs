using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LJH;
using Player = LJH.Player;
using BackEnd;
using BackEnd.Tcp;
using MorningBird.SceneManagement;
using MorningBird.Sound;
using System.Numerics;
using KSY;
using BackEnd.Game;
using KSY.Protocol;

namespace JES
{
    public class InGameManager : MonoBehaviour
    {
        [SerializeField] GameObject _playerPrefab;
        [SerializeField] Transform[] _playerPositions;
        [SerializeField] GameObject[] _growingItemPrefabs;
        // [SerializeField] List<GroawingItems>() = new List<GroawingItems>();
        [SerializeField] string[] _playerNicNames;
        [SerializeField] string _superPlayerNickName;

        #region Singleton

        static InGameManager _instance;
        /// <summary>
        /// TotalGameSceneManager
        /// </summary>
        internal static InGameManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    var obj = FindObjectOfType<InGameManager>();

                    if (obj != null)
                    {
                        _instance = obj;
                    }
                    else
                    {
                        var newSingleton = GameObject.Find("InGameManager").GetComponent<InGameManager>();
                        _instance = newSingleton;
                    }
                }
                return _instance;
            }
        }

        #endregion

        private void Awake()
        {
            #region Singleton Instantiate
            var objs = FindObjectsOfType<InGameManager>();
            if (objs.Length > 1)
            {
                Debug.LogError("New InGameManager Added And Destroy Automatically");
                Destroy(this.gameObject);
                return;
            }
            #endregion
            AwakeInitialize();
        }

        void AwakeInitialize()
        {

        }

        void Start()
        {
            // Regest Event
            {
                BackEndManager.Instance.Parsing.GrabItemEvent += Parsing_GrabItemEvent;
            }

            // Setting Players
            {
                string[] playerNickNames = { "1", "2", "3", "4" }; /* _playerNicNames = TotalGameManager.Instance.GetPlayerNickNames(); */
                string superPlayerNicNames = "1"; /* _superPlayerNickName = TotalGameManager.Instance.GetSuperGamePlayer(); */

                for (int i = 0; i < playerNickNames.Length; i++)
                {
                    bool isSuperPlayer = false;
                    if (playerNickNames[i] == superPlayerNicNames)
                    {
                        isSuperPlayer = true;
                    }


                    GameObject tGO = Instantiate(_playerPrefab);
                    LJH.Player tplayer = tGO.GetComponent<LJH.Player>();

                    tplayer.NickName = playerNickNames[i];

                    if (isSuperPlayer == true)
                    {
                        //tPlayer.SetSuperPlayer(true);
                    }
                    else
                    {
                        //tPlayer.SetSuperPlayer(false);
                    }

                    // tPlayer.SetAnimalType(Etype (int)i)

                    // tGO.transform.position = _playerPositions[i].position;

                }
            }

            // Setting Items
            {
                
            }

            // ������ ���� ����
            // �������� �Ѹ���, �������� �����. �������� �����Ѵ�.
            // int �ε����� �ް�, �� �ش��ϴ� ���� �����.
            // 
            // null�� �߻��� ����, �Ѵ� ��� ������ ó���Ѵ�.
            // null�� �߻��� �� �־, null�� �߻��ϸ� Catch�ϸ鼭 �����Ų��.


            //GrabItemMessage msg = new GrabItemMessage(333);
            //BackEndManager.Instance.InGame.SendDataToInGame(msg);

        }

        private void Parsing_GrabItemEvent(int obj)
        {
            
        }
    } 
}
