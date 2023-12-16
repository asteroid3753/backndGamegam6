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

        // TODO : 임시 데이터 나중에 삭제
        public Dictionary<SessionId, string> players;

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

                instance.inGame.Init();
                instance.parsing.Init();
            }
        }

        private void Update()
        {
            StartCoroutine(BackEndPoll());
        }

        /// <summary>
        /// BackEnd 송수신 이벤트를 위해 주기적으로 호출해야하는 함수
        /// </summary>
        /// <returns></returns>
        IEnumerator BackEndPoll()
        {
            while (true)
            {
                if (Backend.IsInitialized)
                {
                    // 처리된 이벤트 갯수 return
                    // BackEND 송수신 이벤트를 위해 꼭 주기적으로 호출해야함 (실질적 송수신)
                    Backend.Match.Poll();
                }

                // TODO : 주기에 대한 설명 여쭤보기
                yield return new WaitForFixedUpdate();
            }
        }
    }
}

