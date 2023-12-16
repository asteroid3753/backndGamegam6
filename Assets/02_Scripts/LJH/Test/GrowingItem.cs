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
        [SerializeField] Define.ItemType type;

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
        public void SetItemType(Define.ItemType _type){
            type = _type;
        }

    }

}
