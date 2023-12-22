using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSY.Protocol;
using KSY;
using Sirenix.OdinInspector;

namespace LJH
{
    public class Player_Mover : SerializedMonoBehaviour
    {
        float x, y;
        float uy = 22.5f, dy = -24.3f, lx = -42.8f, rx = 41.5f;

        [SerializeField] Player_Logic player;
        [SerializeField] bool isConrollAble = false;
        [SerializeField] bool isSlime = false;
        [SerializeField] Rigidbody2D _rigidBody;

        [SerializeField] Vector2 _inputVector;

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

            if (TotalGameManager.Instance.myNickName != player.NickName)
            {
                return;
            }

            Vector2 moveVector = _inputVector.normalized;
            

            moveVector *= player.MovingSpeed * Time.deltaTime;
            _rigidBody.velocity = moveVector;

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
                }

                _inputVector = new Vector2(horizontal, vertical);
            }

            // Player GetItem
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Debug.Log("");

                    if (isSlime)
                    {
                        if (player.GetUserItem() != 0)
                        {
                            SlimeSizeUpMessage sizeMsg = new SlimeSizeUpMessage(player.GetUserItem());
                            BackEndManager.Instance.InGame.SendDataToInGame(sizeMsg);
                        }
                        else
                        {
                            Debug.Log("슬라임 먹이 없음");
                        }
                    }
                    else if (player.CurrentTargetItem != null && player.GetUserItem() == 0)
                    {
                        Debug.Log("습득:" + player.CurrentTargetItem.ItemCode);
                        GrabItemMessage itemMsg = new GrabItemMessage(player.CurrentTargetItem.ItemCode);
                        BackEndManager.Instance.InGame.SendDataToInGame(itemMsg);
                    }
                }

            }

        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Item")
            {
                GrowingItem item = other.GetComponent<GrowingItem>();
                if (item != null)
                {
                    player.CurrentTargetItem = item;
                }
            }
            else if (other.tag == "Slime")
            {
                isSlime = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.tag == "Item")
            {
                GrowingItem item = other.GetComponent<GrowingItem>();
                if (item != null)
                {
                    player.CurrentTargetItem = null;
                }
            }
            else if (other.tag == "Slime")
            {
                isSlime = false;
            }
        }

    }
}

