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
            // StartCoroutine(SendTest());

        }

        // Update is called once per frame
        void FixedUpdate()
        {
            x = player.transform.position.x + (Input.GetAxisRaw("Horizontal") * Time.deltaTime * player.GetUserSpeed());
            y = player.transform.position.y + (Input.GetAxisRaw("Vertical") * Time.deltaTime * player.GetUserSpeed());
            
            PlayerMoveMessage msg = new PlayerMoveMessage(SessionId.Reserve, new Vector2(x, y));

            // move ��ǥ ���� (�ش� �÷��̾�(�� �ڽ�)�� SessionID, ��� ��ǥ��)
            BackEndManager.Instance.InGame.SendDataToInGame(msg); 
        }

        IEnumerator SendTest()
        {
            while (true)
            {
                PlayerMoveMessage msg = new PlayerMoveMessage(SessionId.Reserve, new Vector2(x, y));

                // move ��ǥ ���� (�ش� �÷��̾�(�� �ڽ�)�� SessionID, ��� ��ǥ��)
                BackEndManager.Instance.InGame.SendDataToInGame(msg); 
                yield return new WaitForEndOfFrame();
            }
        }
        
    }
}

