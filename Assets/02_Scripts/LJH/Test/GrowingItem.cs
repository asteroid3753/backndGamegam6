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


        public int GetItemCode(){
            return itemCode;
        }
        public Sprite GetItemImg(){
            return itemImg;
        }
        public Define.ItemType GetItemType(){
            return type;
        }


        public void SetItemCode(int _code){
            itemCode = _code;
        } 
        public void SetItemImg(Sprite _itemImg){
            itemImg = _itemImg;
        }

        private void SetItemType(Define.ItemType itemType)
        {
            switch (itemType)
            {
                case Define.ItemType.can:
                    //itemImg = "";
                    growPoint = 10;
                    break;
                case Define.ItemType.apple:
                    //itemImg = "";
                    growPoint = 20;
                    break;
                case Define.ItemType.backendIcon:
                    //itemImg = "";
                    growPoint = 30;
                    break;
            }
        }

    }

}
