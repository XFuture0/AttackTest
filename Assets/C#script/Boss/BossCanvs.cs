using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossCanvs : MonoBehaviour
{
    public Image BossHealthPurple;
    public float BossHealth;
    [Header("ÊÂ¼þ¼àÌý")]
    public FloatEventSO AttackBossEvent;
    private void OnEnable()
    {
        AttackBossEvent.OnRaiseFloatEvent += OnAttackBoss;
    }

    private void OnAttackBoss(float AttackCount)
    {
        if (BossHealthPurple.transform.localScale.x != 0)
        {
            var AttackHurt = AttackCount / BossHealth;
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
    }
}
