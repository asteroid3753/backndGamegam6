using BackEnd.Tcp;
using KSY.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KSY
{
    public class ParsingManager
    {
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
                    KeyMessageEvent(args.From.SessionId, keyMessage);
                    break;
            }
        }

        private void KeyMessageEvent(SessionId index, PlayerMoveMessage keyMessage)
        {
            // TODO : 호스트만 수행할지, 호스트 아닌애들만 수행할지 고민해보기
            if (!BackEndMatchManager.GetInstance().IsHost())
            {
                //호스트만 수행
                return;
            }
        }

    } 
}
