using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace LJH{
    [System.Serializable]
    public class GrowingItem : MonoBehaviour
    {
        [SerializeField] int itemCode;
        [SerializeField] Sprite itemImg;
        private int growPoint;

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

        public GrowingItem(int _itemCode){
            itemCode = _itemCode;
            itemImg = Resources.Load<Sprite>("Item" + _itemCode);
        }
        public int GetItemCode(){
            return itemCode;
        }
        public Sprite GetItemImg(){
            return itemImg;
        }
        public Define.ItemType GetItemType(){
            return type;
        }


        private void SetItemType(Define.ItemType itemType)
        {
            switch (itemType)
            {
                case Define.ItemType.can:
                    itemImg = Resources.Load<Sprite>("Item6");
                    itemCode = 6;
                    growPoint = 10;
                    break;
                case Define.ItemType.apple:
                    itemImg = Resources.Load<Sprite>("Item1");
                    itemCode = 1;
                    growPoint = 20;
                    break;
                case Define.ItemType.backendIcon:
                    itemImg = Resources.Load<Sprite>("Item2");
                    itemCode = 2;
                    growPoint = 30;
                    break;
            }
        }

    }

}
