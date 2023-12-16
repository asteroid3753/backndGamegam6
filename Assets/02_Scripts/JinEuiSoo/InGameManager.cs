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
using Protocol;

namespace JES
{
    public class InGameManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject itemPrefab;

        [SerializeField] GameObject _playerPrefab;
        [SerializeField] Transform[] _playerPositions;
        [SerializeField] GameObject[] _growingItemPrefabs;
        // [SerializeField] List<GroawingItems>() = new List<GroawingItems>();
        [SerializeField] string[] _playerNickNames;
        [SerializeField] string _superPlayerNickName;
        [SerializeField] string _myClientNickName;

        [SerializeField] Dictionary<string, TestPlayer> _namePlayerPairs;
        public Dictionary<string, TestPlayer> NamePlayerPairs;

        public Dictionary<int, GrowingItem> InGameItemDic;

        int itemCount = 0;
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
                BackEndManager.Instance.Parsing.CreateItemEvent += Parsing_CreateItemEvent;
            }

            // Setting Players
            {
                _playerNickNames = TotalGameManager.Instance.playerNickNames;
                _superPlayerNickName = TotalGameManager.Instance.host;
                _myClientNickName = TotalGameManager.Instance.myNickName;

                NamePlayerPairs = new Dictionary<string, TestPlayer>();
                InGameItemDic = new Dictionary<int, GrowingItem>();

                for (int i = 0; i < _playerNickNames.Length; i++)
                {
                    bool isSuperPlayer = false;
                    if (_playerNickNames[i] == _superPlayerNickName)
                    {
                        isSuperPlayer = true;
                    }


                    GameObject tGO = Instantiate(_playerPrefab);
                    TestPlayer tplayer = tGO.GetComponent<TestPlayer>();

                    tplayer.PlayerNickName = _playerNickNames[i];
                    tplayer.MyClientNickName = _myClientNickName;

                    if (isSuperPlayer == true)
                    {
                        //tPlayer.SetSuperPlayer(true);
                    }
                    else
                    {
                        //tPlayer.SetSuperPlayer(false);
                    }

                    // tPlayer.SetAnimalType(Etype (int)i)

                    tGO.transform.position = _playerPositions[i].position;

                    NamePlayerPairs.Add(_playerNickNames[i], tplayer);
                }
            }

            // Setting Items
            {
                // 자신이 호스트인 경우에 아이템 생성 로직
                if (TotalGameManager.Instance.isHost)
                {
                    itemCount = 0;
                    StartCoroutine(CreateItem());
                }
            }

            // 아이템 관련 로직
            // 아이템을 뿌리고, 아이템을 지운다. 아이템을 관리한다.
            // int 인덱스를 받고, 그 해당하는 것을 지운다.
            // 
            // null이 발생할 때는, 둘다 얻는 것으로 처리한다.
            // null이 발생할 수 있어서, null이 발생하면 Catch하면서 종료시킨다.


            //GrabItemMessage msg = new GrabItemMessage(333);
            //BackEndManager.Instance.InGame.SendDataToInGame(msg);
        }

        IEnumerator CreateItem()
        {
            while (true)
            {
                int itemType = Random.Range(0, 3);
                UnityEngine.Vector2 spawnPos = new UnityEngine.Vector2(0, itemCount);

                CreateItemMessage msg = new CreateItemMessage(itemType, itemCount, spawnPos);
                BackEndManager.Instance.InGame.SendDataToInGame(msg);
                itemCount++;
                yield return new WaitForSeconds(3);
            }
        }

        private void Update()
        {
            // 게임의 엔딩 체크
            // // 게임의 엔딩 선언

        }

        #region PasingEventFunc

        private void Parsing_GrabItemEvent(int obj)
        {
            Debug.Log(obj);
        }

        private void Parsing_CreateItemEvent(int itemType, int itemCode, UnityEngine.Vector2 arg3)
        {
            if (InGameItemDic != null)
            {
                GameObject obj = Instantiate(itemPrefab);
                obj.transform.position = arg3;
                GrowingItem item = obj.GetComponent<GrowingItem>();
                item.ItemCode = itemCode;
                item.Type = (Define.ItemType)itemType;
                InGameItemDic.Add(itemCode, item);
            }       
        } 
        #endregion
    } 
}
