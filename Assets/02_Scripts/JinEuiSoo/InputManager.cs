using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSY.Protocol;
using KSY;
using BackEnd.Tcp;
using BackEnd;

namespace JES
{
    public class InputManager : MonoBehaviour
    {  
        float x, y;
        TestPlayer player;
        public Vector2 GetUserPos(){
            return new Vector2(x,y);
        }

        // Start is called before the first frame update
        void Start()
        {
            player = GetComponent<TestPlayer>();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            x = player.transform.position.x + (Input.GetAxisRaw("Horizontal") * Time.deltaTime * player.MovementSpeed);
            y = player.transform.position.y + (Input.GetAxisRaw("Vertical") * Time.deltaTime * player.MovementSpeed);
            
            PlayerMoveMessage msg = new PlayerMoveMessage(new Vector2(x, y));

            // move ��ǥ ���� (�ش� �÷��̾�(�� �ڽ�)�� SessionID, ��� ��ǥ��)
            BackEndManager.Instance.InGame.SendDataToInGame(msg); 
        }

        // IEnumerator SendTest()
        // {
        //     while (true)
        //     {
        //         PlayerMoveMessage msg = new PlayerMoveMessage(new Vector2(x, y));

        //         // move ��ǥ ���� (�ش� �÷��̾�(�� �ڽ�)�� SessionID, ��� ��ǥ��)
        //         BackEndManager.Instance.InGame.SendDataToInGame(msg); 
        //         yield return new WaitForEndOfFrame();
        //     }
        // }
        
    }
}

