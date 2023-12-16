using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LJH;
using BackEnd;
using BackEnd.Tcp;
using MorningBird.SceneManagement;
using MorningBird.Sound;
using System.Numerics;
using KSY;
using BackEnd.Game;
using KSY.Protocol;
using Protocol;

namespace JES
{
    public class InGameManager : MonoBehaviour
    {
        [SerializeField] GameObject _playerPrefab;
        [SerializeField] Transform[] _playerPositions;
        [SerializeField] GameObject[] _growingItemPrefabs;
        // [SerializeField] List<GroawingItems>() = new List<GroawingItems>();
        [SerializeField] string[] _playerNickNames;
        [SerializeField] string _superPlayerNickName;
        [SerializeField] string _myClientNickName;

        [SerializeField] Dictionary<string, LJH.Player> _namePlayerPairs;
        public Dictionary<string, LJH.Player> NamePlayerPairs;

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
            StartCoroutine(IEWaitAndStart());

        }

        IEnumerator IEWaitAndStart()
        {

            yield return new WaitForSeconds(0.5f);

            // Regest Event
            {
                BackEndManager.Instance.Parsing.GrabItemEvent += Parsing_GrabItemEvent;
            }

            // Setting Players
            {
                _playerNickNames = TotalGameManager.Instance.playerNickNames;
                _superPlayerNickName = TotalGameManager.Instance.host;
                _myClientNickName = TotalGameManager.Instance.myNickName;
                bool isSuperPlayer = TotalGameManager.Instance.isHost;

                NamePlayerPairs = new Dictionary<string, LJH.Player>();

                for (int i = 0; i < _playerNickNames.Length; i++)
                {
                    LJH.Player player = Instantiate(_playerPrefab).GetComponent<LJH.Player>();
                    player.SetUserName(_playerNickNames[i]);

                    if (isSuperPlayer == true)
                    {
                        //tPlayer.SetSuperPlayer(true);
                    }
                    else
                    {
                        //tPlayer.SetSuperPlayer(false);
                    }

                    // tPlayer.SetAnimalType(Etype (int)i)

                    player.transform.position = _playerPositions[i].position;

                    NamePlayerPairs.Add(_playerNickNames[i], player);
                }
            }

            // �̰� �ص� ����.
            // �ڽ��� �������� �˾ƾ� �ϰ�, �׸��� �װ� �ڽ��ΰ� �˾ƾ� �Ѵ�.
            // �׸��� �̸� �ɷ��� �ൿ�ؾ� �Ѵ�.

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

        private void Update()
        {
            // ������ ���� üũ
            // // ������ ���� ����

        }

        private void Parsing_GrabItemEvent(int obj)
        {
            Debug.Log(obj);
        }
    } 
}
