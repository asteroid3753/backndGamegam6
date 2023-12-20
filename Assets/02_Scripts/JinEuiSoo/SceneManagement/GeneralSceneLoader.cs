using System.Collections;
using TMPro;
using UnityEngine;

namespace MorningBird.SceneManagement
{
    public class GeneralSceneLoader : MonoBehaviour
    {
        [SerializeField] string[] sceneNames;
        [SerializeField] float _waitTime;
        [SerializeField] bool _requestUnloadAllScenes;

        // Use this for initialization
        void Start()
        {
            if (sceneNames.Length == 0)
            {
                Destroy(this.gameObject);
            }
            else
            {
                if (sceneNames[0].Length < 3)
                {
                    Destroy(this.gameObject);
                }
                else
                {
                    StartCoroutine(WaintOneFrameAndLoad());
                }
            }
            Debug.Log(UnityEngine.SceneManagement.SceneManager.GetSceneAt(0).path);
        }

        [SerializeField] bool _test;

        private void Update()
        {
            if(_test == true)
            {
                _test = false;
                StartCoroutine(WaintOneFrameAndLoad());
            }
        }

        public void LoadScene(bool isTransitionOn = true)
        {

            if (sceneNames.Length == 0)
                return;

            if (sceneNames[0].Length < 3)
                return;

            GameSceneLoadManager.Instance.LoadSceneAsync(sceneNames, isTransitionOn);
        }

        IEnumerator WaintOneFrameAndLoad()
        {
            yield return new WaitForSecondsRealtime(_waitTime);
            yield return new WaitForEndOfFrame();
            if(_requestUnloadAllScenes == true)
            {
                GameSceneLoadManager.Instance.UnLoadAllScenes();
            }

            GameSceneLoadManager.Instance.LoadSceneAsync(sceneNames);

            Destroy(this.gameObject);
        }
    }
}