using BackEnd.Tcp;
using KSY.Protocol;
using LJH;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace KSY
{
    public class ParsingManager
    {
        #region 
        #endregion
        public event Action<string, Vector2> PlayerMoveEvent;
public event Action<string, float> SlimeSizeUpEvent;
        public event Action<string, int> GrabItemEvent;
        public event Action<int, int, int> CreateItemEvent;
        public event Action<float[]> TotalScoreEvent;

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

            if (InGameManager.Instance.NamePlayerPairs == null)
            {
               Debug.LogError("Players 정보가 존재하지 않습니다.");
               return;
            }

            switch (msg.type)
            {
                case MsgType.PlayerMove:
                    PlayerMoveMessage moveMsg = DataParser.ReadJsonData<PlayerMoveMessage>(args.BinaryUserData);
                    PlayerMoveMsgEvent(args.From.NickName, moveMsg);
                    break;
                case MsgType.SlimeSizeUp:
                    SlimeSizeUpMessage sizeUpMsg = DataParser.ReadJsonData<SlimeSizeUpMessage>(args.BinaryUserData);
                    SlimeSizeUpMsgEvent(args.From.NickName, sizeUpMsg);
                    break;
                case MsgType.GrabItem:
                    GrabItemMessage grabItemMsg = DataParser.ReadJsonData<GrabItemMessage>(args.BinaryUserData);
                    GrabItemMsgEvent(args.From.NickName, grabItemMsg);
                    break;
                case MsgType.CreateItem:
                    CreateItemMessage createItemMsg = DataParser.ReadJsonData<CreateItemMessage>(args.BinaryUserData);
                    CreateItemMsgEvent(createItemMsg);
                    break;
                case MsgType.TotalScore:
                    TotalScoreMessage scoreMsg = DataParser.ReadJsonData<TotalScoreMessage>(args.BinaryUserData);
                    TotalScoreMsgEvent(scoreMsg);
                    break;
            }
        }

        private void PlayerMoveMsgEvent(string nickname, PlayerMoveMessage data)
        {
            Vector2 moveVector = new Vector2(data.x, data.y);

            // 타겟 백터랑 일치하는지 확인 동일하면 이벤트 튀길 필요가 없음
            if (!moveVector.Equals(InGameManager.Instance.NamePlayerPairs[nickname].MovingTarget))
            {
                PlayerMoveEvent?.Invoke(nickname, moveVector);
            }
        }

        private void SlimeSizeUpMsgEvent(string nickname, SlimeSizeUpMessage data)
        {
            SlimeSizeUpEvent?.Invoke(nickname, data.addSize);
        }

        private void GrabItemMsgEvent(string nickname, GrabItemMessage data)
        {
            GrabItemEvent?.Invoke(nickname, data.itemCode);
        }

        private void CreateItemMsgEvent(CreateItemMessage data)
        {
            CreateItemEvent?.Invoke(data.itemType, data.itemCode, data.index);
        }

        private void TotalScoreMsgEvent(TotalScoreMessage data)
        {
            TotalScoreEvent?.Invoke((float[])data.scoreArr.Clone());
        }
    } 
}
