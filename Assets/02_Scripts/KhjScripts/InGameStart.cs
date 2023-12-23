using BackEnd.Tcp;
using BackEnd;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace khj
{
    public class InGameStart : MonoBehaviour
    {
        MatchInGameRoomInfo currentGameRoomInfo;
        Dictionary<string, MatchUserGameRecord> inGameUserList = new Dictionary<string, MatchUserGameRecord>();

        private void Start()
        {
            Backend.Match.OnSessionListInServer = (MatchInGameSessionListEventArgs args) => {
                if (args.ErrInfo == ErrorCode.Success)
                {
                    foreach (var list in args.GameRecords)
                    {
                        if (inGameUserList.ContainsKey(list.m_nickname))
                        {
                            continue;
                        }

                        inGameUserList.Add(list.m_nickname, list);
                    }
                }
            };
            Backend.Match.OnMatchInGameAccess = (MatchInGameSessionEventArgs args) => {
                if (args.ErrInfo == ErrorCode.Success)
                {
                    if (!inGameUserList.ContainsKey(args.GameRecord.m_nickname))
                    {
                        inGameUserList.Add(args.GameRecord.m_nickname, args.GameRecord);
                    }
                }
            };
            Backend.Match.OnMatchInGameStart = () => {
                TotalGameManager.Instance.playerNickNames = inGameUserList.Keys.ToList<string>();
                foreach (var list in inGameUserList)
                {
                    if (list.Value.m_isSuperGamer == true)
                        TotalGameManager.Instance.host = list.Value.m_nickname;
                }
            };
        }
        private void LeaveMatchMaking()
        {
            Backend.Match.OnLeaveMatchMakingServer = (LeaveChannelEventArgs args) => {
                Debug.Log("OnLeaveMatchMakingServer - 매칭 서버 접속 종료 : " + args.ToString());
            };

            Debug.Log($"5-a. LeaveMatchMakingServer 매치메이킹 서버 접속 종료 요청");

            Backend.Match.LeaveMatchMakingServer();
        }
        public void JoinGameServer(MatchInGameRoomInfo gameRoomInfo)
        {
            Backend.Match.OnSessionJoinInServer = (JoinChannelEventArgs args) => {
                if (args.ErrInfo == ErrorInfo.Success)
                {
                    Debug.Log("4-2. OnSessionJoinInServer 게임 서버 접속 성공 : " + args.ToString());
                    JoinGameRoom();
                }
                else
                {
                    Debug.LogError("4-2. OnSessionJoinInServer 게임 서버 접속 실패 : " + args.ToString());
                }

                // 게임 서버에 정상적으로 접속했으면 매칭 서버를 종료
                LeaveMatchMaking();
            };
            Debug.Log("4-1. JoinGameServer 인게임 서버 접속 요청");

            currentGameRoomInfo = gameRoomInfo;
            ErrorInfo errorInfo = null;

            if (!Backend.Match.JoinGameServer(currentGameRoomInfo.m_inGameServerEndPoint.m_address, currentGameRoomInfo.m_inGameServerEndPoint.m_port, false, out errorInfo))
            {
                Debug.LogError("JoinGameServer 중 로컬 에러가 발생했습니다." + errorInfo);
                return;
            }
        }
        public void JoinGameRoom()
        {
            Backend.Match.OnSessionListInServer = (MatchInGameSessionListEventArgs args) => {
                if (args.ErrInfo == ErrorCode.Success)
                {
                    Debug.Log("5-2. OnSessionListInServer 게임룸 접속 성공 : " + args.ToString());
                    foreach (var list in args.GameRecords)
                    {
                        if (inGameUserList.ContainsKey(list.m_nickname))
                        {
                            continue;
                        }

                        inGameUserList.Add(list.m_nickname, list);
                    }
                }
                else
                {
                    Debug.LogError("5-2. OnSessionListInServer : " + args.ToString());
                }
            };

            Backend.Match.OnMatchInGameAccess = (MatchInGameSessionEventArgs args) => {
                if (args.ErrInfo == ErrorCode.Success)
                {
                    Debug.Log($"5-3. OnMatchInGameAccess - 유저가 접속했습니다 : {args.GameRecord.m_nickname}({args.GameRecord.m_sessionId})");
                    if (!inGameUserList.ContainsKey(args.GameRecord.m_nickname))
                    {
                        inGameUserList.Add(args.GameRecord.m_nickname, args.GameRecord);
                    }
                }
                else
                {
                    Debug.LogError("5-3. OnMatchInGameAccess : " + args.ErrInfo.ToString());
                }
            };

            Backend.Match.OnMatchInGameStart = () => {
                string userListString = "접속한 유저 : \n";
                foreach (var list in inGameUserList)
                {
                    if (list.Value.m_isSuperGamer == true)
                    {
                        TotalGameManager.Instance.isHost = (list.Value.m_nickname == TotalGameManager.Instance.MyClientNickName);
                        TotalGameManager.Instance.host = list.Value.m_nickname;
                        userListString += $"{list.Value.m_nickname}({list.Value.m_sessionId})" + (list.Value.m_isSuperGamer == true ? "슈퍼게이머" : "");
                    }
                }
                TotalGameManager.Instance.playerNickNames = inGameUserList.Keys.ToList<string>();
                TotalGameManager.Instance.playerNickNames.Sort();
                TotalGameManager.Instance.resultSlimeSize = 0;
                TotalGameManager.Instance.playerResultSocres.Clear();

                Debug.Log("6-1. OnMatchInGameStart 인게임 시작");
                Debug.Log(userListString);
            };

            Debug.Log($"5-1. JoinGameRoom 게임룸 접속 요청 : 토큰({currentGameRoomInfo.m_inGameRoomToken}");
            Backend.Match.JoinGameRoom(currentGameRoomInfo.m_inGameRoomToken);
            Invoke("ChangeState", 3f);
        }

        void ChangeState()
        {
            TotalGameManager.Instance.ChangeState(TotalGameManager.GameState.Start);
        }
    }
}
