using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCanvs : MonoBehaviour
{
    public Image HealthGreen;
    public GameObject BlockBox;
    public TextMeshProUGUI Health;
    public float PlayerHealth;
    private float FullHealth;
    [Header("ÊÂ¼þ¼àÌý")]
    public FloatEventSO AttackPlayerEvent;
    public FloatEventSO GetPlayerHurtCountEvent;
    private void Awake()
    {
        FullHealth = PlayerHealth;
    }
    private void OnEnable()
    {
        AttackPlayerEvent.OnRaiseFloatEvent += OnAttackPlayer;
        GetPlayerHurtCountEvent.OnRaiseFloatEvent += OnGetHurtCount;
    }
    private void OnGetHurtCount(float HurtCount)
    {
        if (BlockBox.activeSelf == false)
        {
            if (PlayerHealth > 0)
            {
                PlayerHealth -= HurtCount;
                if (PlayerHealth < 0)
                {
                    PlayerHealth = 0;
                }
                Health.text = PlayerHealth.ToString();
            }
        }
    }
    private void OnAttackPlayer(float BossAttackCount)
    {
        if (BlockBox.activeSelf == false)
        {
            if (HealthGreen.transform.localScale.x != 0)
            {
                var AttackHurt = BossAttackCount / FullHealth;
                HealthGreen.transform.localScale -= new Vector3(AttackHurt, 0, 0);
            }
            if (HealthGreen.transform.localScale.x <= 0)
            {
                HealthGreen.transform.localScale = new Vector3(0, 1, 1);
            }
        }
    }

    private void OnDisable()
    {
        AttackPlayerEvent.OnRaiseFloatEvent -= OnAttackPlayer;
        GetPlayerHurtCountEvent.OnRaiseFloatEvent -= OnGetHurtCount;
    }
}
