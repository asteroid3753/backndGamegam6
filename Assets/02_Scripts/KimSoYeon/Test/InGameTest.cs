using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KSY
{
    public class InGameTest : MonoBehaviour
    {
        [SerializeField]
        private GameObject itemPrefab;

        Dictionary<int, string> itemList = new Dictionary<int, string>();
        int itemCount = 0;
        BoxCollider2D slimeArea;
        BoxCollider2D groundArea;
        // Start is called before the first frame update
        void Start()
        {
            slimeArea = GameObject.Find("Slime").GetComponent<BoxCollider2D>();
            groundArea = GameObject.Find("Ground").GetComponent<BoxCollider2D>();

            StartGame();
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private void StartGame()
        {
            itemCount = 0;
            StartCoroutine("CreateRanItem");
        }

        IEnumerator CreateRanItem()
        {
            while (true)
            {
                int itemType = Random.Range(0, 3);
                Vector2 spawnPos = GetRandomPosition();
                GameObject obj = Instantiate(itemPrefab);
                obj.transform.position = spawnPos;
                yield return new WaitForSeconds(4);
                itemCount++;
            }
        }
        private Vector2 GetRandomPosition()
        {
            Vector2 basePosition = transform.position;
            Vector2 size = groundArea.size;            //box colider2d, ¡Ô ∏ ¿« ≈©±‚ ∫§≈Õ

            //x, y√‡ ∑£¥˝ ¡¬«• æÚ±‚
            float posX = basePosition.x + Random.Range(-size.x / 2f, size.x / 2f);
            float posY = basePosition.y + Random.Range(-size.y / 2f, size.y / 2f);

            Vector2 spawnPos = new Vector2(posX, posY);

            return spawnPos;
        }


    }
}
