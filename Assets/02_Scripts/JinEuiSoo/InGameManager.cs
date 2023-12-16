using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using LJH;
using BackEnd;
using BackEnd.Tcp;
using MorningBird.SceneManagement;
using MorningBird.Sound;
using KSY;
using BackEnd.Game;
using KSY.Protocol;
using UnityEngine.SceneManagement;

namespace LJH
{
    public class InGameManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject itemPrefab;
        [SerializeField]
        private float itemSpawnSpan = 1f;

        [SerializeField] bool _isGameEnd = false;

        [SerializeField] GameObject _playerPrefab;
        [SerializeField] Transform[] _playerPositions;
        [SerializeField] GameObject[] _growingItemPrefabs;
        // [SerializeField] List<GroawingItems>() = new List<GroawingItems>();
        [SerializeField] string[] _playerNickNames;
        [SerializeField] string _superPlayerNickName;
        [SerializeField] string _myClientNickName;

        public Dictionary<string, LJH.PlayerController> NamePlayerPairs;

        public Dictionary<int, GrowingItem> InGameItemDic;
        
        int itemCount = 0;

        BoxCollider2D slimeArea;
        BoxCollider2D groundArea;

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
            slimeArea = GameObject.Find("Slime").GetComponent<BoxCollider2D>();
            groundArea = GameObject.Find("Ground").GetComponent<BoxCollider2D>();
            JES.JESFunctions.SetCollider(groundArea, slimeArea);
        }

        void Start()
        {
            // Regest Event
            {
                BackEndManager.Instance.Parsing.GrabItemEvent += Parsing_GrabItemEvent;
                BackEndManager.Instance.Parsing.CreateItemEvent += Parsing_CreateItemEvent;
                BackEndManager.Instance.Parsing.PlayerMoveEvent += Parsing_PlayerMove;

                Backend.Match.OnLeaveInGameServer += OnLeaveInGameServerEvent;

            }


            StartCoroutine(IEWaitAndStart());

        }

        IEnumerator IEWaitAndStart()
        {

            yield return new WaitForSeconds(.5f);



            // Setting Players
            {
                _playerNickNames = TotalGameManager.Instance.playerNickNames;
                _superPlayerNickName = TotalGameManager.Instance.host;
                _myClientNickName = TotalGameManager.Instance.myNickName;
                bool isSuperPlayer = TotalGameManager.Instance.isHost;

                NamePlayerPairs = new Dictionary<string, LJH.PlayerController>();
                InGameItemDic = new Dictionary<int, GrowingItem>();

                for (int i = 0; i < _playerNickNames.Length; i++)
                {
                    GameObject player = Instantiate(_playerPrefab);//, _playerPositions[i].position, Quaternion.identity);
                    LJH.PlayerController playerController = player.GetComponent<LJH.PlayerController>();
                    NamePlayerPairs.Add(_playerNickNames[i], playerController);
                    player.GetComponent<Player>().SetUserName(_playerNickNames[i]);

                    if (isSuperPlayer == true)
                    {
                        //tPlayer.SetSuperPlayer(true);
                    }
                    else
                    {
                        //tPlayer.SetSuperPlayer(false);
                    }

                    // tPlayer.SetAnimalType(Etype (int)i)
                    if (_playerNickNames[i].ToString() == _myClientNickName){
                        Debug.Log(_playerPositions[i].position);
                        player.GetComponent<Player>().SetUserTarget(_playerPositions[i].position);
                        player.gameObject.AddComponent<InputManager>().SetFirstPos(_playerPositions[i].position);
                        player.transform.position = _playerPositions[i].position;
                    }
                    else{
                        player.transform.position = _playerPositions[i].position;
                        //player.GetComponent<Player>().SetUserTarget(_playerPositions[i].position);
                    }
                    // if (_playerNickNames[i].ToString() == _myClientNickName)
                    // {
                        
                    //     PlayerMoveMessage msg = new PlayerMoveMessage(player.transform.position);
                    //     BackEndManager.Instance.InGame.SendDataToInGame(msg);
                    // }
                    // player.transform.position = _playerPositions[i].position;
                    // player.GetComponent<Player>().SetUserTarget(_playerPositions[i].position);
                    
                }
            }

            // Setting Items
            {
                // �ڽ��� ȣ��Ʈ�� ��쿡 ������ ���� ����
                if (TotalGameManager.Instance.isHost)
                {
                    itemCount = 0;
                    StartCoroutine(CreateItem());
                }
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

        IEnumerator CreateItem()
        {
            while (true)
            {
                int itemType = Random.Range(0, 3);
                UnityEngine.Vector2 spawnPos = JES.JESFunctions.CreateRandomInstance();

                CreateItemMessage msg = new CreateItemMessage(itemType, itemCount, spawnPos);
                BackEndManager.Instance.InGame.SendDataToInGame(msg);
                itemCount++;
                yield return new WaitForSeconds(itemSpawnSpan);
            }
        }

        int testCnt = 0;
        private void Update()
        {
            #region GameEndingConditionCheck


            if (_isGameEnd == true)
            {
                DeclareMatchEnd();
                _isGameEnd = false;
            }

            #endregion

            // ������ ���� üũ
            // // ������ ���� ����
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log($"{testCnt}�� �ƿ��� ����");
                BackEndManager.Instance.InGame.SendDataToInGame(new GrabItemMessage(testCnt++));
            }


            // If Someone want, Change the DeclareMatchEnd. But, Have to check IsInGameServerConnet() for checking the InGame is running.

        }

        private void OnDisable()
        {
            UnSubscribeOnLeaveInGameServerEvent();
        }

        #region EndingMatch
        void OnLeaveInGameServerEvent(MatchInGameSessionEventArgs args)
        {
            if (args.ErrInfo == ErrorCode.Success)
            {
                Debug.Log("OnLeaveInGameServer 인게임 서버 접속 종료 : " + args.ErrInfo.ToString());
            }
            else
            {
                Debug.LogError("OnLeaveInGameServer 인게임 서버 접속 종료 : " + args.ErrInfo + " / " + args.Reason);
            }
        }

        void UnSubscribeOnLeaveInGameServerEvent()
        {
            Backend.Match.OnLeaveInGameServer -= OnLeaveInGameServerEvent;
        }

        void DeclareMatchEnd()
        {
            Backend.Match.OnMatchResult = (MatchResultEventArgs args) =>
            {
                if (args.ErrInfo == ErrorCode.Success)
                {
                    Debug.Log("8-2. OnMatchResult 성공 : " + args.ErrInfo.ToString());
                }
                else
                {
                    Debug.LogError("8-2. OnMatchResult 실패 : " + args.ErrInfo.ToString());
                }

                TotalGameManager.Instance.ChangeState(TotalGameManager.GameState.Result);
                



            };

            Debug.Log("8-1. MatchEnd 호출");

            //MatchGameResult matchGameResult = new MatchGameResult();
            //matchGameResult.m_winners = new List<SessionId>();
            //matchGameResult.m_losers = new List<SessionId>();

            //foreach (var session in inGameUserList)
            //{
            //    // 순서는 무관합니다.
            //    matchGameResult.m_winners.Add(session.Value.m_sessionId);
            //}

            //Backend.Match.MatchEnd(matchGameResult);

            var matchResult = new MatchGameResult();

            Backend.Match.MatchEnd(matchResult);
        } 
        #endregion

        #region PasingEventFunc

        private void Parsing_GrabItemEvent(string nickname, int itemCode)
        {
            //NamePlayerPairs[nickname].
            if (InGameItemDic.ContainsKey(itemCode))
            {
                Destroy(InGameItemDic[itemCode].gameObject);
                InGameItemDic.Remove(itemCode);
            }
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

        private void Parsing_PlayerMove(string nickName, Vector2 target)
        {
            Debug.Log(nickName+"/"+target);
            NamePlayerPairs[nickName].PlayerMoveRecvFunc(target);
        }
    } 
}
