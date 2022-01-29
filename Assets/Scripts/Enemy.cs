using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    [SerializeField] private Transform hpBar;

    public override void UpdateHPUI()
    {
        hpBar.localScale = new Vector2((currentHP/(float)maxHp) *  hpBar.localScale.x, hpBar.localScale.y);
    }
}
