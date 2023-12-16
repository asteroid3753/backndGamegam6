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

                // move ��ǥ ���� (�ش� �÷��̾�(�� �ڽ�)�� SessionID, ��� ��ǥ��)
                BackEndManager.Instance.InGame.SendDataToInGame(msg); 
                yield return new WaitForSeconds(2);
            }
        }

      
    } 
}
