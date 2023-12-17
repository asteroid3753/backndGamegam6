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
                Debug.Log("OnLeaveMatchMakingServer - ��Ī ���� ���� ���� : " + args.ToString());
            };

            Debug.Log($"5-a. LeaveMatchMakingServer ��ġ����ŷ ���� ���� ���� ��û");

            Backend.Match.LeaveMatchMakingServer();
        }
        public void JoinGameServer(MatchInGameRoomInfo gameRoomInfo)
        {
            Backend.Match.OnSessionJoinInServer = (JoinChannelEventArgs args) => {
                if (args.ErrInfo == ErrorInfo.Success)
                {
                    JoinGameRoom();
                }
                else
                {
                    Debug.LogError("4-2. OnSessionJoinInServer ���� ���� ���� ���� : " + args.ToString());
                }

                // ���� ������ ���������� ���������� ��Ī ������ ����
                LeaveMatchMaking();
            };

            currentGameRoomInfo = gameRoomInfo;
            ErrorInfo errorInfo = null;

            if (!Backend.Match.JoinGameServer(currentGameRoomInfo.m_inGameServerEndPoint.m_address, currentGameRoomInfo.m_inGameServerEndPoint.m_port, false, out errorInfo))
            {
                Debug.LogError("JoinGameServer �� ���� ������ �߻��߽��ϴ�." + errorInfo);
                return;
            }
        }
        public void JoinGameRoom()
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
                else
                {
                    Debug.LogError("5-2. OnSessionListInServer : " + args.ToString());
                }
            };

            Backend.Match.OnMatchInGameAccess = (MatchInGameSessionEventArgs args) => {
                if (args.ErrInfo == ErrorCode.Success)
                {
                    Debug.Log($"5-3. OnMatchInGameAccess - ������ �����߽��ϴ� : {args.GameRecord.m_nickname}({args.GameRecord.m_sessionId})");
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
                string userListString = "������ ���� : \n";
                foreach (var list in inGameUserList)
                {
                    if (list.Value.m_isSuperGamer == true)
                    {
                        TotalGameManager.Instance.isHost = (list.Value.m_nickname == TotalGameManager.Instance.myNickName);
                        TotalGameManager.Instance.host = list.Value.m_nickname;
                        userListString += "���۰��̸�";
                    }
                }
                TotalGameManager.Instance.playerNickNames = inGameUserList.Keys.ToList<string>();
                TotalGameManager.Instance.playerNickNames.Sort();

                Debug.Log("6-1. OnMatchInGameStart �ΰ��� ����");
                Debug.Log(userListString);
            };

            Backend.Match.JoinGameRoom(currentGameRoomInfo.m_inGameRoomToken);
            Invoke("ChangeState", 3f);
        }

        void ChangeState()
        {
            TotalGameManager.Instance.ChangeState(TotalGameManager.GameState.Start);
        }
    }
}
