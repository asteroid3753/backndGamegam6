using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using BackEnd.Game;
using UnityEngine.UI;
using TMPro;
using KSY;
using Sirenix.Utilities;
using System.Text.RegularExpressions;

namespace khj
{
    public class LobbyManager : MonoBehaviour
    {
        [SerializeField] GameObject titlePanel;
        [SerializeField] GameObject loginPanel;
        [SerializeField] TMP_InputField nameInputField;
        [SerializeField] LoginInfoText loginInfoText;
        [SerializeField] int idLength = 10;
        string nickName;

        [SerializeField] bool _isLoginButtonClicked = false;

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
            if(_isLoginButtonClicked == true)
            {
                return;
            }

            Regex regex = new Regex(@"^[0-9a-zA-Z가-힝]{1,"+ idLength + @"}$");
            if (!regex.IsMatch(nameInputField.text)) 
            {
                loginInfoText.InfoText = $"1~{idLength}사이의 영문, 한글 혹은 숫자로 이루어진 아이디를 입력해주세요";
               // Debug.Log($"1~{idLength}사이의 영문 혹은 숫자로 이루어진 아이디를 입력해주세요");
                return;
            }

            _isLoginButtonClicked = true;

            nickName = nameInputField.text;
            var bro = Backend.BMember.CustomLogin(nickName, nickName);

            if (bro.IsSuccess())
            {
                Debug.Log("로그인 : " + bro);
            }
            else if (bro.GetStatusCode() == "401")
            {
                SignUp();
                var broTwo = Backend.BMember.CustomLogin(nickName, nickName);

                Debug.Log("로그인 : " + broTwo);
            }
            else
            {
                Debug.LogError("로그인 : " + bro);
            }

            StartMatch();
        }

        void StartMatch()
        {
            TotalGameManager.Instance.MyClientNickName = nickName;
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

        public void SubmitName(string name){
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                Login();
            }

        }
    }
}