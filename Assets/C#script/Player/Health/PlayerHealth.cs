using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private PlayerAnim playerAnim;
    [Header("玩家血量")]
    private bool IsDead;
    public float Fullhealth;
    public float Currenthealth;
    [Header("事件监听")]
    public FloatEventSO AttackPlayerEvent;
    private void Awake()
    {
        Currenthealth = Fullhealth;
        playerAnim = GetComponent<PlayerAnim>();
    }
    private void Update()
    {
        if(Currenthealth <= 0)
        {
            if (!IsDead)
            {
                IsDead = true;
                PlayerDead();
            }
        }
    }
    private void PlayerDead()
    {
        playerAnim.PlayerisDead();
    }
    private void OnEnable()
    {
        AttackPlayerEvent.OnRaiseFloatEvent += OnAttackPlayer;
    }

    private void OnAttackPlayer(float BossAttackCount)
    {
        Currenthealth -= BossAttackCount;
    }

    private void OnDisable()
    {
        AttackPlayerEvent.OnRaiseFloatEvent -= OnAttackPlayer;
    }
}
