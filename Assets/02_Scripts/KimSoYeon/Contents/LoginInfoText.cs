using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace KSY
{
    public class LoginInfoText : MonoBehaviour
    {
        private TextMeshProUGUI infoTMP;
        private Animator anim;

        private string infoText;

        public string InfoText
        {
            get
            {
                return infoText;
            }
            set
            { 
                infoText = value;
                infoTMP.text = infoText;
                anim.Play("FadeOut");
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            infoTMP = GetComponent<TextMeshProUGUI>();
            anim = GetComponent<Animator>();
        }
    } 
}
