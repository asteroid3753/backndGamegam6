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

        void Start()
        {
            player = GetComponent< LJH.Player>();
            player.SetUserSpeed(20f);
        }

        private void FixedUpdate() {
            //UserMove
            this.gameObject.transform.Find("Body").GetComponent<SpriteRenderer>().flipX = player.GetUserFlip();
            if(player.GetUserItem() != null)
                this.gameObject.transform.Find("Item").GetComponent<SpriteRenderer>().sprite = player.GetUserItem().ItemImg;
            else
                this.gameObject.transform.Find("Item").GetComponent<SpriteRenderer>().sprite = null;
        }

        private void PlayerGrapRecvFunc(string nickname, int itemCode){
            //아이템을 잡았을 때 아이템 띄우기
            print("send item");
           
        }
    }

}
