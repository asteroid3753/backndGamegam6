using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KSY
{
    public class SlimeSizeUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject slimeSizeObj;

        [SerializeField]
        private GameObject slimeSizeBgObj;


        private Vector2 slimeSize;

        public Vector2 SlimeSize
        {
            get { return  slimeSize; }
            set
            {
                slimeSize = value;

                slimeSizeObj.transform.localScale = slimeSize;
            }
        }

        private float initSlimeSize;

        public float InitSlimeSize
        {
            get { return initSlimeSize; }
            set
            {
                initSlimeSize = value;

                slimeSizeBgObj.transform.localScale = new Vector3(initSlimeSize, initSlimeSize);
            }
        }
    } 
}
