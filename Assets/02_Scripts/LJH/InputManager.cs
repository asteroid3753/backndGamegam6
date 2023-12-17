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
        float uy = 22.5f, dy = -24.3f, lx = -42.8f, rx = 41.5f;

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
            
            //lx = -42.8 rx = 41.5 uy = 22.5 dy = -24.3
            if((lx <= x && x <= rx) && (dy <= y && y <= uy)){
                PlayerMoveMessage msg = new PlayerMoveMessage(new Vector2(x, y));
                BackEndManager.Instance.InGame.SendDataToInGame(msg); 
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("");

                if (isSlime)
                {
                    Debug.Log("������ ���̱� ����");
                    if (player.GetUserItem() == 0)
                    {
                        SlimeSizeUpMessage sizeMsg = new SlimeSizeUpMessage(player.GetUserItem());
                        BackEndManager.Instance.InGame.SendDataToInGame(sizeMsg);
                    }
                    else
                    {
                        Debug.Log("������ ���� ����");
                    }
                }
                else if (player.GetUserNowItem() != null && player.GetUserItem() == 0)
                {
                    Debug.Log("������");
                    GrabItemMessage itemMsg = new GrabItemMessage(player.GetUserNowItem().ItemCode);
                    BackEndManager.Instance.InGame.SendDataToInGame(itemMsg);
                }
            }

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

