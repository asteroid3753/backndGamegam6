using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using BackEnd;
using BackEnd.Tcp;
using KSY;


namespace LJH{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] LJH.Player player;
        
        LJH.InputManager inputManager;
    
        //server
        string serverAddress;
        ushort serverPort;
        
        // Start is called before the first frame update
        void Start()
        {
            //서버연결이 완료되면 서버에서 현재 플레이어의 아이디와 이름을 가져온 후 초기화.
           
            BackEndManager.Instance.Parsing.PlayerMoveEvent += PlayerMoveRecvFunc;
        }

        // Update is called once per frame
        private void FixedUpdate() {
            //UserMove

        }

        private void PlayerMoveRecvFunc(SessionId sessionId, Vector2 vec)
        {
            // ������ 
            player.SetUserTarget(vec);
        }

    }

}
