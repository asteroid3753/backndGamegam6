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
        [FoldoutGroup("PreDefine"), SerializeField] ParticleSystem _moveParticle;


        [FoldoutGroup("Debug")]
        [ShowInInspector] public bool IsWalk;
        [FoldoutGroup("Debug")]
        [SerializeField] GameObject canGiveItemUI;

        ParticleSystem.EmissionModule _moveParticleEmission;



        private void Start()
        {
            _bodySpriteRenderer = this.gameObject.transform.Find("Body").GetComponent<SpriteRenderer>();
            _moveParticleEmission = _moveParticle.emission;
        }


        private void Update() 
        {
            // Update Visual's Transform
            this.transform.position = player.transform.position;

            // Update FlipX
            _bodySpriteRenderer.flipX = player.IsFlipX;

            // Update SetWalk
            _animator.SetBool("Walk", IsWalk);

            // Update Move ParticleSystem
            if (IsWalk == true)
            {
                _moveParticleEmission.enabled = true;
            }
            else
            {
                _moveParticleEmission.enabled = false;

            }
            
            // canGiveItemUI
            canGiveItemUI.SetActive(player.canGiveItemToSlime);
        }

        public void SetNickName(string name)
        {
            _nameTMP.text = name;
        }
    }
}
