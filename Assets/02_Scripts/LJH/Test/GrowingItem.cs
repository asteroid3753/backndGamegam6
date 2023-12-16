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
                case Define.ItemType.can:
                    ItemImg = Resources.Load<Sprite>("Item6");
                    growPoint = 10;
                    break;
                case Define.ItemType.apple:
                    ItemImg = Resources.Load<Sprite>("Item1");
                    growPoint = 20;
                    break;
                case Define.ItemType.backendIcon:
                    ItemImg = Resources.Load<Sprite>("Item2");
                    growPoint = 30;
                    break;
            }
        }

    }

}
