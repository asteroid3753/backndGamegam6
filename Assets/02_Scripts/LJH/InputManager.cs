using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSY.Protocol;
using KSY;
using BackEnd.Tcp;
using BackEnd;

namespace LJH{
    public class InputManager : MonoBehaviour
    {  
        float x, y;
        Player player;



        public Vector2 GetUserPos(){
            return new Vector2(x,y);
        }

        // Start is called before the first frame update
        void Start()
        {
            player = GetComponent<Player>();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
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
            
            if(player.GetUserNowItem() != null && Input.GetKeyDown(KeyCode.Space)){
                print("item" + player.GetUserNowItem().ItemCode);
                
                GrabItemMessage itemMsg = new GrabItemMessage(player.GetUserNowItem().ItemCode);
                BackEndManager.Instance.InGame.SendDataToInGame(itemMsg); 
            }
            if(player.GetUserNowItem() == null && Input.GetKeyDown(KeyCode.Space)){
                print("item drop");
                GrabItemMessage itemMsg = new GrabItemMessage(-1);
                BackEndManager.Instance.InGame.SendDataToInGame(itemMsg); 
            }
            // move ��ǥ ���� (�ش� �÷��̾�(�� �ڽ�)�� SessionID, ��� ��ǥ��)
            BackEndManager.Instance.InGame.SendDataToInGame(msg); 
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if(other.tag == "Item"){
                player.SetUserNowItem(other.GetComponent<GrowingItem>());
            }
        }
        private void OnTriggerExit2D(Collider2D other)
        {
            if(other.tag == "Item"){
                player.SetUserNowItem(null);
            }
        }
        
    }
}

