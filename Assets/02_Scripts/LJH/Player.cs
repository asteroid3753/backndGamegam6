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


        //init
        public Player(int _id, string _name){
            userID = _id;
            userName = _name;
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

        private void Update() {
            float x = Mathf.Lerp(this.transform.position.x, target.x,0.1f);
            float y = Mathf.Lerp(this.transform.position.y, target.y,0.1f);
            this.transform.position = new Vector3(x,y,transform.position.z);
        }
    }
}
