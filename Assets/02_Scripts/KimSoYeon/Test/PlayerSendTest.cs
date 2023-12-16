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
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                SlimeSizeUpMessage msg = new SlimeSizeUpMessage(0, Random.Range(0.1f, 0.5f));
                BackEndManager.Instance.InGame.SendDataToInGame(msg);
            }

            //if (Input.GetKeyDown(KeyCode.DownArrow))
            //{
            //    SlimeSizeUpMessage msg = new SlimeSizeUpMessage(1, Random.Range(0.1f, 0.5f));
            //    BackEndManager.Instance.InGame.SendDataToInGame(msg);
            //}

            //if (Input.GetKeyDown(KeyCode.LeftArrow))
            //{
            //    SlimeSizeUpMessage msg = new SlimeSizeUpMessage(2, Random.Range(0.1f, 0.5f));
            //    BackEndManager.Instance.InGame.SendDataToInGame(msg);
            //}

            //if (Input.GetKeyDown(KeyCode.RightArrow))
            //{
            //    SlimeSizeUpMessage msg = new SlimeSizeUpMessage(3, Random.Range(0.1f, 0.5f));
            //    BackEndManager.Instance.InGame.SendDataToInGame(msg);
            //}
        }

        private void Parsing_SlimeSizeUpEvent(int id, float addSize)
        {
            if (slimeObj != null)
            {
                slimeObj.transform.localScale = new Vector3(slimeObj.transform.localScale.x + addSize, slimeObj.transform.localScale.y + addSize);
            }
        }
    }
}
