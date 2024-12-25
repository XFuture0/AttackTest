using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class CaremaImpulse : MonoBehaviour
{
    public CinemachineImpulseSource impulse;
    [Header("ÊÂ¼þ¼àÌý")]
    public FloatEventSO AttackBossEvent;
    public FloatEventSO AttackPlayerEvent;
    public VoidEventSO BlockBossEvent;
    private void OnEnable()
    {
        AttackBossEvent.OnRaiseFloatEvent += OnAttackBoss;
        AttackPlayerEvent.OnRaiseFloatEvent += OnAttackPlayer;
        BlockBossEvent.OnRaiseEvent += OnBlockBoss;
    }

    private void OnAttackPlayer(float HurtCount)
    {
        impulse.m_DefaultVelocity = new Vector3(0.2f, 0.2f, 0.2f);
        impulse.GenerateImpulse();
    }

    private void OnBlockBoss()
    {
        impulse.m_DefaultVelocity = new Vector3(0.2f, 0.2f, 0.2f);
        impulse.GenerateImpulse();
    }

    private void OnAttackBoss(float HurtCount)
    {
        if(HurtCount > 15)
        {
            impulse.m_DefaultVelocity = new Vector3(0.2f, 0.2f, 0.2f);
        }
        else
        {
            impulse.m_DefaultVelocity = new Vector3(0.05f, 0.05f, 0.05f);
        }
        impulse.GenerateImpulse();
    }

    private void OnDisable()
    {
        AttackBossEvent.OnRaiseFloatEvent += OnAttackBoss;
        AttackPlayerEvent.OnRaiseFloatEvent += OnAttackPlayer;
        BlockBossEvent.OnRaiseEvent -= OnBlockBoss;
    }
}
