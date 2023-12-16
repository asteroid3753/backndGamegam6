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
        public event Action<int, float> SlimeSizeUpEvent;
        public event Action<string, int> GrabItemEvent;
        public event Action<int, int, Vector2> CreateItemEvent;

        public void Init()
        {

        }

        public void OnRecieve(MatchRelayEventArgs args)
        {
            if (args.BinaryUserData == null)
            {
                Debug.LogWarning(string.Format("�� �����Ͱ� ��ε�ĳ���� �Ǿ����ϴ�.\n{0} - {1}", args.From, args.ErrInfo));
                // �����Ͱ� ������ �׳� ����
                return;
            }

            Message msg = DataParser.ReadJsonData<Message>(args.BinaryUserData);
            if (msg == null)
            {
                Debug.Log("������ �޼��� ����");
                return;
            }

            //args.From.NickName

            // TODO : Player ���� �迭 �ʿ�, ���� �÷��̾�� �� �÷��̾� �� ������ �ʿ�
            //if (players == null)
            //{
            //    Debug.LogError("Players ������ �������� �ʽ��ϴ�.");
            //    return;
            //}

            switch (msg.type)
            {
                case MsgType.PlayerMove:
                    PlayerMoveMessage moveMsg = DataParser.ReadJsonData<PlayerMoveMessage>(args.BinaryUserData);
                    PlayerMoveMsgEvent(args.From.NickName, moveMsg);
                    break;
                case MsgType.SlimeSizeUp:
                    SlimeSizeUpMessage sizeUpMsg = DataParser.ReadJsonData<SlimeSizeUpMessage>(args.BinaryUserData);
                    SlimeSizeUpMsgEvent(sizeUpMsg);
                    break;
                case MsgType.GrabItem:
                    GrabItemMessage grabItemMsg = DataParser.ReadJsonData<GrabItemMessage>(args.BinaryUserData);
                    GrabItemMsgEvent(args.From.NickName, grabItemMsg);
                    break;
                case MsgType.CreateItem:
                    CreateItemMessage createItemMsg = DataParser.ReadJsonData<CreateItemMessage>(args.BinaryUserData);
                    CreateItemMsgEvent(createItemMsg);
                    break;
            }
        }

        private void PlayerMoveMsgEvent(string nickname, PlayerMoveMessage data)
        {
            Vector2 moveVector = new Vector2(data.x, data.y);

            // if Now Vector diffrent
            if (!moveVector.Equals(InGameManager.Instance.NamePlayerPairs[nickname].GetUserTarget()))
            {
                PlayerMoveEvent?.Invoke(nickname, moveVector);
            }
        }

        private void SlimeSizeUpMsgEvent(SlimeSizeUpMessage data)
        {
            SlimeSizeUpEvent?.Invoke(data.id, data.addSize);
        }

        private void GrabItemMsgEvent(string nikname, GrabItemMessage data)
        {
            GrabItemEvent?.Invoke(nikname, data.itemCode);
        }

        private void CreateItemMsgEvent(CreateItemMessage data)
        {
            CreateItemEvent?.Invoke(data.itemType, data.itemCode, new Vector2(data.x, data.y));
        }
    } 
}
