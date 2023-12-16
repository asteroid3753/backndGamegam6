using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MorningBird.Sound;

namespace JES
{
    public class VFXSoundPlayer : MonoBehaviour
    {
        #region Singleton Settings

        private static VFXSoundPlayer _instance;

        /// <summary>
        /// VFXSoundPlayer
        /// </summary>
        public static VFXSoundPlayer Instance
        {
            get
            {
                if (_instance == null)
                {
                    VFXSoundPlayer obj = FindObjectOfType<VFXSoundPlayer>();

                    if (obj != null)
                    {
                        _instance = obj;
                    }
                    else
                    {
                        var newSingleton = new GameObject("VFXSoundPlayer").AddComponent<VFXSoundPlayer>();
                        _instance = newSingleton;
                    }
                }
                return _instance;
            }
        }

        #endregion

        private void Awake()
        {
            #region Singleton Instantiate
            var objs = FindObjectsOfType<VFXSoundPlayer>();
            if (objs.Length > 1)
            {
                Debug.LogError("New VFXSoundPlayer Added And Destroy Automatically");
                Destroy(this.gameObject);
                return;
            }
            #endregion
        }

        [SerializeField] AudioStorage[] slimeSmiles;
        [SerializeField] AudioStorage slimeGrowl;
        [SerializeField] AudioStorage pickUp;
        [SerializeField] AudioStorage pickDown;

        public void PlaySlimeSmile()
        {
            var sound = slimeSmiles[Random.Range(0, slimeSmiles.Length)];

            SoundManager.Instance.RequestPlayClip(sound);
        }

        public void PlaySlimeGrowl()
        {
            SoundManager.Instance.RequestPlayClip(slimeGrowl);
        }

        public void PlayPickUp()
        {
            SoundManager.Instance.RequestPlayClip(pickUp);
        }

        public void PlayPickDown()
        {
            SoundManager.Instance.RequestPlayClip(pickDown);

        }

    }

}