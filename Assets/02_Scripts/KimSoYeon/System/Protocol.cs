using BackEnd.Tcp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KSY.Protocol
{
    // 업데이트 할 때마다 ParsingManager switch문 업데이트도 같이 해줘야함
    public enum MsgType
    {
        PlayerMove = 0, //플레이어 키 입력
        SlimeSizeUp,
        GrabItem,
        CreateItem
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

    public class GrabItemMessage : Message
    {
        public int itemCode;

        public GrabItemMessage(int itemCode) : base(MsgType.GrabItem)
        {
            this.itemCode = itemCode;
        }
    }

    public class CreateItemMessage : Message
    {
        public int itemType;
        public int itemCode;
        public float x;
        public float y;

        public CreateItemMessage(int itemType, int itemCode, Vector2 pos) : base(MsgType.CreateItem)
        {
            this.itemType = itemType;
            this.itemCode = itemCode;
            this.x = pos.x;
            this.y = pos.y;
        }
    }
}

