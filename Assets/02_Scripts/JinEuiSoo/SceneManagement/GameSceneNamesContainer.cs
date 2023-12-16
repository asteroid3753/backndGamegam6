using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;

namespace MorningBird.SceneManagement
{
    public class GameSceneNamesContainer : SerializedMonoBehaviour
    {
        #region Singleton Instance

        static GameSceneNamesContainer gameSceneNamesContainer;
        /// <summary>
        /// GameSceneNamesContainer
        /// </summary>
        public static GameSceneNamesContainer Instance
        {
            get
            {
                if (gameSceneNamesContainer == null)
                {
                    var obj = FindObjectOfType<GameSceneNamesContainer>();

                    if (obj != null)
                    {
                        gameSceneNamesContainer = obj;
                    }
                    else
                    {
                        var newSingleton = GameObject.Find("TotalGameController").GetComponent<GameSceneNamesContainer>();
                        gameSceneNamesContainer = newSingleton;
                    }
                }
                return gameSceneNamesContainer;
            }
        }

        private void Awake()
        {
            var obj = FindObjectsOfType<GameSceneNamesContainer>();
            if (obj.Length > 1)
            {
                Destroy(this);
                Debug.LogError("GameSceneNamesContainer :: instantiate another singleton object. So Destory instance");
            }

            AwakeInitialize();
        }

        #endregion

        private void AwakeInitialize()
        {
           _sceneNames = gameSceneLoaderStringPairs.Keys.ToArray<string>();
        }

        [System.Serializable]
        struct GameObjects
        {
            public GameObject[] objs;
        }

        [SerializeField] string[] _sceneNames;
        [SerializeField] Dictionary<string, GameObjects> gameSceneLoaderStringPairs = new Dictionary<string, GameObjects>();

        public string[] SceneNames => gameSceneLoaderStringPairs.Keys.ToArray<string>();

        public int GetSceneLoaderAmount(string key)
        {
            if (gameSceneLoaderStringPairs.TryGetValue(key, out GameObjects value) == false)
                return 0;

            return value.objs.Length;
        }

        public bool TryGetSceneLoadGO(string sceneKey, int sceneNumber, out GameObject nullAbleSceneLoader)
        {
            GameObjects sceneList;
            bool isKeyExist = gameSceneLoaderStringPairs.TryGetValue(sceneKey, out sceneList);

            GameObject returnObject = null;
            try
            {
                returnObject = sceneList.objs[sceneNumber];
            }
            catch(System.IndexOutOfRangeException)
            {
                returnObject = null;
            }


            nullAbleSceneLoader = returnObject;
            if(nullAbleSceneLoader == null)
            {
                isKeyExist = false;
            }

            return isKeyExist;
        }

        public bool TryGetSceneLoadGO(int sceneKey, int sceneNumber, out GameObject nullAbleSceneLoader)
        {
            GameObjects sceneList = new GameObjects();
            bool isKeyExist = false;
            try
            {
                isKeyExist = gameSceneLoaderStringPairs.TryGetValue(_sceneNames[sceneKey], out sceneList);
            }
            catch(System.IndexOutOfRangeException)
            {
                isKeyExist = false;
                gameSceneLoaderStringPairs.TryGetValue(_sceneNames[sceneKey], out sceneList);
            }

            GameObject returnObject = null;
            try
            {
                returnObject = sceneList.objs[sceneNumber];
            }
            catch (System.IndexOutOfRangeException)
            {
                returnObject = null;
            }


            nullAbleSceneLoader = returnObject;
            if (nullAbleSceneLoader == null)
            {
                isKeyExist = false;
            }

            return isKeyExist;
        }
    }

}

