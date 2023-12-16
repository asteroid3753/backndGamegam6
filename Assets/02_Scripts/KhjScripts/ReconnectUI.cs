using BackEnd;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace khj
{
    public class ReconnectUI : MonoBehaviour
    {
        public void Reconnect()
        {
            TotalGameManager.Instance.ChangeState(TotalGameManager.GameState.MatchLobby);
            MatchManager.Instance.Join();
        }

        public void Logout()
        {
            var bro = Backend.BMember.Logout();
            Destroy(MatchManager.Instance.gameObject);
            TotalGameManager.Instance.ChangeState(TotalGameManager.GameState.Login);
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}
