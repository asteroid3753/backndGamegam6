using KSY;
using KSY.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JES
{
    public class TestPlayer : MonoBehaviour
    {
        [SerializeField] public float MovementSpeed;
        [SerializeField] TestItem _currentItem;

        [SerializeField] public string PlayerNickName;
        [SerializeField] public string MyClientNickName;

        [SerializeField] Vector3 _target;

        private void Update()
        {
            if(PlayerNickName != MyClientNickName)
            {
                return;
            }

            if(Input.GetKey(KeyCode.UpArrow))
            {
                this.transform.Translate(Vector3.up * Time.deltaTime * MovementSpeed);
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                this.transform.Translate(Vector3.down * Time.deltaTime * MovementSpeed);
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                this.transform.Translate(Vector3.right * Time.deltaTime * MovementSpeed);
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                this.transform.Translate(Vector3.left * Time.deltaTime * MovementSpeed);
            }

            if(Input.GetKeyDown(KeyCode.Space))
            {
                GetGrowingItem();
            }

            float x = Mathf.Lerp(this.transform.position.x, _target.x, 0.1f);
            float y = Mathf.Lerp(this.transform.position.y, _target.y, 0.1f);

            this.transform.position = new Vector3(x, y, transform.position.z);

        }

        void GetGrowingItem()
        {
            if (_currentItem == null)
            {
                return;
            }

            if (_currentItem.IsDisalbed == true)
            {
                return;
            }



            int itemValue = _currentItem.GetIndexValue();

            GrabItemMessage msg = new GrabItemMessage(itemValue);
            BackEndManager.Instance.InGame.SendDataToInGame(msg);

        }

        public void SetUserTarget(Vector2 target)
        {
            _target = target;
        }



        private void OnTriggerStay2D(Collider2D collision)
        {
            if(collision.TryGetComponent<TestItem>(out TestItem testItem) == true)
            {
                _currentItem = testItem;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent<TestItem>(out TestItem testItem) == true)
            {
                _currentItem = null;
            }
        }

    }

}