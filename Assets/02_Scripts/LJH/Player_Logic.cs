using BackEndChat.Message;
using KSY.Protocol;
using KSY;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MorningBird;

namespace LJH
{
    public class Player_Logic : SerializedMonoBehaviour
    {
        [SerializeField, FoldoutGroup("PreDefine")] Player_Visual _playerVisual;
        [SerializeField, FoldoutGroup("PreDefine")] MorningBird.Sound.AudioStorage _dashSound;
        public MorningBird.Sound.AudioStorage DashSound => _dashSound;

        [SerializeField, FoldoutGroup("About Moving")] Vector2 _movningTarget;
        public Vector2 MovingTarget 
        {
            get => _movningTarget;
            set
            {
                _movningTarget = value;
                //Debug.Log($"ServerGetPosition : {_movningTarget} ");
            }

        }

        [SerializeField, FoldoutGroup("About Moving")] float _movingSpeed = 2.0f;
        public float MovingSpeed
        {
            get => _movingSpeed;
            set
            {
                _movingSpeed = value;
            }
        }

        [SerializeField, FoldoutGroup("About Moving")] bool isFlipX = false;
        public bool IsFlipX
        {
            get => isFlipX;
            set => isFlipX = value;
        }

        [SerializeField, FoldoutGroup("About Moving")] float _logicMoveLearpingTime = 0.15f;
        [SerializeField, FoldoutGroup("About Moving")] float _dashPower = 500f;
        public float DashPower => _dashPower;
        [SerializeField, FoldoutGroup("About Moving")] float _dashCoolTime = 2f;
        public float DashCoolTime => _dashCoolTime;


        [SerializeField, FoldoutGroup("Player Information")] public string NickName { get; set; }

        [SerializeField, FoldoutGroup("About Item")] int _currentHavingItemIdex = 0; 
        [SerializeField, FoldoutGroup("About Item")] int _currentItemGrowingPoint = 0; 
        [SerializeField, FoldoutGroup("About Item")] GrowingItem _currentTargetItem;
        public GrowingItem CurrentTargetItem
        {
            get => _currentTargetItem;
            set
            {
                _currentTargetItem = value;
            }
        }

        [SerializeField, FoldoutGroup("About Item")] float _itemDetectingArea = 1.5f;
        [SerializeField, FoldoutGroup("About Item")] bool _findSlime; 

        SpriteRenderer _itemSpriteRenderer;
        Vector3 offset = Vector3.zero;
        Vector2 capsuleSize = Vector2.zero;
        SpriteRenderer bodyRenderer;

        //setter Test
        public void SetUserItem(GrowingItem _item)
        {
            if (_item == null)
            {
                _currentHavingItemIdex = -1;
                _currentItemGrowingPoint = 0;
                _itemSpriteRenderer.sprite = null;
                Debug.Log("item이 null임");
            }
            else
            {
                //_currentHavingItemIdex = _item.GrowPoint;
                _currentHavingItemIdex = _item.ItemCode;
                _currentItemGrowingPoint = _item.GrowPoint;
                _itemSpriteRenderer.sprite = _item.ItemImg;
                Debug.Log("item이 null이 아님");
            }
        }

        private void Update()
        {
            // Set Player Position to MoveTargetPosition
            {
                float x = Mathf.Lerp(this.transform.position.x, MovingTarget.x, _logicMoveLearpingTime);
                float y = Mathf.Lerp(this.transform.position.y, MovingTarget.y, _logicMoveLearpingTime);

                this.transform.position = new Vector3(x, y, transform.position.z);
            }

            // Set Player Flip
            {
                if (MovingTarget.x - this.transform.position.x > 0.1f)
                {
                    isFlipX = true; // right
                }
                if (MovingTarget.x - this.transform.position.x < -0.1f)
                {
                    isFlipX = false; // left
                }
            }

            // Set Player Animation
            {
                Vector2 calculateIsMoving;
                calculateIsMoving = MovingTarget - new Vector2(this.transform.position.x, this.transform.position.y);
                bool isPlayerMoving = calculateIsMoving.magnitude >= 0.1f;

                if (isPlayerMoving == true)
                {
                    _playerVisual.IsWalk = true;
                }
                else
                {
                    _playerVisual.IsWalk = false;
                }
            }

            if (NickName != TotalGameManager.Instance.MyClientNickName)
            {
                return;
            }

            // Detecting Nerby Item and Slime
            {
                DetectingNerbyItemAndSlime();
            }

            // GetGrowingItem
            {
                GetGrowingItem();
            }
        }

        void GetGrowingItem()
        {
            if (Input.GetKeyDown(KeyCode.Space) == false)
            {
                return;
            }

            bool canTakeItem = (CurrentTargetItem != null) == true && (_currentHavingItemIdex == -1) == true;

            if (canTakeItem == true)
            {
                GrabItemMessage itemMsg = new GrabItemMessage(CurrentTargetItem.ItemCode);
                BackEndManager.Instance.InGame.SendDataToInGame(itemMsg);
                Debug.Log($"Send Meg to Server _Grab Item_ : item index {CurrentTargetItem.ItemCode} .");
                return;
            }

            bool haveItem = (_currentHavingItemIdex != -1) == true;
            bool canGiveItemToSlime = _findSlime == true && haveItem;

            if (canGiveItemToSlime)
            {
                SlimeSizeUpMessage sizeMsg = new SlimeSizeUpMessage(_currentItemGrowingPoint);
                BackEndManager.Instance.InGame.SendDataToInGame(sizeMsg);
                Debug.Log($"Send Meg to Server _Give item to slime_ : item Grow Point {_currentItemGrowingPoint} .");
            }
        }

        void DetectingNerbyItemAndSlime()
        {
            // Early Return for resource save
            if (TimeSafer.Instance.Get100msSafer == false)
            {
                return;
            }

            // Find Item
            List<Collider2D> nerbyItemList = Physics2D.OverlapCapsuleAll(this.transform.position + offset, capsuleSize, CapsuleDirection2D.Vertical, 0).ToList();

            // refine GrowingItem only
            for (int i = nerbyItemList.Count - 1; i >= 0; i--)
            {
                Collider2D content = nerbyItemList[i];

                if (content.transform.tag == "Slime")
                {
                    _findSlime = true;
                    continue;
                }

                if (content.transform.TryGetComponent<GrowingItem>(out _) == false)
                {
                    nerbyItemList.Remove(content);
                }
            }

            // Early Return
            if (nerbyItemList.Count == 0)
            {
                _currentTargetItem = null;
                return;
            }

            // calculate each of distance
            Vector2 logicPosition = this.transform.position;
            Vector2 targetPosition;

            float lastShortestLength = 0f;
            int leastShortestItemIdex = 0;

            // find most nearest item
            for (int i = 0; i < nerbyItemList.Count; i++)
            {
                Collider2D item = nerbyItemList[i];

                targetPosition = item.transform.position;

                Vector2 directionToItem = targetPosition - logicPosition;
                float distanceToItem = directionToItem.magnitude;

                // If the index is 0, declare stand.
                if (i == 0)
                {
                    lastShortestLength = distanceToItem;
                    leastShortestItemIdex = 0;
                    continue;
                }

                if (distanceToItem < lastShortestLength)
                {
                    leastShortestItemIdex = i;
                }
            }

            GrowingItem growingItem = nerbyItemList[leastShortestItemIdex].transform.GetComponent<GrowingItem>();

            _currentTargetItem = growingItem;
        }

        private void LateUpdate()
        {
            _findSlime = false;
        }

        private void Start()
        {
            _itemSpriteRenderer = _playerVisual.transform.Find("Item").GetComponent<SpriteRenderer>();
            CapsuleCollider2D col = GetComponent<CapsuleCollider2D>();
            offset = col.offset;
            capsuleSize = col.size;
        }
    }
}
