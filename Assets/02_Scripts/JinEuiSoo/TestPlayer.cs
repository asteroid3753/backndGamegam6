using KSY;
using KSY.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JES
{
    public class TestPlayer : MonoBehaviour
    {
        [SerializeField] float _movementSpeed;
        [SerializeField] TestItem _currentItem;

        [SerializeField] public string PlayerNickName;
        [SerializeField] public string MyClientNickName;

        private void Update()
        {
            if(PlayerNickName != MyClientNickName)
            {
                return;
            }

            if(Input.GetKey(KeyCode.UpArrow))
            {
                this.transform.Translate(Vector3.up * Time.deltaTime * _movementSpeed);
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                this.transform.Translate(Vector3.down * Time.deltaTime * _movementSpeed);
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                this.transform.Translate(Vector3.right * Time.deltaTime * _movementSpeed);
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                this.transform.Translate(Vector3.left * Time.deltaTime * _movementSpeed);
            }

            if(Input.GetKeyDown(KeyCode.Space))
            {
                GetGrowingItem();
            }

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