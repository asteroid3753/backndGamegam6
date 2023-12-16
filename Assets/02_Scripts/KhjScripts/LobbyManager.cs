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
        [SerializeField] GameObject loadingPanel;
        [SerializeField] GameObject matchPanel;
        [SerializeField] GameObject matchManager;
        [SerializeField] GameObject roomPanel;
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
        public void MatchStart()
        {
            loginPanel.SetActive(false);
            matchPanel.SetActive(true);
            matchManager.SetActive(true);
        }

        public void Login()
        {
            nickName = nameInputField.text;
            var bro = Backend.BMember.CustomLogin(nickName, nickName);

            if (bro.IsSuccess())
            {
                Debug.Log("�α��� : " + bro);
                MatchStart();
            }
            else if (bro.GetStatusCode() == "401")
            {
                SignUp();
                MatchStart();
            }
            else
            {
                Debug.LogError("�α��� : " + bro);
            }
        }

        void SignUp()
        {
            var bro = Backend.BMember.CustomSignUp(nickName, nickName);

            if (bro.IsSuccess())
            {
                Debug.Log("ȸ������: " + bro);
            }
            else
            {
                Debug.LogError("ȸ������: " + bro);
            }
        }

        public void UpdateNickname()
        {
            var bro = Backend.BMember.UpdateNickname(nickName);

            if (bro.IsSuccess())
            {
                Debug.Log("�г��� ���� : " + bro);
            }
            else
            {
                Debug.LogError("�г��� ���� : " + bro);
            }
        }
    }
}