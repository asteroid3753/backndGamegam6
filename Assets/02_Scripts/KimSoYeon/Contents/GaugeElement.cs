using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KSY
{
    public class GaugeElement : MonoBehaviour
    {
        private TextMeshProUGUI textGUI;

        private LayoutElement element;

        private Image img;

        private float percent;
        public float Percent
        {
            get { return percent; }
            set
            {
                percent = value;
                element.flexibleWidth = percent;
                // �Ҽ��� ���ڸ� �� ���� ǥ��
                textGUI.text = $"{percent.ToString("F1")}%";
            }
        }

        private Color gaugeColor;
        public Color GaugeColor
        {
            get { return gaugeColor; }
            set
            {
                gaugeColor = value;
                img.color = gaugeColor;
            }

        }

        void Awake()
        {
            img = GetComponent<Image>();
            textGUI = GetComponentInChildren<TextMeshProUGUI>();
            element = GetComponent<LayoutElement>();
            element.transform.localScale = Vector3.one;
        }
    }

}