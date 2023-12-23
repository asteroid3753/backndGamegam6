using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MorningBird.Sound
{
    public class AmbienceSoundCreator_Common : SerializedMonoBehaviour
    {
        [SerializeField, FoldoutGroup("PreDefine")] AudioStorage[] _ambienceSound;
        [SerializeField, FoldoutGroup("PreDefine")] bool _stopWhenDisalbed = true;
        [SerializeField, FoldoutGroup("PreDefine")] float _delay;

        [SerializeField, FoldoutGroup("Debug")] List<BasicSoundClipPlay_Common> _soundCommons;

        SoundManager _soundManager;

        private void Start()
        {
            _soundManager = SoundManager.Instance;
            _soundCommons = new List<BasicSoundClipPlay_Common>(_ambienceSound.Length);

            StartCoroutine(WaitAndStart());
        }

        IEnumerator WaitAndStart()
        {
            yield return new WaitForSeconds(_delay);

            for (int i = 0; i < _ambienceSound.Length; i++)
            {
                _soundCommons.Add(_soundManager.RequestPlayAmbience(_ambienceSound[i], this.transform.position));
            }
        }

        private void OnDisable()
        {
            if(_stopWhenDisalbed == true)
            {
                foreach (var item in _soundCommons)
                {
                    item.ReturnToPool();
                }
            }
        }
    }

}