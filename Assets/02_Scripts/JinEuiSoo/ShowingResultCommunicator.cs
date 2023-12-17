using MorningBird.Sound;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JES
{
    public class ShowingResultCommunicator : MonoBehaviour
    {
        #region Singleton Settings

        private static ShowingResultCommunicator _instance;

        /// <summary>
        /// ShowingResultCommunicator
        /// </summary>
        public static ShowingResultCommunicator Instance
        {
            get
            {
                if (_instance == null)
                {
                    ShowingResultCommunicator obj = FindObjectOfType<ShowingResultCommunicator>();

                    if (obj != null)
                    {
                        _instance = obj;
                    }
                    else
                    {
                        var newSingleton = new GameObject("ShowingResultCommunicator").AddComponent<ShowingResultCommunicator>();
                        _instance = newSingleton;
                    }
                }
                return _instance;
            }
        }

        #endregion

        private void Awake()
        {
            #region Singleton Instantiate
            var objs = FindObjectsOfType<ShowingResultCommunicator>();
            if (objs.Length > 1)
            {
                Debug.LogError("New ShowingResultCommunicator Added And Destroy Automatically");
                Destroy(this.gameObject);
                return;
            }
            #endregion
        }


        public Dictionary<string, float> NickNameSlimeSizeRatioPair = new Dictionary<string, float>();
        public float LastSize;


        public void SetResults(Dictionary<string, float> nickNameSlimeSizeRatioPair, float lastSize)
        {
            NickNameSlimeSizeRatioPair = nickNameSlimeSizeRatioPair;
            LastSize = lastSize;
        }

        public Dictionary<string, float> GetResultInfo(out float lastSize)
        {
            lastSize = LastSize;
            return NickNameSlimeSizeRatioPair;
        }
    }

}