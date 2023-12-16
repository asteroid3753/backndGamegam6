using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using BackEnd;
using BackEnd.Tcp;


namespace LJH{
    [System.Serializable]
    public class User{
        int userID;
        string userName;
        float x, y;
        float userSpeed = 2.0f;


        //init
        public User(int _id, string _name){
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
        public Vector2 GetUserPos(){
            return new Vector2(x,y);
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
        public void SetUserPos(float _x, float _y){
            x = _x; y = _y;
        }
        public void SetUserSpeed(float _speed){
            userSpeed = _speed;
        }
    }
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] User user;
        

        //server
        string serverAddress;
        ushort serverPort;
        
        // Start is called before the first frame update
        void Start()
        {
            //서버연결이 완료되면 서버에서 현재 플레이어의 아이디와 이름을 가져온 후 초기화.
            user = new User(0000,"asdf");
        }

        // Update is called once per frame
        void Update()
        {
            //UserMove
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");
            this.transform.Translate(new Vector3(x,y,0) * Time.deltaTime * user.GetUserSpeed());
            user.SetUserPos(this.transform.position.x, this.transform.position.y);

        }

        void JoinGameServer(){
            bool isReConnecting = true;
            ErrorInfo errorInfo;
            if(Backend.Match.JoinGameServer(serverAddress, serverPort, isReConnecting, out errorInfo) == false){
                //error
                return;
            }

        }
    }

}
