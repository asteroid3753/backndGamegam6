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
            // recv �̺�Ʈ ���
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

                // move ��ǥ ���� (�ش� �÷��̾�(�� �ڽ�)�� SessionID, ��� ��ǥ��)
                BackEndManager.Instance.InGame.SendDataToInGame(msg); 
                yield return new WaitForSeconds(10);
            }
        }

        private void PlayerMoveRecvFunc(SessionId sessionId, Vector2 vec)
        {
            // ������ 

        }
    } 
}
