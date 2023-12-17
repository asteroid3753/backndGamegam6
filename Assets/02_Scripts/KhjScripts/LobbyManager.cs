using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using BackEnd.Game;
using UnityEngine.UI;
using TMPro;
using KSY;

namespace khj
{
    public class LobbyManager : MonoBehaviour
    {
        [SerializeField] GameObject titlePanel;
        [SerializeField] GameObject loginPanel;
        [SerializeField] TMP_InputField nameInputField;
        string nickName;

        void Awake()
        {
            titlePanel.SetActive(true);
            loginPanel.SetActive(false);
        }

        private void Start()
        {
            var bro = Backend.Initialize();
            BackEndManager.Instance.InGame.Init();

        }

        public void TouchToStart()
        {
            loginPanel.SetActive(true);
        }

        public void Login()
        {
            nickName = nameInputField.text;
            var bro = Backend.BMember.CustomLogin(nickName, nickName);

            if (bro.IsSuccess())
            {
                Debug.Log("�α��� : " + bro);
            }
            else if (bro.GetStatusCode() == "401")
            {
                SignUp();
                var broTwo = Backend.BMember.CustomLogin(nickName, nickName);

                Debug.Log("�α��� : " + broTwo);
            }
            else
            {
                Debug.LogError("�α��� : " + bro);
            }

            StartMatch();
        }

        void StartMatch()
        {
            TotalGameManager.Instance.myNickName = nickName;
            TotalGameManager.Instance.ChangeState(TotalGameManager.GameState.MatchLobby);
        }
        void SignUp()
        {
            var bro = Backend.BMember.CustomSignUp(nickName, nickName);

            if (!bro.IsSuccess())
            {
                Debug.LogError("ȸ������: " + bro);
            }

            bro = Backend.BMember.UpdateNickname(nickName);

            if (!bro.IsSuccess())
            {
                Debug.LogError("�г��� ���� : " + bro);
            }

        }
    }
}