using Google.Protobuf.WellKnownTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JES
{
    public class TestItem : MonoBehaviour
    {
        [SerializeField] bool _isDisabled;
        [SerializeField] int _value;

        public int GetIndexValue()
        {
            return _value;
        }

        public void SetIndexValue(int value) => _value = value;

        public bool SetIsDisalbed
        {
            set { _isDisabled = value; }
        }

        public bool IsDisalbed => _isDisabled;
    }

}