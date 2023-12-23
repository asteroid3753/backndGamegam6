using KSY;
using KSY.Protocol;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LJH
{

    public partial class InGameManager : SerializedMonoBehaviour
    {
        [SerializeField]
        int itemMaxCount = 10;

        private int itemTypeCount;

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
                itemTypeCount = System.Enum.GetValues(typeof(Define.ItemType)).Length;
                StartCoroutine(CreateItem());
            }
        }
        IEnumerator CreateItem()
        {
            while (true)
            {
                if (itemMaxCount <= InGameItemDic.Count)
                {
                    yield return new WaitForSeconds(0.2f);
                    continue;
                }

                int itemType = Random.Range(0, itemTypeCount + 1);
                Vector2 spawnPos = JES.JESFunctions.CreateRandomInstance();

                CreateItemMessage msg = new CreateItemMessage(itemType, itemCount, spawnPos);
                BackEndManager.Instance.InGame.SendDataToInGame(msg);
                itemCount++;
                yield return new WaitForSeconds(itemSpawnSpan);
            }
        }

        #region PasingEventFunc

        private void Parsing_GrabItemEvent(string nickname, int itemCode)
        {
            if (_isGameEnd) return;
            Debug.Log($"{nickname}이 {itemCode}를 주웠다!");
            if (InGameItemDic.ContainsKey(itemCode))
            {
                NamePlayerPairs[nickname].SetUserItem(InGameItemDic[itemCode]);

                Destroy(InGameItemDic[itemCode].gameObject);
                InGameItemDic.Remove(itemCode);
            }
        }

        private void Parsing_CreateItemEvent(int itemType, int itemCode, UnityEngine.Vector2 arg3)
        {
            if (_isGameEnd) return;
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