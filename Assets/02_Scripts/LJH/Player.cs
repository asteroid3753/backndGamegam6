using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LJH{
    public class Player : MonoBehaviour
    {
        [SerializeField] int userID;
        [SerializeField] string userName;
        [SerializeField] Vector2 target;
        [SerializeField] float userSpeed = 2.0f;
        [SerializeField] bool flipX = false;

        [SerializeField] private string nickName;
        [SerializeField] public string NickName;
        [SerializeField] public string SetNickName => nickName;

        //test
        [SerializeField] Item havingItem;

        //init
        public Player(int _id, string _name, float _speed){
            userID = _id;
            userName = _name;
            userSpeed = _speed;
        }

        //getter
        public int GetUserID(){
            return userID;
        }
        public string GetUserName(){
            return userName;
        }
        public Vector2 GetUserTarget(){
            return target;
        }
        public float GetUserSpeed(){
            return userSpeed;
        }
        public bool GetUserFlip(){
            return flipX;
        }
        //getter Test
        public Item GetUserItem(){
            return havingItem;
        }


        //setter
        public void SetUserID(int _id){
            userID = _id;
        }
        public void SetUserName(string _name){
            userName = _name;
        }
        public void SetUserTarget(Vector2 _target){
            target = _target;
        }
        public void SetUserSpeed(float _speed){
            userSpeed = _speed;
        }
        public void SetUserFlip(bool _flipX){
            flipX = _flipX;
        }
        //setter Test
        public void SetUserItem(Item _item){
            havingItem = _item;
        }

        private void Update() {
            if(target.x - this.transform.position.x > 0.1f){
                SetUserFlip(true); //right
            }
            if(target.x - this.transform.position.x < -0.1f){
                SetUserFlip(false); //right
            }
            float x = Mathf.Lerp(this.transform.position.x, target.x,0.1f);
            float y = Mathf.Lerp(this.transform.position.y, target.y,0.1f);
            
            this.transform.position = new Vector3(x,y,transform.position.z);
        }
    }
}
