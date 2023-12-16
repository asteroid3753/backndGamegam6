using BackEnd.Tcp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KSY.Protocol
{
    // 업데이트 할 때마다 ParsingManager switch문 업데이트도 같이 해줘야함
    public enum Type
    {
        PlayerMove = 0, //플레이어 키 입력
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

