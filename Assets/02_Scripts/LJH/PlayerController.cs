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


        // Start is called before the first frame update
        void Start()
        {
            //서버연결이 완료되면 서버에서 현재 플레이어의 아이디와 이름을 가져온 후 초기화.
            player.SetUserSpeed(20f);
            BackEndManager.Instance.Parsing.PlayerMoveEvent += PlayerMoveRecvFunc;
            BackEndManager.Instance.Parsing.GrabItemEvent += PlayerGrapRecvFunc;
        }

        // Update is called once per frame
        private void FixedUpdate() {
            //UserMove
            this.gameObject.transform.Find("Body").GetComponent<SpriteRenderer>().flipX = player.GetUserFlip();
            if(player.GetUserItem() != null)
                this.gameObject.transform.Find("Item").GetComponent<SpriteRenderer>().sprite = player.GetUserItem().ItemImg;
            else
                this.gameObject.transform.Find("Item").GetComponent<SpriteRenderer>().sprite = null;
        }

        private void PlayerMoveRecvFunc(string nickname, Vector2 vec)
        {
            // ������ 
            player.SetUserTarget(vec);
        }

        private void PlayerGrapRecvFunc(string nickname, int itemCode){
            //아이템을 잡았을 때 아이템 띄우기
            print("send item");
            player.SetUserItem(player.GetUserNowItem());
        }


    }

}
