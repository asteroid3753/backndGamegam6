using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LJH
{
    public class Player : SerializedMonoBehaviour
    {
        [SerializeField] Vector2 _movningTarget;
        public Vector2 MovingTarget 
        {
            get => _movningTarget;
            set
            {
                _movningTarget = value;
                Debug.Log($"ServerGetPosition : {_movningTarget} ");
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

        [SerializeField] int havingItem = -1; 
        [SerializeField] GrowingItem _currentTargetItem;
        public GrowingItem CurrentTargetItem
        {
            get => _currentTargetItem;
            set
            {
                _currentTargetItem = value;
            }
        }

        SpriteRenderer spriteRenderer;

        //getter Test
        public int GetUserItem()
        {
            return havingItem;
        }

        //setter Test
        public void SetUserItem(GrowingItem _item)
        {
            if (_item == null)
            {
                havingItem = 0;
                spriteRenderer.sprite = null;
            }
            else
            {
                havingItem = _item.GrowPoint;
                spriteRenderer.sprite = _item.ItemImg;
            }
        }

        private void Update()
        {
            // Set Player Position
            {
                float x = Mathf.Lerp(this.transform.position.x, MovingTarget.x, 0.5f);
                float y = Mathf.Lerp(this.transform.position.y, MovingTarget.y, 0.5f);

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
        }

        private void Start()
        {
            spriteRenderer = gameObject.transform.Find("Item").GetComponent<SpriteRenderer>();
        }
    }
}
