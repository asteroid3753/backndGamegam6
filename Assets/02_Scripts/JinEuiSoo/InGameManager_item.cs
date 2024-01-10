using KSY;
using KSY.Protocol;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace LJH
{

    public partial class InGameManager : SerializedMonoBehaviour
    {
        [SerializeField]
        int itemMaxCount = 10;
        [SerializeField]
        Transform spawnPointObj;

        private int itemTypeCount;

        Transform[] itemSpawnPoint;
        HashSet<int> availablePoints;

        Coroutine createItemCoroutine;

        private void ItemEventAdd()
        {
            BackEndManager.Instance.Parsing.GrabItemEvent += Parsing_GrabItemEvent;
            BackEndManager.Instance.Parsing.CreateItemEvent += Parsing_CreateItemEvent;
        }

        private void SpawnPointInit()
        {
            itemSpawnPoint = spawnPointObj.GetComponentsInChildren<Transform>();
            availablePoints = new HashSet<int> ();

            for(int i = 0; i < itemSpawnPoint.Length; i++)
            {
                availablePoints.Add (i);
            }
        }
        
        private void GameItemInit()
        {
            if (TotalGameManager.Instance.isHost)
            {
                itemCount = 0;
                itemTypeCount = System.Enum.GetValues(typeof(Define.ItemType)).Length;
                createItemCoroutine = StartCoroutine(CreateItem());
            }
        }
        IEnumerator CreateItem()
        {
            while (true)
            {
                yield return new WaitForSeconds(itemSpawnSpan);

                if (itemMaxCount <= InGameItemDic.Count)
                {
                    continue;
                }

                int itemType = Random.Range(0, itemTypeCount + 1);
                int randomIndex = Random.Range(0, availablePoints.Count); // 랜덤한 인덱스 선택
                int spawnIndex = GetAvailableIndexByOrder(randomIndex);

                //Vector2 spawnPos = JES.JESFunctions.CreateRandomInstance();

                CreateItemMessage msg = new CreateItemMessage(itemType, itemCount, spawnIndex);
                BackEndManager.Instance.InGame.SendDataToInGame(msg);
                itemCount++;
            }
        }

        int GetAvailableIndexByOrder(int order)
        {
            int count = 0;
            foreach (int index in availablePoints)
            {
                if (count == order)
                    return index;
                count++;
            }
            return -1; //추후 에러처리 필요
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
                availablePoints.Add(InGameItemDic[itemCode].SpawnPointIndex);
                InGameItemDic.Remove(itemCode);
            }
        }

        private void Parsing_CreateItemEvent(int itemType, int itemCode, int index)
        {
            if (_isGameEnd) return;
            if (InGameItemDic != null)
            {
                GameObject obj = Instantiate(itemPrefab);
                obj.transform.position = itemSpawnPoint[index].position;
                GrowingItem item = obj.GetComponent<GrowingItem>();
                item.ItemCode = itemCode;
                item.Type = (Define.ItemType)itemType;
                item.SpawnPointIndex = index;
                availablePoints.Remove(index);
                InGameItemDic.Add(itemCode, item);
            }
        }

        #endregion
    }
}