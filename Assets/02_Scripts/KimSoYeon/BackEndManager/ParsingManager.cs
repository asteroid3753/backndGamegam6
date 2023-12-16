using BackEnd.Tcp;
using KSY.Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace KSY
{
    public class ParsingManager
    {
        public event Action<SessionId, Vector2> PlayerMoveEvent;
        public event Action<SessionId, float> SlimeSizeUpEvent;

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

            // TODO : ȣ��Ʈ ���� �ʿ�, �����̰� HOST ���� üũ �̰Ŵ� �� üũ�ϴ°��� üũ�غ�����
            //if (BackEndMatchManager.GetInstance().IsHost() != true && args.From.SessionId == myPlayerIndex)
            //{
            //    return;
            //}

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
                    PlayerMoveMsgEvent(moveMsg);
                    break;
                case MsgType.SlimeSizeUp:
                    SlimeSizeUpMessage sizeUpMsg = DataParser.ReadJsonData<SlimeSizeUpMessage>(args.BinaryUserData);
                    SlimeSizeUpMsgEvent(sizeUpMsg);
                    break;
          
            }
        }

        /// <summary>
        /// PlayerMove �޼��� ���� �� �̺�Ʈ ȣ�� �Լ�
        /// </summary>
        /// <param name="data"></param>
        private void PlayerMoveMsgEvent(PlayerMoveMessage data)
        {
            Vector2 moveVector = new Vector2(data.x, data.y);

            //// Ÿ�� ���Ͷ� ��ġ�ϴ��� Ȯ�� �����ϸ� �̺�Ʈ Ƣ�� �ʿ䰡 ����
            //if (!moveVector.Equals(BackEndManager.Instance.players[data.playerSession].moveVector))
            //{
            //    //�̺�Ʈ Ƣ���
            //}

            // �̺�Ʈ
            PlayerMoveEvent?.Invoke(data.playerSession, moveVector);
        }

        private void SlimeSizeUpMsgEvent(SlimeSizeUpMessage data)
        {
            SlimeSizeUpEvent?.Invoke(data.playerSession, data.addSize);
        }

    } 
}
