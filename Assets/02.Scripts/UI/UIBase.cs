using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class UIBase : MonoBehaviour
{
    public eUIPosition uiPosition;
    public UnityAction<object[]> opened;
    public UnityAction<object[]> closed;

    private void Awake()
    {
        opened = Opened;
        closed = Closed;
    }

    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    public virtual void HideDirect() { }

    public virtual void Opened(object[] param) { }

    public virtual void Closed(object[] param) { }
}