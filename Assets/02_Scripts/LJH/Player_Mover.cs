using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSY.Protocol;
using KSY;
using Sirenix.OdinInspector;
using MorningBird.Sound;

namespace LJH
{
    public class Player_Mover : SerializedMonoBehaviour
    {
        float x, y;
        float uy = 22.5f, dy = -24.3f, lx = -42.8f, rx = 41.5f;

        [SerializeField, FoldoutGroup("PreDefine")] Player_Logic _player;
        [SerializeField, FoldoutGroup("PreDefine")] Rigidbody2D _rigidBody;
        [SerializeField, FoldoutGroup("About Dash")] float _dashPower => _player.DashPower;
        [SerializeField, FoldoutGroup("About Dash")] float _dashCoolTIme => _player.DashCoolTime;

        [SerializeField, FoldoutGroup("Debug/Dash")] bool _inputDash = false;
        [SerializeField, FoldoutGroup("Debug/Dash")] bool _canDash = false;
        [SerializeField, FoldoutGroup("Debug/Dash")] float _dashInnerCoolTime = 0f;
        [SerializeField, FoldoutGroup("Debug")] bool isConrollAble = false;
        [SerializeField, FoldoutGroup("Debug")] Vector2 _inputVector;

        public Vector2 GetUserPos()
        {
            return new Vector2(x, y);
        }

        // Start is called before the first frame update
        void Start()
        {
            _rigidBody = this.GetComponent<Rigidbody2D>();

            isConrollAble = false;

            Invoke("WaitAndStart", 2f);
        }

        public void SetFirstPos(Vector2 pos)
        {
            PlayerMoveMessage msg = new PlayerMoveMessage(pos);
            BackEndManager.Instance.InGame.SendDataToInGame(msg);
            Debug.Log(pos);
        }
        void WaitAndStart()
        {
            isConrollAble = true;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (isConrollAble == false)
                return;

            if (TotalGameManager.Instance.MyClientNickName != _player.NickName)
            {
                return;
            }

            Vector2 nomalizedInputVector = _inputVector.normalized;
            Vector2 moveVector = nomalizedInputVector;
            
            // BasicMove
            moveVector += (moveVector * _player.MovingSpeed * Time.deltaTime);
            
            // Dash
            if(_inputDash == true && _canDash == true)
            {
                _canDash = false;
                _dashInnerCoolTime = _dashCoolTIme;


                SoundManager.Instance.RequestPlayClip(_player.DashSound);
                moveVector += nomalizedInputVector * _dashPower;
            }

            // Apply to rigidBody
            _rigidBody.velocity += moveVector;

            // Send Moving Message To Server
            if (MorningBird.TimeSafer.Instance.GetFixed50msSafer == true)
            {
                Vector2 currentPosition = this.transform.position;

                PlayerMoveMessage msg = new PlayerMoveMessage(currentPosition);
                BackEndManager.Instance.InGame.SendDataToInGame(msg);

                // Debug.Log($"ServerSendingPosition : {currentPosition}");
            }
        }


        private void Update()
        {
            // Player Get input
            {
                float horizontal;
                float vertical;

                // Get Input
                {
                    horizontal = Input.GetAxisRaw("Horizontal");
                    vertical = Input.GetAxisRaw("Vertical");
                    if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
                    {
                        _inputDash = true;
                    }
                    else if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
                    {
                        _inputDash = false;
                    }
                }

                _inputVector = new Vector2(horizontal, vertical);
            }

            // Check Dash Time
            _dashInnerCoolTime -= Time.deltaTime;
            if(_dashInnerCoolTime <= 0f)
            {
                _canDash = true;
            }

        }
    }
}

