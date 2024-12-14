using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
    [Header("玩家血量")]
    private bool IsDead;
    public float Fullhealth;
    public float Currenthealth;
    [Header("事件监听")]
    public FloatEventSO AttackBossEvent;
    private void Awake()
    {
        Currenthealth = Fullhealth;
    }
    private void OnEnable()
    {
        AttackBossEvent.OnRaiseFloatEvent += OnAttackBoss;
    }

    private void OnAttackBoss(float AttackCount)
    {
        Currenthealth -= AttackCount;
    }

    private void OnDisable()
    {
        AttackBossEvent.OnRaiseFloatEvent -= OnAttackBoss;
    }
}
