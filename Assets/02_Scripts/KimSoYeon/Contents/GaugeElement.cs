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

        [SerializeField]
        private TextMeshProUGUI nicknameTextGUI;

        private float percent;
        public float Percent
        {
            get { return percent; }
            set
            {
                percent = value;
                element.flexibleWidth = percent;
                // 소수점 한자리 수 까지 표현
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

        private string nickName;
        public string NickName
        {
            get { return nickName; }
            set
            {
                nickName = value;
                nicknameTextGUI.text = nickName;
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