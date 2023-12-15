using System.Collections;
using UnityEngine;
using Sirenix.OdinInspector;

namespace MorningBird.Sound
{
    public class SimpleAmbienceSoundCreator : SerializedMonoBehaviour
    {
        [FoldoutGroup("PreDefine")]
        [SerializeField] AudioStorage _ambienceSound;

        [FoldoutGroup("Debug")]
        [SerializeField] BasicSoundClipPlay_Common _soundCommon;


        private void OnEnable()
        {
            if(_soundCommon != null)
            {
                _soundCommon.ReturnToPool();
            }

            _soundCommon = SoundManager.Instance.RequestPlayAmbience(_ambienceSound, this.transform.position, isLoop: true, setFollowTarget: this.transform);

        }

        private void OnDisable()
        {
            if (_soundCommon == null)
                return;

            try
            {
                _soundCommon.ReturnToPool();

            }
            catch (System.NullReferenceException)
            {

            }

        }

        private void OnDestroy()
        {
            if (_soundCommon == null)
                return;

            try
            {
                _soundCommon.ReturnToPool();

            }
            catch(System.NullReferenceException)
            {

            }

        }
    }
}