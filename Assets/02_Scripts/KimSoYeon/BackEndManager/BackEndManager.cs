using BackEnd;
using BackEnd.Tcp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KSY
{
    public class BackEndManager : MonoBehaviour
    {
        #region instance
        private static BackEndManager instance;

        public static BackEndManager Instance { get { Init(); return instance; } }
        #endregion

        #region Manager Properties

        private BackEndInGame inGame = new BackEndInGame();
        public BackEndInGame InGame { get { return inGame; } }


        private ParsingManager parsing = new ParsingManager();
        public ParsingManager Parsing { get { return parsing; } }

        #endregion
        private static string managersName = "BackEndManager";

        private void Awake()
        {
            Init();
        }

        static void Init()
        {
            if (instance == null)
            {
                GameObject managerObj = GameObject.Find(managersName);

                if (managerObj == null)
                {
                    managerObj = new GameObject { name = managersName };
                    managerObj.AddComponent<BackEndManager>();
                }

                DontDestroyOnLoad(managerObj);
                instance = managerObj.GetComponent<BackEndManager>();
 
                instance.parsing.Init();
            }
        }

        private void Update()
        {
            if (Backend.IsInitialized)
            {
                Debug.Log("Poll");
                // 처리된 이벤트 갯수 return
                // BackEND 송수신 이벤트를 위해 꼭 주기적으로 호출해야함 (실질적 송수신)
                Backend.Match.Poll();
            }
        }


    }
}

