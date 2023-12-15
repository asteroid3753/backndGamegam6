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
        public int keyData;
        public float x;
        public float y;
        public float z;

        public PlayerMoveMessage(int data, Vector3 pos) : base(Type.PlayerMove)
        {
            this.keyData = data;
            this.x = pos.x;
            this.y = pos.y;
            this.z = pos.z;
        }
    }
}

