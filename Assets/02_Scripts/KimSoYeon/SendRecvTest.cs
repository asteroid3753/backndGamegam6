using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSY.Protocol;

namespace KSY
{
    public class SendRecvTest : MonoBehaviour
    {
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
                //KeyMessage msg = new KeyMessage(10, new bool[4] { true, false, false, false });
                yield return new WaitForSeconds(10);
            }
        }

        
    } 
}
