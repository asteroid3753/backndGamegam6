using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSY.Protocol;
using KSY;
using BackEnd.Tcp;
using BackEnd;
using Unity.VisualScripting;

namespace LJH{
    public class InputManager : MonoBehaviour
    {  
        float x, y;
        Player player;

        [SerializeField] bool isConrollAble = false;

        [SerializeField] bool isSlime = false;

        public Vector2 GetUserPos(){
            return new Vector2(x,y);
        }

        // Start is called before the first frame update
        void Start()
        {
            player = GetComponent<Player>();

            isConrollAble = false;

            Invoke("WaitAndStart", 2f);
        }

        public void SetFirstPos(Vector2 pos)
        {
            PlayerMoveMessage msg = new PlayerMoveMessage(pos);
            BackEndManager.Instance.InGame.SendDataToInGame(msg);
            Debug.Log(pos);
        }
        void WaitAndStart()
        {
            isConrollAble = true;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (isConrollAble == false)
                return;

            if(TotalGameManager.Instance.myNickName != player.GetUserName())
            {
                return;
            }
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            if(horizontal != 0 || vertical != 0){
                this.GetComponent<Animator>().SetBool("Walk",true);
            }
            else{
                this.GetComponent<Animator>().SetBool("Walk",false);
            }

            x = player.transform.position.x + (horizontal * Time.deltaTime * player.GetUserSpeed());
            y = player.transform.position.y + (vertical * Time.deltaTime * player.GetUserSpeed());

            PlayerMoveMessage msg = new PlayerMoveMessage(new Vector2(x, y));

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("");

                if (isSlime)
                {
                    Debug.Log("슬라임 먹이기 가능");
                    if (player.GetUserItem() == 0)
                    {
                        SlimeSizeUpMessage sizeMsg = new SlimeSizeUpMessage(player.GetUserItem());
                        BackEndManager.Instance.InGame.SendDataToInGame(sizeMsg);
                    }
                    else
                    {
                        Debug.Log("슬라임 먹이 없음");
                    }
                }
                else if (player.GetUserNowItem() != null && player.GetUserItem() == 0)
                {
                    Debug.Log("아이템");
                    GrabItemMessage itemMsg = new GrabItemMessage(player.GetUserNowItem().ItemCode);
                    BackEndManager.Instance.InGame.SendDataToInGame(itemMsg);
                }
            }

            BackEndManager.Instance.InGame.SendDataToInGame(msg); 
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (other.tag == "Item")
            {
                GrowingItem item = other.GetComponent<GrowingItem>();
                if (item != null)
                {
                    player.SetUserNowItem(item);
                }
            }
            else if (other.tag == "Slime")
            {
                isSlime = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if(other.tag == "Item"){
                GrowingItem item = other.GetComponent<GrowingItem>();
                if (item != null)
                {
                    player.SetUserNowItem(null);
                } 
            }
            else if (other.tag == "Slime")
            {
                isSlime = false;
            }
        }
        
    }
}

