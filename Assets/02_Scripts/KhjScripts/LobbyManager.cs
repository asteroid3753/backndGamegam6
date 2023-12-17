using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using BackEnd.Game;
using UnityEngine.UI;
using TMPro;

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
        }

        public void TouchToStart()
        {
            titlePanel.SetActive(false);
            loginPanel.SetActive(true);
        }

        public void Login()
        {
            nickName = nameInputField.text;
            var bro = Backend.BMember.CustomLogin(nickName, nickName);

            if (bro.IsSuccess())
            {
                Debug.Log("로그인 : " + bro);
                StartMatch();
            }
            else if (bro.GetStatusCode() == "401")
            {
                SignUp();
                StartMatch();
            }
            else
            {
                Debug.LogError("로그인 : " + bro);
            }
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
                Debug.LogError("회원가입: " + bro);
            }

            bro = Backend.BMember.UpdateNickname(nickName);

            if (!bro.IsSuccess())
            {
                Debug.LogError("닉네임 변경 : " + bro);
            }
        }
    }
}