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
                Debug.Log("�α��� : " + bro);
                GameManager.Instance.ChangeState(GameManager.GameState.MatchLobby);
            }
            else if (bro.GetStatusCode() == "401")
            {
                SignUp();
                GameManager.Instance.ChangeState(GameManager.GameState.MatchLobby);
            }
            else
            {
                Debug.LogError("�α��� : " + bro);
            }
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