using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCanvs : MonoBehaviour
{
    public Image HealthGreen;
    [Header("ÊÂ¼þ¼àÌý")]
    public FloatEventSO AttackPlayerEvent;
    private void OnEnable()
    {
        AttackPlayerEvent.OnRaiseFloatEvent += OnAttackPlayer;
    }

    private void OnAttackPlayer(float BossAttackCount)
    {
        if (HealthGreen.transform.localScale.x != 0)
        {
            var AttackHurt = BossAttackCount / 100f;
            HealthGreen.transform.localScale -= new Vector3(AttackHurt, 0, 0);
        }
        if(HealthGreen.transform.localScale.x <= 0)
        {
            HealthGreen.transform.localScale = new Vector3(0, 1, 1);
        }
    }

    private void OnDisable()
    {
        AttackPlayerEvent.OnRaiseFloatEvent -= OnAttackPlayer;
    }
}
