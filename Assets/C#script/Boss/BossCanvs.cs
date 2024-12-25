using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossCanvs : MonoBehaviour
{
    public Image BossHealthPurple;
    public TextMeshProUGUI Health;
    public float BossHealth;
    private float FullHealth;
    [Header("ÊÂ¼þ¼àÌý")]
    public FloatEventSO AttackBossEvent;
    public FloatEventSO GetBossHurtCountEvent;
    private void Awake()
    {
        FullHealth = BossHealth;
    }
    private void OnEnable()
    {
        AttackBossEvent.OnRaiseFloatEvent += OnAttackBoss;
        GetBossHurtCountEvent.OnRaiseFloatEvent += OnGetHurtCount;
    }

    private void OnGetHurtCount(float hurtCount)
    {
        if (BossHealth > 0)
        {
            BossHealth -= hurtCount;
            if(BossHealth <= 0)
            {
                BossHealth = 0;
            }
            Health.text = BossHealth.ToString();
        }
    }

    private void OnAttackBoss(float PlayerAttackCount)
    {
        if (BossHealthPurple.transform.localScale.x != 0)
        {
            var AttackHurt = PlayerAttackCount / FullHealth;
            BossHealthPurple.transform.localScale -= new Vector3(AttackHurt, 0, 0);
        }
        if (BossHealthPurple.transform.localScale.x <= 0)
        {
            BossHealthPurple.transform.localScale = new Vector3(0, 1, 1);
        }
    }

    private void OnDisable()
    {
        AttackBossEvent.OnRaiseFloatEvent -= OnAttackBoss;
        GetBossHurtCountEvent.OnRaiseFloatEvent -= OnGetHurtCount;
    }
}
