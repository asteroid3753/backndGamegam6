using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSY.Protocol;
using BackEnd.Tcp;

namespace KSY
{
    public class SendRecvTest : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            // recv 이벤트 등록
            BackEndManager.Instance.Parsing.PlayerMoveEvent.AddListener(PlayerMoveRecvFunc);
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
                PlayerMoveMessage msg = new PlayerMoveMessage(SessionId.Reserve, new Vector2(0, 0));

                // move 좌표 전송 (해당 플레이어(나 자신)의 SessionID, 계산 좌표값)
                BackEndManager.Instance.InGame.SendDataToInGame(msg); 
                yield return new WaitForSeconds(10);
            }
        }

        private void PlayerMoveRecvFunc(SessionId sessionId, Vector2 vec)
        {
            // 들어오면 

        }
    } 
}
