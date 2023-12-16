using BackEnd.Tcp;
using KSY;
using KSY.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KSY
{
    public class PlayerSendTest : MonoBehaviour
    {
        GameObject slimeObj;
        // Start is called before the first frame update
        void Start()
        {
            slimeObj = GameObject.Find("Slime");
            BackEndManager.Instance.Parsing.SlimeSizeUpEvent += Parsing_SlimeSizeUpEvent;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SlimeSizeUpMessage msg = new SlimeSizeUpMessage(SessionId.Reserve, Random.Range(0.1f, 0.5f));
                BackEndManager.Instance.InGame.SendDataToInGame(msg);
                Debug.Log("Send Msg");
            }
        }

        private void Parsing_SlimeSizeUpEvent(SessionId sessionId, float addSize)
        {
            if (slimeObj != null)
            {
                slimeObj.transform.localScale = new Vector3(slimeObj.transform.localScale.x + addSize, slimeObj.transform.localScale.y + addSize);
            }
        }
    }
}
