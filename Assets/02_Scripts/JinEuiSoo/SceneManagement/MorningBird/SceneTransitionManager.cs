using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TransitionsPlus;
using UnityEngine.Events;
using System.Threading.Tasks;
using UnityEngine.Rendering.Universal;

namespace MorningBird.SceneManagement
{
    public enum TransitionType : sbyte
    {
        None,
        FadeIn_TurnToBlack,
        FadeOut_TurnToWhite,
    }

    public class SceneTransitionManager : MonoBehaviour
    {
        [SerializeField] TransitionAnimator _transitionAnimator;

        [SerializeField] TransitionProfile _fadeInProfile;
        [SerializeField] TransitionProfile _fadeOutProfile;

        [SerializeField] bool _isTransitionStarted = false;
        public bool IsTransitionStarted => _isTransitionStarted;

        private void Awake()
        {
            _transitionAnimator.progress = 0f;
            _transitionAnimator.progress = 0f;
        }

        public void QuitTransitionImmediatly()
        {
            _transitionAnimator.SetProgress(1.1f);
        }

        public async Task StartTransition(TransitionType transitionType)
        {

            // Check Transition is on going And Ealry Return
            {
                if (_isTransitionStarted == true)
                {
#if UNITY_EDITOR
                    //Debug.Log("Currently, Transition is on going");
#endif
                    return;
                }
            }

#if UNITY_EDITOR
            Debug.Log($"SceneLoading Screen Turn On");
#endif

            // Initialize Transition
            {
                _transitionAnimator.enabled = true;
                _transitionAnimator.onTransitionEnd.RemoveAllListeners();
                _isTransitionStarted = true;

                _transitionAnimator.onTransitionEnd.AddListener(() =>
                {
                    _isTransitionStarted = false;

                });
            }

            // Start Transition
            {
                switch (transitionType)
                {
                    case TransitionType.FadeIn_TurnToBlack:
                        OnTransition();
                        break;
                    case TransitionType.FadeOut_TurnToWhite:
                        OffTransition();
                        _transitionAnimator.onTransitionEnd.AddListener(() =>
                        {
                            _transitionAnimator.profile = _fadeOutProfile;
                        });
                        break;
                    case TransitionType.None:
                    default:
                        Debug.LogAssertion("Unrecognizing Type");
                        return;
                }

            }

            // Check Condition
            await CheckTransitionConditionAndDisable();
        }
        
        public void SetProfile(TransitionType transitionType)
        {
            _transitionAnimator.enabled = true;

            TransitionProfile profile = _fadeInProfile;

            switch (transitionType)
            {
                case TransitionType.FadeIn_TurnToBlack:
                    profile = _fadeInProfile;
                    break;
                case TransitionType.FadeOut_TurnToWhite:
                    profile = _fadeOutProfile;
                    break;
                case TransitionType.None:
                default:
                    Debug.LogAssertion("Invalid Type");
                    break;
            }

            _transitionAnimator.SetProfile(profile);
            _transitionAnimator.enabled = false;
        }

        void OnTransition()
        {
            _transitionAnimator.StartTransition(_fadeInProfile);

        }

        void OffTransition()
        {
            _transitionAnimator.StartTransition(_fadeOutProfile);
        }

        async Task CheckTransitionConditionAndDisable()
        {

            // Check Progress
            {
                await Task.Run(async () =>
                {
                    while (_isTransitionStarted == true)
                    {
#if UNITY_EDITOR
                        //Debug.Log("Transition is keeping going");
#endif

                        await Task.Delay(250);
                    }
                });

#if UNITY_EDITOR
                Debug.Log($"SceneLoading Screen Turn Off");
#endif
            }

            // Disable
            {
                _transitionAnimator.enabled = false;
                _transitionAnimator.onTransitionEnd.RemoveAllListeners();

            }
            
        }
    }
}