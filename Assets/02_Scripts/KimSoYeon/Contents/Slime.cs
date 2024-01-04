using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KSY
{
    public class Slime : MonoBehaviour
    {
        SpriteRenderer spriteRender;
        Color normalColor;
        Color hideColor;

        private void Start()
        {
            spriteRender = GetComponent<SpriteRenderer>();
            normalColor = spriteRender.color;
            hideColor = normalColor;
            hideColor.a = 0.7f;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.CompareTag("Player"))
                spriteRender.color = hideColor;
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
                spriteRender.color = normalColor;
        }
    } 
}
