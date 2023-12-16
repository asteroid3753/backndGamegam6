using KSY;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KSY
{
    public class ScoreGauge : MonoBehaviour
    {
        Dictionary<int, LayoutElement> gaugeList;

        // Start is called before the first frame update
        void Start()
        {
            gaugeList = new Dictionary<int, LayoutElement>();
            AddGauge(0, Color.red);
            AddGauge(1, Color.blue);
            AddGauge(2, Color.yellow);
            AddGauge(3, Color.green);

            BackEndManager.Instance.Parsing.SlimeSizeUpEvent += Parsing_SlimeSizeUpEvent;
        }

        private void AddGauge(int cnt, Color color)
        {
            GameObject imgobj = new GameObject { name = $"Player{cnt}" };
            imgobj.transform.SetParent(this.transform);
            Image img = imgobj.AddComponent<Image>();
            LayoutElement layoutEle = imgobj.AddComponent<LayoutElement>();
            img.color = color;
            layoutEle.flexibleWidth = 1;
            gaugeList.Add(cnt, layoutEle);
        }

        private void Parsing_SlimeSizeUpEvent(int id, float addSize)
        {
            gaugeList[id].flexibleWidth = (gaugeList[id].flexibleWidth + addSize) % 100;
        }
    }

}