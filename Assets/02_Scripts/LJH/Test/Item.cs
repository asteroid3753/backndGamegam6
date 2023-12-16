using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class Item : MonoBehaviour
{
    [SerializeField] int itemCode;
    [SerializeField] Sprite itemImg;

    public int GetItemCode(){
        return itemCode;
    }
    public Sprite GetItemImg(){
        return itemImg;
    }


    public void SetItemCode(int _code){
        itemCode = _code;
    }

}
