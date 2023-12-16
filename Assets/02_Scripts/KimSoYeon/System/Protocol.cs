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
        public float x;
        public float y;

        public PlayerMoveMessage(Vector2 pos) : base(MsgType.PlayerMove)
        {
            this.x = pos.x;
            this.y = pos.y;
        }
    }

    public class SlimeSizeUpMessage : Message
    {
        public int id;
        public float addSize;

        public SlimeSizeUpMessage(int id, float addSize) : base(MsgType.SlimeSizeUp)
        {
            this.id = id;
            this.addSize = addSize;
        }
    }
}

