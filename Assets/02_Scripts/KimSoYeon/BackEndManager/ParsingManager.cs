using BackEnd.Tcp;
using KSY.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace KSY
{
    public class ParsingManager
    {
        public UnityEvent<SessionId, Vector2> PlayerMoveEvent;

        public void Init()
        {

        }

        public void OnRecieve(MatchRelayEventArgs args)
        {
            if (args.BinaryUserData == null)
            {
                Debug.LogWarning(string.Format("빈 데이터가 브로드캐스팅 되었습니다.\n{0} - {1}", args.From, args.ErrInfo));
                // 데이터가 없으면 그냥 리턴
                return;
            }

            Message msg = DataParser.ReadJsonData<Message>(args.BinaryUserData);
            if (msg == null)
            {
                Debug.Log("데이터 메세지 없음");
                return;
            }

            // TODO : 호스트 정보 필요, 보낸이가 HOST 인지 체크 이거는 뭘 체크하는건지 체크해봐야함
            //if (BackEndMatchManager.GetInstance().IsHost() != true && args.From.SessionId == myPlayerIndex)
            //{
            //    return;
            //}

            // TODO : Player 정보 배열 필요, 현재 플레이어와 내 플레이어 가 누군지 필요
            //if (players == null)
            //{
            //    Debug.LogError("Players 정보가 존재하지 않습니다.");
            //    return;
            //}

            switch (msg.type)
            {
                case Type.PlayerMove:
                    PlayerMoveMessage keyMessage = DataParser.ReadJsonData<PlayerMoveMessage>(args.BinaryUserData);
                    PlayerMoveMsgEvent(keyMessage);
                    break;
            }
        }

        /// <summary>
        /// PlayerMove 메세지 가공 후 이벤트 호출 함수
        /// </summary>
        /// <param name="data"></param>
        private void PlayerMoveMsgEvent(PlayerMoveMessage data)
        {
            Vector2 moveVector = new Vector2(data.x, data.y);

            //// 타겟 백터랑 일치하는지 확인 동일하면 이벤트 튀길 필요가 없음
            //if (!moveVector.Equals(BackEndManager.Instance.players[data.playerSession].moveVector))
            //{
            //    //이벤트 튀기기
            //}

            // 이벤트
            PlayerMoveEvent?.Invoke(data.playerSession, moveVector);
        }

    } 
}
