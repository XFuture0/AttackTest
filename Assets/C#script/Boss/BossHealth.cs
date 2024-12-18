using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
    private MainBoss Boss;
    private Bossanim anim;
    [Header("受击特效")]
    public float AdjustY;
    public GameObject BeAttackEffect;
    public GameObject BeAttackEffect_Box;
    [Header("玩家血量")]
    private bool IsDead;
    public float Fullhealth;
    public float Currenthealth;
    [HideInInspector]public bool IsBeHurt;
    public int BeHurtCount;
    [Header("事件监听")]
    public FloatEventSO AttackBossEvent;
    private void Awake()
    {
        anim = GetComponent<Bossanim>();
        Boss = GetComponent<MainBoss>();
        Currenthealth = Fullhealth;
        BeHurtCount = 0;
    }
    private void OnEnable()
    {
        AttackBossEvent.OnRaiseFloatEvent += OnAttackBoss;
    }

    private void OnAttackBoss(float AttackCount)
    {
        anim.OnBossBeHurt();
        Boss.GetPlayerPo();
        var PlayerFace = Boss.Player;
        var HitPosition = new Vector3(transform.position.x, transform.position.y + AdjustY, transform.position.z);
        if(PlayerFace.localScale.x == 1)
        {
            Instantiate(BeAttackEffect, HitPosition, Quaternion.Euler(0, 0, 0),BeAttackEffect_Box.transform);
        }
        if (PlayerFace.localScale.x == -1)
        {
            Instantiate(BeAttackEffect, HitPosition, Quaternion.Euler(0, 180, 0),BeAttackEffect_Box.transform);
        }
        IsBeHurt = true;
        BeHurtCount++;
        Currenthealth -= AttackCount;
    }

    private void OnDisable()
    {
        AttackBossEvent.OnRaiseFloatEvent -= OnAttackBoss;
    }
}
