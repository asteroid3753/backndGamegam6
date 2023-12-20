using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
// using MorningBird.UI;
using Sirenix.OdinInspector;
using TransitionsPlus;
using MorningBird.Sound;
using System.Threading.Tasks;
using UnityEngine.Events;

namespace MorningBird.SceneManagement
{
    public class GameSceneLoadManager : SerializedMonoBehaviour
    {
        [Title("BasicSettings"), SerializeField] string[] preserveSceneList;
        Scene thisScene;

        #region Singleton
        
        static GameSceneLoadManager _instance;
        /// <summary>
        /// TotalGameSceneManager
        /// </summary>
        internal static GameSceneLoadManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    var obj = FindObjectOfType<GameSceneLoadManager>();

                    if (obj != null)
                    {
                        _instance = obj;
                    }
                    else
                    {
                        var newSingleton = GameObject.Find("TotalGameManager").GetComponent<GameSceneLoadManager>();
                        _instance = newSingleton;
                    }
                }
                return _instance;
            }
        }

        private void Awake()
        {
            #region Singleton Instantiate
            var objs = FindObjectsOfType<SoundManager>();
            if (objs.Length > 1)
            {
                Debug.LogError("New SoundManager Added And Destroy Automatically");
                Destroy(this.gameObject);
                return;
            }
            #endregion
            AwakeInitialize();
        }

        #endregion

        #region TestSettings
#if UNITY_EDITOR
        [FoldoutGroup("Test Settings_It will disable when game is build"), SerializeField] bool _testLoading;
        [FoldoutGroup("Test Settings_It will disable when game is build"), SerializeField] bool _testUnLoading;
        [FoldoutGroup("Test Settings_It will disable when game is build"), SerializeField] string[] _testStringArr;
#endif
        #endregion

        #region Conditions And Debug Variables

        [FoldoutGroup("Conditions"), SerializeField] List<AsyncOperation> sceneLoadingList = new List<AsyncOperation>();
#pragma warning disable CS0414 // 사용되지 않은 변수에 대한 경고 끄기
        [FoldoutGroup("Conditions"), SerializeField] public bool _isAllScenesLoaded;
#pragma warning restore CS0414 // 사용되지 않은 변수에 대한 경고 끄기
        [FoldoutGroup("Transition Controll")] [SerializeField] SceneTransitionManager _transitionSceneManager;

        #endregion

        #region Transition

        //[FoldoutGroup("Transition"), SerializeField] UnityEvent _onTransitionEnded;

        #endregion

        void AwakeInitialize()
        {
            // Find this Manager Scene
            {
                Scene[] scenes = new Scene[SceneManager.sceneCount];

                // GetCurrentOpenedScenes
                {
                    for (int ia = 0; ia < scenes.Length; ia++)
                    {
                        scenes[ia] = SceneManager.GetSceneAt(ia);
                    }
                }

                foreach (Scene scene in scenes)
                {
                    if (scene.name == preserveSceneList[0])
                    {
                        thisScene = scene;
                        break;
                    }
                }
            }

        }

        private void FixedUpdate()
        {
#if UNITY_EDITOR
            if (_testLoading)
            {
                _testLoading = false;
                LoadSceneAsync(_testStringArr);
            }

            if (_testUnLoading)
            {
                _testUnLoading = false;
                UnLoadSceneAsync(_testStringArr);
            }
#endif
        }

        void SetSceneLoadBoolToFalse()
        {

#if UNITY_EDITOR
            Debug.Log("Set SceneLoad bool to false"); 
#endif
            _isAllScenesLoaded = false;
        }

        void SetSceneLoadBoolToTrue()
        {
            _isAllScenesLoaded = true;
        }

        Scene[] GetCurrentOpenedScenes()
        {
            Scene[] scenes = new Scene[SceneManager.sceneCount];

            // GetCurrentOpenedScenes
            {
                for (int ia = 0; ia < scenes.Length; ia++)
                {
                    scenes[ia] = SceneManager.GetSceneAt(ia);
                }
            }
            return scenes;
        }

        #region Transition

        async Task StartTransition(TransitionType transitionType)
        {
            await _transitionSceneManager.StartTransition(transitionType);
        }

        async Task WaitEndTransition()
        {
            await Task.Run(async () =>
            {
                while (_isAllScenesLoaded == false)
                {
                    await Task.Delay(250);
                }
            });

            await StartTransition(TransitionType.FadeOut_TurnToWhite);

        }

        #endregion

        IEnumerator WaitUntilScenesLoadedAndSetActiveScene(string sceneName)
        {

            while (sceneLoadingList.Count > 0)
            {
                yield return new WaitForSeconds(0.25f);
#if UNITY_EDITOR
                Debug.Log($"SceneLoading Screen keeping On");
#endif

                float totalRatioOfPregress = new();
                int preserveScenesCount = sceneLoadingList.Count;
                for (int i = 0; i < sceneLoadingList.Count; i++)
                {
                    AsyncOperation operation = sceneLoadingList[i];
                    totalRatioOfPregress += operation.progress;

                    if (operation.isDone == true)
                    {
                        sceneLoadingList.Remove(operation);
                    }
                }

                // Check Total Progress
                if (sceneLoadingList.Count == 0)
                {
                    totalRatioOfPregress = 1;
                }
                else
                {
                    totalRatioOfPregress = totalRatioOfPregress / preserveScenesCount;
                }
#if UNITY_EDITOR
                Debug.Log($"Scene Loading {totalRatioOfPregress} Complite.");
#endif
            }
#if UNITY_EDITOR


            Debug.Log($"SceneLoading Screen Turn Off");
#endif
            SetSceneLoadBoolToTrue();
            sceneLoadingList.Clear();
            SetCenterOfScenes(sceneName);

        }

        public async void LoadSceneAsync(string sceneName, bool isUseTransitionEffect = true)
        {
            if (isUseTransitionEffect == true)
            {
                await StartTransition(TransitionType.FadeIn_TurnToBlack);
            }

            sceneLoadingList.Add(SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive));

            SetSceneLoadBoolToFalse();
            StartCoroutine(WaitUntilScenesLoadedAndSetActiveScene(sceneName));

            if (isUseTransitionEffect == true)
            {
                await WaitEndTransition();
            }
        }

        public async void LoadSceneAsync(string[] sceneName, bool isUseTransitionEffect = true)
        {
            if (isUseTransitionEffect == true)
            {
                await StartTransition(TransitionType.FadeIn_TurnToBlack);
            }

            for (int i = 0; i < sceneName.Length; i++)
            {
                var name = sceneName[i];
                sceneLoadingList.Add(SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive));
            }

            SetSceneLoadBoolToFalse();
            StartCoroutine(WaitUntilScenesLoadedAndSetActiveScene(sceneName[0]));

            if (isUseTransitionEffect == true)
            {
                await WaitEndTransition();
            }
        }

        public async void UnLoadSceneAsync(string sceneName, bool isUseTransitionEffect = false)
        {

            if (isUseTransitionEffect == true)
            {
                await StartTransition(TransitionType.FadeIn_TurnToBlack);
            }


            SceneManager.UnloadSceneAsync(sceneName);
            SetSceneLoadBoolToFalse();

            if (isUseTransitionEffect == true)
            {
                await WaitEndTransition();
            }
        }

        public async void UnLoadSceneAsync(string[] sceneName, bool isUseTransitionEffect = false)
        {
            if (isUseTransitionEffect == true)
            {
                await StartTransition(TransitionType.FadeIn_TurnToBlack);
            }

            foreach (var scene in sceneName)
            {
                SceneManager.UnloadSceneAsync(scene);
            }

            SetSceneLoadBoolToFalse();

            if (isUseTransitionEffect == true)
            {
                await WaitEndTransition();
            }
        }

        public async void UnLoadAllScenes(string[] exceptingScene = null, bool isUseTransitionEffect = false)
        {
            if (isUseTransitionEffect == true)
            {
                await StartTransition(TransitionType.FadeIn_TurnToBlack);
            }


            Scene[] scenes = GetCurrentOpenedScenes();

            List<string> sceneList = new List<string>();

            foreach (Scene scene in scenes)
            {
                sceneList.Add(scene.name);
            }

            sceneList.Remove(thisScene.name);

            if(exceptingScene != null)
            {
                foreach (var name in exceptingScene)
                {
                    sceneList.Remove(name);
                }
            }


            foreach (var name in preserveSceneList)
            {
                sceneList.Remove(name);
            }

            UnLoadSceneAsync(sceneList.ToArray());


            if (isUseTransitionEffect == true)
            {
                await WaitEndTransition();
            }

        }

        public async void UnLoadAllSceneAndLoadSceneAsync(string sceneName, string[] exceptingScene = null, bool isUseTransitionEffect = false)
        {
            if (isUseTransitionEffect == true)
            {
                await StartTransition(TransitionType.FadeIn_TurnToBlack);
            }


            UnLoadAllScenes(exceptingScene);
            LoadSceneAsync(sceneName, false);


            if (isUseTransitionEffect == true)
            {
                await WaitEndTransition();
            }

        }

        public async void UnLoadAllSceneAndLoadSceneAsync(string[] sceneNames, string[] exceptingScene = null, bool isUseTransitionEffect = false)
        {
            if (isUseTransitionEffect == true)
            {
                await StartTransition(TransitionType.FadeIn_TurnToBlack);
            }


            UnLoadAllScenes(exceptingScene);
            LoadSceneAsync(sceneNames, false);


            if (isUseTransitionEffect == true)
            {
                await WaitEndTransition();
            }
        }

        public async void UnLoadAllSceneAndLoadSceneAsync(GroupSceneLoader sceneLoaderClass, bool isUseTransitionEffect = false)
        {
            if (isUseTransitionEffect == true)
            {
                await StartTransition(TransitionType.FadeIn_TurnToBlack);
            }


            UnLoadAllScenes(isUseTransitionEffect : false);
            sceneLoaderClass.LoadScene(false);


            if (isUseTransitionEffect == true)
            {
                await WaitEndTransition();
            }
        }

        public async void UnLoadAllSceneAndLoadSceneAsync(GeneralSceneLoader sceneLoaderClass, bool isUseTransitionEffect = false)
        {
            if (isUseTransitionEffect == true)
            {
                await StartTransition(TransitionType.FadeIn_TurnToBlack);
            }


            UnLoadAllScenes(isUseTransitionEffect : false);
            sceneLoaderClass.LoadScene(false);


            if (isUseTransitionEffect == true)
            {
                await WaitEndTransition();
            }
        }

        public void SetCenterOfScenes(string sceneName)
        {
            Scene[] scenes = new Scene[SceneManager.sceneCount];

            // GetCurrentOpenedScenes
            {
                for (int ia = 0; ia < scenes.Length; ia++)
                {
                    scenes[ia] = SceneManager.GetSceneAt(ia);
                }
            }

            // Find sceneName For Set Active Scene.
            for (int ia = 0; ia < scenes.Length; ia++)
            {
                if (scenes[ia].name == sceneName)
                {
                    SceneManager.SetActiveScene(scenes[ia]);
                    return;
                }
            }

            Debug.LogError("Check SceneName. Set active scene is failed.");
            return;
        }


    }
}
