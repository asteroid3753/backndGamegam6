using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace LJH{
    [System.Serializable]
    public class GrowingItem : MonoBehaviour
    {
        private SpriteRenderer spriteRender;

        [SerializeField] int itemCode;
        public int ItemCode { get { return itemCode; } set { itemCode = value; } }

        [SerializeField] Sprite itemImg;
        public Sprite ItemImg
        {
            get { return itemImg; }
            set 
            { 
                itemImg = value;
                spriteRender.sprite = itemImg;
            }
        }

        private int growPoint;
        public int GrowPoint { get { return growPoint; } set { growPoint = value; } }

        private Define.ItemType type;

        public Define.ItemType Type
        {
            get { return type; }
            set
            {
                type = value;
                SetItemType(type);
            }
        }

        private void Awake()
        {
            spriteRender = GetComponent<SpriteRenderer>();
        }

        private void SetItemType(Define.ItemType itemType)
        {
            switch (itemType)
            {
                case Define.ItemType.backendIcon1:
                    ItemImg = Resources.Load<Sprite>("Item0");
                    growPoint = 20;
                    break;
                case Define.ItemType.backendIcon2:
                    ItemImg = Resources.Load<Sprite>("Item1");
                    growPoint = 20;
                    break;
                case Define.ItemType.backendIcon3:
                    ItemImg = Resources.Load<Sprite>("Item2");
                    growPoint = 20;
                    break;
                case Define.ItemType.backendIcon4:
                    ItemImg = Resources.Load<Sprite>("Item3");
                    growPoint = 20;
                    break;
                case Define.ItemType.fan:
                    ItemImg = Resources.Load<Sprite>("Item4");
                    growPoint = 100;
                    break;
                case Define.ItemType.washingMachine:
                    ItemImg = Resources.Load<Sprite>("Item5");
                    growPoint = 100;
                    break;
                case Define.ItemType.hotSix:
                    ItemImg = Resources.Load<Sprite>("Item6");
                    growPoint = 300;
                    break;
                case Define.ItemType.berry:
                    ItemImg = Resources.Load<Sprite>("Item7");
                    growPoint = 10;
                    break;
                case Define.ItemType.acorn:
                    ItemImg = Resources.Load<Sprite>("Item8");
                    growPoint = 30;
                    break;
            }
        }

    }

}
