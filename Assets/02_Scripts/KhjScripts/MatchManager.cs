using BackEnd.Tcp;
using BackEnd;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace khj
{
    public class MatchManager : MonoBehaviour
    {
        public static MatchManager Instance;
        InGameStart ingame;
        List<MatchCard> matchCardList = new List<MatchCard>();
        int index;
        
        void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        void Start()
        {
            ingame = GetComponent<InGameStart>();
            JoinMatchMakingServer();
        }

        void JoinMatchMakingServer()
        {
            Backend.Match.OnJoinMatchMakingServer = (JoinChannelEventArgs args) => {
                if (args.ErrInfo == ErrorInfo.Success)
                {
                    CreateMatchRoom();
                    GetMatchList();
                }
            };

            Join();
        }

        public void Join() {
            ErrorInfo errorInfo;
            if (Backend.Match.JoinMatchMakingServer(out errorInfo))
            {
                Debug.Log("1-1. JoinMatchMakingServer ��û : " + errorInfo.ToString());
            }
            else
            {
                Debug.LogError("1-1. JoinMatchMakingServer ���� : " + errorInfo.ToString());
            }
        }

        public void GetMatchList()
        {
            matchCardList.Clear();

            Backend.Match.GetMatchList(callback => {
                if (!callback.IsSuccess())
                {
                    Debug.LogError("Backend.Match.GetMatchList Error : " + callback);
                    return;
                }

                JsonData matchCardListJson = callback.FlattenRows();

                Debug.Log("Backend.Match.GetMatchList : " + callback);

                for (int i = 0; i < matchCardListJson.Count; i++)
                {
                    MatchCard matchCard = new MatchCard();

                    matchCard.inDate = matchCardListJson[i]["inDate"].ToString();

                    matchCard.result_processing_type = matchCardListJson[i]["result_processing_type"].ToString();

                    matchCard.version = Int32.Parse(matchCardListJson[i]["version"].ToString());

                    matchCard.matchTitle = matchCardListJson[i]["matchTitle"].ToString();

                    matchCard.enable_sandbox = matchCardListJson[i]["enable_sandbox"].ToString() == "true" ? true : false;

                    string matchType = matchCardListJson[i]["matchType"].ToString();
                    string matchModeType = matchCardListJson[i]["matchModeType"].ToString();

                    switch (matchType)
                    {
                        case "random":
                            matchCard.matchType = MatchType.Random;
                            break;

                        case "point":
                            matchCard.matchType = MatchType.Point;
                            break;

                        case "mmr":
                            matchCard.matchType = MatchType.MMR;
                            break;
                    }

                    switch (matchModeType)
                    {
                        case "OneOnOne":
                            matchCard.matchModeType = MatchModeType.OneOnOne;
                            break;

                        case "TeamOnTeam":
                            matchCard.matchModeType = MatchModeType.TeamOnTeam;

                            break;

                        case "Melee":
                            matchCard.matchModeType = MatchModeType.Melee;
                            break;
                    }

                    matchCard.matchHeadCount = Int32.Parse(matchCardListJson[i]["matchHeadCount"].ToString());

                    matchCard.enable_battle_royale = matchCardListJson[i]["enable_battle_royale"].ToString() == "true" ? true : false;

                    matchCard.match_timeout_m = Int32.Parse(matchCardListJson[i]["match_timeout_m"].ToString());

                    matchCard.transit_to_sandbox_timeout_ms = Int32.Parse(matchCardListJson[i]["transit_to_sandbox_timeout_ms"].ToString());

                    matchCard.match_start_waiting_time_s = Int32.Parse(matchCardListJson[i]["match_start_waiting_time_s"].ToString());

                    if (matchCardListJson[i].ContainsKey("match_increment_time_s"))
                    {
                        matchCard.match_increment_time_s = Int32.Parse(matchCardListJson[i]["match_increment_time_s"].ToString());
                    }

                    if (matchCardListJson[i].ContainsKey("maxMatchRange"))
                    {
                        matchCard.maxMatchRange = Int32.Parse(matchCardListJson[i]["maxMatchRange"].ToString());
                    }

                    if (matchCardListJson[i].ContainsKey("increaseAndDecrease"))
                    {
                        matchCard.increaseAndDecrease = Int32.Parse(matchCardListJson[i]["increaseAndDecrease"].ToString());
                    }

                    if (matchCardListJson[i].ContainsKey("initializeCycle"))
                    {
                        matchCard.initializeCycle = matchCardListJson[i]["initializeCycle"].ToString();
                    }

                    if (matchCardListJson[i].ContainsKey("defaultPoint"))
                    {
                        matchCard.defaultPoint = Int32.Parse(matchCardListJson[i]["defaultPoint"].ToString());
                    }

                    if (matchCardListJson[i].ContainsKey("savingPoint"))
                    {
                        if (matchCardListJson[i]["savingPoint"].IsArray)
                        {
                            for (int listNum = 0; listNum < matchCardListJson[i]["savingPoint"].Count; listNum++)
                            {
                                var keyList = matchCardListJson[i]["savingPoint"][listNum].Keys;
                                foreach (var key in keyList)
                                {
                                    matchCard.savingPoint.Add(key, Int32.Parse(matchCardListJson[i]["savingPoint"][listNum][key].ToString()));
                                }
                            }
                        }
                        else
                        {
                            foreach (var key in matchCardListJson[i]["savingPoint"].Keys)
                            {
                                matchCard.savingPoint.Add(key, Int32.Parse(matchCardListJson[i]["savingPoint"][key].ToString()));
                            }
                        }
                    }

                    matchCardList.Add(matchCard);
                }

                RequestMatchMaking();
                //for (int i = 0; i < matchCardList.Count; i++)
                //{
                //    Debug.Log($"{i} 번째 매치카드 : \n" + matchCardList[i].ToString());
                //}
            });
        }
        public void RequestMatchMaking()
        {
            Backend.Match.OnMatchMakingResponse = (MatchMakingResponseEventArgs args) => {
                if (args.ErrInfo == ErrorCode.Match_InProgress)
                {
                    int second = matchCardList[index].transit_to_sandbox_timeout_ms / 1000;

                    //if (second > 0)
                    //{
                    //    Debug.Log($"{second}�� �ڿ� ����ڽ� Ȱ��ȭ�� �˴ϴ�.");
                    //    StartCoroutine(WaitFor10Seconds(second));
                    //}
                }
                else if (args.ErrInfo == ErrorCode.Success)
                {
                    Debug.Log("3-3. OnMatchMakingResponse ��Ī ���� �Ϸ�");

                    ingame.JoinGameServer(args.RoomInfo);
                }
                else
                {
                    Debug.LogError("3-2. OnMatchMakingResponse ��Ī ��û ������ ���� �߻� : " + args.ToString());
                }
            };

            Debug.Log("3-1. RequestMatchMaking ��Ī ��û ����");

            Backend.Match.RequestMatchMaking(matchCardList[index].matchType, matchCardList[index].matchModeType, matchCardList[index].inDate);
        }
        IEnumerator WaitFor10Seconds(int second)
        {
            var delay = new WaitForSeconds(1.0f);
            for (int i = 0; i < second; i++)
            {
                Debug.Log($"{i}�� ���");
                yield return delay;
            }
            TotalGameManager.Instance.ChangeState(TotalGameManager.GameState.Ready);
        }
        public void CreateMatchRoom()
        {
            Backend.Match.OnMatchMakingRoomCreate = (MatchMakingInteractionEventArgs args) => {
                if (args.ErrInfo == ErrorCode.Success)
                {
                    Debug.Log("2-2. OnMatchMakingRoomCreate ����");
                    Backend.Match.RequestMatchMaking(matchCardList[index].matchType, matchCardList[index].matchModeType, matchCardList[index].inDate);
                }
                else
                {
                    Debug.LogError("2-2. OnMatchMakingRoomCreate ����");
                }
            };

            Debug.Log("2-1. CreateMatchRoom ��û");
            Backend.Match.CreateMatchRoom();
        }

        void Update()
        {
            if (Backend.IsInitialized)
            {
                Backend.Match.Poll();
            }
        }
    }
    public class MatchCard
    {
        public string inDate;
        public string matchTitle;
        public bool enable_sandbox;
        public MatchType matchType;
        public MatchModeType matchModeType;
        public int matchHeadCount;
        public bool enable_battle_royale;
        public int match_timeout_m;
        public int transit_to_sandbox_timeout_ms;
        public int match_start_waiting_time_s;
        public int match_increment_time_s;
        public int maxMatchRange;
        public int increaseAndDecrease;
        public string initializeCycle;
        public int defaultPoint;
        public int version;
        public string result_processing_type;
        public Dictionary<string, int> savingPoint = new Dictionary<string, int>(); // ����/�������� ���� Ű���� �޶��� �� ����.

        public override string ToString()
        {
            string savingPointString = "savingPont : \n";
            foreach (var dic in savingPoint)
            {
                savingPointString += $"{dic.Key} : {dic.Value}\n";
            }

            savingPointString += "\n";
            return $"inDate : {inDate}\n" +
                   $"matchTitle : {matchTitle}\n" +
                   $"enable_sandbox : {enable_sandbox}\n" +
                   $"matchType : {matchType}\n" +
                   $"matchModeType : {matchModeType}\n" +
                   $"matchHeadCount : {matchHeadCount}\n" +
                   $"enable_battle_royale : {enable_battle_royale}\n" +
                   $"match_timeout_m : {match_timeout_m}\n" +
                   $"transit_to_sandbox_timeout_ms : {transit_to_sandbox_timeout_ms}\n" +
                   $"match_start_waiting_time_s : {match_start_waiting_time_s}\n" +
                   $"match_increment_time_s : {match_increment_time_s}\n" +
                   $"maxMatchRange : {maxMatchRange}\n" +
                   $"increaseAndDecrease : {increaseAndDecrease}\n" +
                   $"initializeCycle : {initializeCycle}\n" +
                   $"defaultPoint : {defaultPoint}\n" +
                   $"version : {version}\n" +
                   $"result_processing_type : {result_processing_type}\n" +
                   savingPointString;
        }
    }
}
