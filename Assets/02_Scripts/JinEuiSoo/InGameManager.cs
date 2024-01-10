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
using System.Linq;
using Cinemachine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;
using khj;

namespace LJH
{
    public partial class InGameManager : SerializedMonoBehaviour
    {
        [SerializeField]
        private GameObject gaugePrefab;

        [SerializeField]
        private SlimeSizeUI slimeUIPrefab;

        [SerializeField]
        private GameObject itemPrefab;
        [SerializeField]
        private float itemSpawnSpan = 1f;

        [SerializeField]
        private float slimeEndScale = 0.5f;

        [SerializeField] bool _isGameEnd = false;
        [SerializeField] GameObject gaugeObj;
        [SerializeField] GameObject cameraPrefab;
        [SerializeField] GameObject[] _playerPrefab;
        [SerializeField] Transform[] _playerPositions;
        // [SerializeField] List<GroawingItems>() = new List<GroawingItems>();
        [SerializeField] List<string> _playerNickNames;
        [SerializeField] string _superPlayerNickName;
        [SerializeField] string _myClientNickName;
        [SerializeField]
        Color[] _gaugeColors = new Color[4] { new Color32(255, 255, 255, 255), new Color32(100, 112, 255, 255),
        new Color32(255, 201, 98, 255), new Color32(186, 83, 255, 255)};

        public Dictionary<string, LJH.Player_Logic> NamePlayerPairs;

        public EventDictionary<int, GrowingItem> InGameItemDic;

        public Dictionary<string, GaugeElement> GaugeDic;
        public Dictionary<string, float> ScoreDic;

        int itemCount = 0;

        private float totalScore = 0;

        [SerializeField] BoxCollider2D slimeArea;
        [SerializeField] BoxCollider2D groundArea;
        GameObject slimeObj;

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

        #endregion

        void AwakeInitialize()
        {
            slimeObj = GameObject.Find("Slime");
            slimeUIPrefab.InitSlimeSize = slimeEndScale;
            //slimeArea = GameObject.Find("Slime").GetComponent<BoxCollider2D>();
            //groundArea = GameObject.Find("Ground").GetComponent<BoxCollider2D>();
            JES.JESFunctions.SetCollider(groundArea, slimeArea);
        }

        void Start()
        {
            // Regest Event
            {
                ItemEventAdd();
                SpawnPointInit();
                BackEndManager.Instance.Parsing.PlayerMoveEvent += Parsing_PlayerMove;
                Backend.Match.OnLeaveInGameServer += OnLeaveInGameServerEvent;
                BackEndManager.Instance.Parsing.SlimeSizeUpEvent += Parsing_SlimeSizeUpEvent;
                BackEndManager.Instance.Parsing.EndGameEvent += Parsing_EndGameEvent;

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
                _myClientNickName = TotalGameManager.Instance.MyClientNickName;
                bool isSuperPlayer = TotalGameManager.Instance.isHost;

                NamePlayerPairs = new Dictionary<string, LJH.Player_Logic>();
                InGameItemDic = new EventDictionary<int, GrowingItem>();
                GaugeDic = new Dictionary<string, GaugeElement>();
                ScoreDic = new Dictionary<string, float>();

                for (int i = 0; i < _playerNickNames.Count; i++)
                {
                    // Declare Varialbles
                    #region 
                    GameObject playerSetGO;
                    GameObject moverGO;
                    GameObject logicGO;
                    GameObject visualGO;

                    Player_Mover mover;
                    Player_Logic logic;
                    Player_Visual visual;
                    #endregion

                    // Instantiate and Set Variables
                    #region 
                    playerSetGO = Instantiate(_playerPrefab[i]);
                    moverGO = playerSetGO.transform.Find("Player_Mover").gameObject;
                    logicGO = playerSetGO.transform.Find("Player_Logic").gameObject;
                    visualGO = playerSetGO.transform.Find("Player_Visual").gameObject; // inCase, this might need it in the future..

                    mover = moverGO.GetComponent<Player_Mover>();
                    logic = logicGO.GetComponent<Player_Logic>();
                    visual = visualGO.GetComponent<Player_Visual>(); // inCase, this might need it in the future..
                    #endregion

                    // Set InGameManager
                    #region 
                    NamePlayerPairs.Add(_playerNickNames[i], logic);
                    logic.NickName = _playerNickNames[i];
                    visual.SetNickName(_playerNickNames[i]);
                    AddGauge(_playerNickNames[i], _gaugeColors[i]);
                    ScoreDic.Add(_playerNickNames[i], 0f);
                    #endregion

                    // Check SuperPlayer
                    #region 
                    if (isSuperPlayer == true)
                    {
                        //tPlayer.SetSuperPlayer(true);
                    }
                    else
                    {
                        //tPlayer.SetSuperPlayer(false);
                    }
                    #endregion

                    // Check player and myclientname is Same. And Do soemthing
                    #region
                    if (_playerNickNames[i].ToString() == _myClientNickName)
                    {

                        // Set Player Initialize Position
                        #region 
                        logic.MovingTarget = _playerPositions[i].position;
                        logicGO.transform.position = _playerPositions[i].position;
                        // Debug.Log(_playerPositions[i].position); 
                        #endregion

                        // Set Tag
                        logicGO.tag = "Player";

                        // Set Cinemachine
                        #region 
                        mover.SetFirstPos(_playerPositions[i].position);
                        CinemachineVirtualCamera cam = Instantiate(cameraPrefab, logicGO.transform).GetComponent<CinemachineVirtualCamera>();
                        cam.gameObject.GetComponent<CinemachineConfiner>().m_BoundingShape2D = GameObject.Find("Bound").GetComponent<Collider2D>();
                        cam.Follow = logicGO.transform;
                        cam.LookAt = logicGO.transform;
                        #endregion
                   

                    }
                    else
                    {
                        // Set Player Initialize Position
                        #region
                        logic.MovingTarget = _playerPositions[i].position;
                        logicGO.transform.position = _playerPositions[i].position;
                        #endregion

                        // Set Player Logic Layer
                        #region 
                        logicGO.layer = LayerMask.NameToLayer("Default");
                        #endregion

                        // Destory Mover
                        Destroy(moverGO);

                    } 
                    #endregion
                }
            }

            GameItemInit();
        }

        private void AddGauge(string nickname, Color color)
        {
            float initPercent = 100 / _playerNickNames.Count;
            GameObject obj = Instantiate(gaugePrefab, gaugeObj.transform);
            GaugeElement gaugeElement = obj.GetComponent<GaugeElement>();
            gaugeElement.Percent = initPercent;
            gaugeElement.GaugeColor = color;
            GaugeDic.Add(nickname, gaugeElement);
        }

        int testCnt = 0;
        private void Update()
        {
            #region GameEndingConditionCheck

            #endregion
            // If Someone want, Change the DeclareMatchEnd. But, Have to check IsInGameServerConnet() for checking the InGame is running.

            if (TotalGameManager.Instance.isHost || _isGameEnd == true)
            {
                if (slimeObj.transform.localScale.x >= slimeEndScale || _isGameEnd == true)
                {
                    float[] scoreArr = new float[4];

                    for (int i = 0; i < ScoreDic.Count; i++)
                    {
                        scoreArr[i] = ScoreDic[_playerNickNames[i]];
                    }

                    // Game End
                    TotalScoreMessage scoreMsg = new TotalScoreMessage(scoreArr);
                    BackEndManager.Instance.InGame.SendDataToInGame(scoreMsg);

                    Message endMsg = new Message(MsgType.EndGame);
                    BackEndManager.Instance.InGame.SendDataToInGame(endMsg);
                    DeclareMatchEnd();

                    _isGameEnd = false;
                }
            }

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
                UnSubscribeOnLeaveInGameServerEvent();
            }
        }

        void UnSubscribeOnLeaveInGameServerEvent()
        {
            BackEndManager.Instance.Parsing.PlayerMoveEvent -= Parsing_PlayerMove;
            Backend.Match.OnLeaveInGameServer -= OnLeaveInGameServerEvent;
            BackEndManager.Instance.Parsing.SlimeSizeUpEvent -= Parsing_SlimeSizeUpEvent;
            BackEndManager.Instance.Parsing.GrabItemEvent -= Parsing_GrabItemEvent;
            BackEndManager.Instance.Parsing.CreateItemEvent -= Parsing_CreateItemEvent;
            BackEndManager.Instance.Parsing.EndGameEvent -= Parsing_EndGameEvent;
        }

        void DeclareMatchEnd()
        {
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

        private void Parsing_PlayerMove(string nickName, Vector2 target)
        {
            if (_isGameEnd) return;
            NamePlayerPairs[nickName].MovingTarget = target;
        }

        private void Parsing_SlimeSizeUpEvent(string nickname, float addSize)
        {
            if (_isGameEnd) return;
            if (slimeObj != null)
            {
                float size = slimeObj.transform.localScale.x + (addSize / 500);
                Vector3 scale = new Vector3(size, size);
                slimeObj.transform.localScale = scale;
                slimeUIPrefab.SlimeSize = scale;
            }

            totalScore += addSize;
            ScoreDic[nickname] += addSize;

            for (int i = 0; i < ScoreDic.Count; i++)
            {
                string name = _playerNickNames[i];
                float percent = ScoreDic[name] / totalScore * 100f;
                GaugeDic[name].Percent = percent;
            }

            Debug.Log($"{nickname}이 먹이를 먹였다!");

            NamePlayerPairs[nickname].SetUserItem(null);
        }

        private void Parsing_EndGameEvent()
        {
            //TODO : 게임이 끝났으니 모든 수신, 동작 멈추게 하고, 사용자에게 표시해야함
        }
    }
}
