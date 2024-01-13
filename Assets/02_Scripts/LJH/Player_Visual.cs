using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace LJH
{
    public class Player_Visual : SerializedMonoBehaviour
    {

        [FoldoutGroup("PreDefine"), SerializeField] LJH.Player_Logic player;
        [FoldoutGroup("PreDefine"), SerializeField] SpriteRenderer _bodySpriteRenderer;
        [FoldoutGroup("PreDefine"), SerializeField] Animator _animator;
        [FoldoutGroup("PreDefine"), SerializeField] TextMeshPro _nameTMP;

        [SerializeField] bool _isWalk;
        [SerializeField] GameObject canGiveItemUI;

        public bool IsWalk 
        {
            get => _isWalk;
            set => _isWalk = value;
        }

        private void Start()
        {
            _bodySpriteRenderer = this.gameObject.transform.Find("Body").GetComponent<SpriteRenderer>();
        }


        private void Update() 
        {
            // Update Visual's Transform
            this.transform.position = player.transform.position;

            // Update FlipX
            _bodySpriteRenderer.flipX = player.IsFlipX;

            // Update SetWalk
            _animator.SetBool("Walk", _isWalk);
            
            // canGiveItemUI
            canGiveItemUI.SetActive(player.canGiveItemToSlime);
        }

        public void SetNickName(string name)
        {
            _nameTMP.text = name;
        }
    }
}
