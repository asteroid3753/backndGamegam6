using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditShower : MonoBehaviour
{
    [SerializeField] GameObject obj;
    [SerializeField] bool _isShow;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowCredit();
        }
    }

    public void ShowCredit()
    {
        _isShow = !_isShow;

        obj.SetActive(_isShow);

    }
}
