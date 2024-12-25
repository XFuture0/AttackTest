using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bossanim : MonoBehaviour
{
    private BossHealth health;
    private Animator anim;
    private MainBoss mainboss;
    [Header("¶¯»­×´Ì¬")]
    [HideInInspector] public bool IsHide;
    [HideInInspector] public bool IsDead;
    [HideInInspector] public bool IsWalk;
    [HideInInspector] public bool IsHeal;
    [HideInInspector] public bool IsSurf;
    private void Awake()
    {
        IsHide = false;
        anim = GetComponent<Animator>();
        health = GetComponent<BossHealth>();
        mainboss = GetComponent<MainBoss>();
    }
    private void Update()
    {
        if(health.Currenthealth <= 0)
        {
            if (!IsDead)
            {
                IsDead = true;
                anim.SetBool("IsDead", true);
            }
        }
        if(health.Currenthealth <= (health.Fullhealth / 2) && !IsHeal)
        {
            IsHeal = true;
            anim.SetTrigger("Heal");
            Invoke("OnState2", 1.5f);
        }
    }
    public void OnState2()
    {
        mainboss.State2();
    }
    public void OnHideAnim()
    {
        IsHide = !IsHide;
        anim.SetBool("Hide", IsHide);
    }
    public void OnWalkAnim()
    {
        IsWalk = !IsWalk;
        anim.SetBool("Walk", IsWalk);
    }
    public void OnAttack1z1()
    {
        anim.SetTrigger("Attack1-1");
    }
    public void OnAttack1z2()
    {
        anim.SetTrigger("Attack1-2");
    }
    public void OnAttack2()
    {
        anim.SetTrigger("Attack2");
    }
    public void OnBallAttack()
    {
        anim.SetTrigger("BallAttack");
    }
    public void OnDefend()
    {
        anim.SetTrigger("Defend");
    }
    public void OnBossBeHurt()
    {
        anim.SetTrigger("BeHurt");
    }
    public void OnSurfAnim()
    {
        IsSurf = !IsSurf;
        anim.SetBool("Surf", IsSurf);
    }
    public void OnHeal()
    {
        anim.SetTrigger("Heal");
    }
}
