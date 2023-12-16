using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JES
{
    public class DontDestroyOnLoadGO : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }

}