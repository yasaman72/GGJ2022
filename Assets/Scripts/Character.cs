using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public delegate void OnDied();
    public static OnDied onDied;

    [SerializeField] private int hp;
    private int currentHP
    {
        get
        {
            return _currentHP;
        }
        set
        {
            _currentHP = value;
            UpdateHPUI();
        }
    }
    private int _currentHP;

    public void OnGetHurt(int damageAmount)
    {
        currentHP -= damageAmount;
        CheckIfDied();
    }

    private void CheckIfDied()
    {
        if(currentHP <= 0)
        {
            if(onDied != null)
            {
                onDied.Invoke();
            }
        }
    }

    public int GetHP()
    {
        return currentHP;
    }

    public void SetCurrentHP(int newHP)
    {
        currentHP = newHP;
    }

    public virtual void UpdateHPUI()
    {
    }
}
