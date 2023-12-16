using BackEnd.Tcp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KSY.Protocol
{
    // ������Ʈ �� ������ ParsingManager switch�� ������Ʈ�� ���� �������
    public enum Type
    {
        PlayerMove = 0, //�÷��̾� Ű �Է�
    }

    public class Message
    {
        public Type type;

        public Message(Type type)
        {
            this.type = type;
        }
    }

    public class PlayerMoveMessage : Message
    {
        public SessionId playerSession;
        public float x;
        public float y;

        public PlayerMoveMessage(SessionId sessionId, Vector2 pos) : base(Type.PlayerMove)
        {
            playerSession = sessionId;
            this.x = pos.x;
            this.y = pos.y;
        }
    }
}

