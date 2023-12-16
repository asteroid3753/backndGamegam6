using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSY.Protocol;
using BackEnd.Tcp;

namespace KSY
{
    public class SendRecvTest : MonoBehaviour
    {
        int count = 0;
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine("SendTest");
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        IEnumerator SendTest()
        {
            while (true)
            {
                count++;
                PlayerMoveMessage msg = new PlayerMoveMessage(SessionId.Reserve, new Vector2(0, count));

                // move 좌표 전송 (해당 플레이어(나 자신)의 SessionID, 계산 좌표값)
                BackEndManager.Instance.InGame.SendDataToInGame(msg); 
                yield return new WaitForSeconds(2);
            }
        }

      
    } 
}
