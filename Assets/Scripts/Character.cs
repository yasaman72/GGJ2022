using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private Transform hpBar;

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
            OnDied();
            Destroy(gameObject);
        }
    }

    protected void OnDied()
    {

    }

    public int GetHP()
    {
        return currentHP;
    }

    public void SetCurrentHP(int newHP)
    {
        currentHP = newHP;
    }

    protected void UpdateHPUI()
    {
        hpBar.localScale = new Vector2((currentHP / (float)maxHp) * hpBar.localScale.x, hpBar.localScale.y);
    }
}
