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
                case Type.PlayerMove:
                    PlayerMoveMessage keyMessage = DataParser.ReadJsonData<PlayerMoveMessage>(args.BinaryUserData);
                    KeyMessageEvent(args.From.SessionId, keyMessage);
                    break;
            }
        }

        private void KeyMessageEvent(SessionId index, PlayerMoveMessage keyMessage)
        {
            // TODO : ȣ��Ʈ�� ��������, ȣ��Ʈ �ƴѾֵ鸸 �������� ����غ���
            if (!BackEndMatchManager.GetInstance().IsHost())
            {
                //ȣ��Ʈ�� ����
                return;
            }
        }

    } 
}
