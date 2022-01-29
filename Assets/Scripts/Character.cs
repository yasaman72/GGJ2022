using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public delegate void OnDied();
    public static OnDied onDied;

    [SerializeField] protected int maxHp = 10;

    protected int currentHP
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

    public void GetHit(int damageAmount)
    {
        currentHP -= damageAmount;
        CheckIfDied();
    }

    public void OnSpawn()
    {
        currentHP = maxHp;
    }

    private void CheckIfDied()
    {
        if (currentHP <= 0)
        {
            if (onDied != null)
            {
                onDied.Invoke();
            }
            Destroy(gameObject);
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
