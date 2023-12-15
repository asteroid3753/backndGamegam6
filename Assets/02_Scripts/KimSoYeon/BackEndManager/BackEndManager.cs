using BackEnd;
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

                instance.inGame.Init();
                instance.inGame.Init();
            }
        }

        private void Update()
        {
            StartCoroutine("BackEndPoll");
        }

        /// <summary>
        /// BackEnd �ۼ��� �̺�Ʈ�� ���� �ֱ������� ȣ���ؾ��ϴ� �Լ�
        /// </summary>
        /// <returns></returns>
        IEnumerator BackEndPoll()
        {
            while (true)
            {
                if (Backend.IsInitialized)
                {
                    // ó���� �̺�Ʈ ���� return
                    // BackEND �ۼ��� �̺�Ʈ�� ���� �� �ֱ������� ȣ���ؾ��� (������ �ۼ���)
                    Backend.Match.Poll();
                }

                // TODO : �ֱ⿡ ���� ���� ���庸��
                yield return new WaitForFixedUpdate();
            }
        }
    }
}

