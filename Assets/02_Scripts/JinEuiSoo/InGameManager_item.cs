using KSY;
using KSY.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LJH
{

    public partial class InGameManager : MonoBehaviour
    {
        private void ItemUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log($"{testCnt}�� �ƿ��� ����");
                BackEndManager.Instance.InGame.SendDataToInGame(new GrabItemMessage(testCnt++));
            }

        }

        private void ItemEventAdd()
        {
            BackEndManager.Instance.Parsing.GrabItemEvent += Parsing_GrabItemEvent;
            BackEndManager.Instance.Parsing.CreateItemEvent += Parsing_CreateItemEvent;
        }

        private void GameItemInit()
        {
            if (TotalGameManager.Instance.isHost)
            {
                itemCount = 0;
                StartCoroutine(CreateItem());
            }
        }
        IEnumerator CreateItem()
        {
            while (true)
            {
                int itemType = Random.Range(0, 3);
                UnityEngine.Vector2 spawnPos = JES.JESFunctions.CreateRandomInstance();

                CreateItemMessage msg = new CreateItemMessage(itemType, itemCount, spawnPos);
                BackEndManager.Instance.InGame.SendDataToInGame(msg);
                itemCount++;
                yield return new WaitForSeconds(itemSpawnSpan);
            }
        }

        #region PasingEventFunc

        private void Parsing_GrabItemEvent(string nickname, int itemCode)
        {
            //NamePlayerPairs[nickname].
            if (InGameItemDic.ContainsKey(itemCode))
            {
                Destroy(InGameItemDic[itemCode].gameObject);
                InGameItemDic.Remove(itemCode);
            }
        }

        private void Parsing_CreateItemEvent(int itemType, int itemCode, UnityEngine.Vector2 arg3)
        {
            if (InGameItemDic != null)
            {
                GameObject obj = Instantiate(itemPrefab);
                obj.transform.position = arg3;
                GrowingItem item = obj.GetComponent<GrowingItem>();
                item.ItemCode = itemCode;
                item.Type = (Define.ItemType)itemType;
                InGameItemDic.Add(itemCode, item);
            }
        }
        #endregion
    }
}