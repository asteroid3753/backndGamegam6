using BackEnd.Tcp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KSY.Protocol
{
    // ������Ʈ �� ������ ParsingManager switch�� ������Ʈ�� ���� �������
    public enum MsgType
    {
        PlayerMove = 0, //�÷��̾� Ű �Է�
        SlimeSizeUp
    }

    public class Message
    {
        public MsgType type;

        public Message(MsgType type)
        {
            this.type = type;
        }
    }

    public class PlayerMoveMessage : Message
    {
        public SessionId playerSession;
        public float x;
        public float y;

        public PlayerMoveMessage(SessionId sessionId, Vector2 pos) : base(MsgType.PlayerMove)
        {
            playerSession = sessionId;
            this.x = pos.x;
            this.y = pos.y;
        }
    }

    public class SlimeSizeUpMessage : Message
    {
        public SessionId playerSession;
        public float addSize;

        public SlimeSizeUpMessage(SessionId sessionId, float addSize) : base(MsgType.SlimeSizeUp)
        {
            playerSession = sessionId;
            this.addSize = addSize;
        }
    }
}

