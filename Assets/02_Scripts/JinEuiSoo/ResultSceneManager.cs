using MorningBird.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JES
{
    public class ResultSceneManager : MonoBehaviour
    {
        public void BackToLogin()
        {
            GameSceneLoadManager.Instance.UnLoadAllScenes();
            GameSceneLoadManager.Instance.LoadSceneAsync("Match");
        }

        public void Quit()
        {
            Debug.LogWarning("Quiting Application");
            Application.Quit();
        }
    } 
}
