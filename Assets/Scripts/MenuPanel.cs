using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MenuPanel : MonoBehaviour
{
    [SerializeField] private UnityEvent onHide;
    [SerializeField] private UnityEvent onShow;

    public void Hide()
    {
        gameObject.SetActive(false);
        onHide.Invoke();
    }

    public void Show()
    {
        gameObject.SetActive(true);
        onShow.Invoke();
    }
}
