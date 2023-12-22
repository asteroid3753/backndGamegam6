using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LJH
{
    public class Player_Logic : SerializedMonoBehaviour
    {
        [SerializeField, FoldoutGroup("PreDefine")] Player_Visual _playerVisual;

        [SerializeField] Vector2 _movningTarget;
        public Vector2 MovingTarget 
        {
            get => _movningTarget;
            set
            {
                _movningTarget = value;
                //Debug.Log($"ServerGetPosition : {_movningTarget} ");
            }

        }

        [SerializeField] float _movingSpeed = 2.0f;
        public float MovingSpeed
        {
            get => _movingSpeed;
            set
            {
                _movingSpeed = value;
            }
        }

        [SerializeField] bool isFlipX = false;
        public bool IsFlipX
        {
            get => isFlipX;
            set => isFlipX = value;
        }

        [SerializeField] public string NickName { get; set; }

        [SerializeField] int _currentHavingItemIdex = -1; 
        [SerializeField] GrowingItem _currentTargetItem;
        public GrowingItem CurrentTargetItem
        {
            get => _currentTargetItem;
            set
            {
                _currentTargetItem = value;
            }
        }

        [SerializeField] float _logicMoveLearpingTime = 0.15f;

        SpriteRenderer _itemSpriteRenderer;

        //getter Test
        public int GetUserItem()
        {
            return _currentHavingItemIdex;
        }

        //setter Test
        public void SetUserItem(GrowingItem _item)
        {
            if (_item == null)
            {
                _currentHavingItemIdex = 0;
                _itemSpriteRenderer.sprite = null;
            }
            else
            {
                _currentHavingItemIdex = _item.GrowPoint;
                _itemSpriteRenderer.sprite = _item.ItemImg;
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
        }

        private void Start()
        {
            _itemSpriteRenderer = _playerVisual.transform.Find("Item").GetComponent<SpriteRenderer>();
        }
    }
}
