using BackEnd.Tcp;
using KSY;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KSY
{
    public class RecvTest : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            // recv �̺�Ʈ ���
            BackEndManager.Instance.Parsing.PlayerMoveEvent += PlayerMoveRecvFunc;
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void PlayerMoveRecvFunc(SessionId sessionId, Vector2 vec)
        {
            // ������ 
            Debug.Log($"x : {vec.x}, y : {vec.y}");
        }
    } 
}
