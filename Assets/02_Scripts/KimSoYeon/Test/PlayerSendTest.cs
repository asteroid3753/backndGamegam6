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
                SlimeSizeUpMessage msg = new SlimeSizeUpMessage(Random.Range(0.1f, 1f));
                BackEndManager.Instance.InGame.SendDataToInGame(msg);
            }
        }

        private void Parsing_SlimeSizeUpEvent(string nickname, float addSize)
        {
            if (slimeObj != null)
            {
                slimeObj.transform.localScale = new Vector3(slimeObj.transform.localScale.x + (addSize / 500),
                    slimeObj.transform.localScale.y + (addSize / 500));
            }


        }
    }
}
