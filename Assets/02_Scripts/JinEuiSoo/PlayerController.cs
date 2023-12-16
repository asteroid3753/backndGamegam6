using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using BackEnd;
using BackEnd.Tcp;
using KSY;


namespace JES
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] JES.TestPlayer player;
        
        JES.InputManager inputManager;

        //item test
        //[SerializeField] GrowingItem nowItem;
        [SerializeField] GameObject itemObj;


        // Start is called before the first frame update
        void Start()
        {
            //서버연결이 완료되면 서버에서 현재 플레이어의 아이디와 이름을 가져온 후 초기화.
            //player.SetUserSpeed(20f);
            BackEndManager.Instance.Parsing.PlayerMoveEvent += PlayerMoveRecvFunc;
        }

        // Update is called once per frame
        //private void FixedUpdate() {
        //    //UserMove
        //    this.gameObject.transform.Find("Body").GetComponent<SpriteRenderer>().flipX = player.GetUserFlip();
        //    if(nowItem != null && Input.GetKeyDown(KeyCode.Space)){
        //        print("item");
        //        player.SetUserItem(nowItem);
        //        itemObj.GetComponent<SpriteRenderer>().sprite = player.GetUserItem().GetItemImg();
                
        //    }
        //    if(nowItem == null && Input.GetKeyDown(KeyCode.Space)){
        //        print("item drop");
        //        player.SetUserItem(null);
        //        itemObj.GetComponent<SpriteRenderer>().sprite = null;
        //    }
        //}

        private void PlayerMoveRecvFunc(string nickname, Vector2 vec)
        {
            // ������ 
            player.SetUserTarget(vec);
        }

        //private void OnTriggerEnter2D(Collider2D other) {
        //    if(other.tag == "Item"){
        //        nowItem = other.GetComponent<GrowingItem>();
        //    }
        //}
        //private void OnTriggerExit2D(Collider2D other)
        //{
        //    if(other.tag == "Item"){
        //        nowItem = null;
        //    }
        //}

    }

}
