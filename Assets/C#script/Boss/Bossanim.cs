using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bossanim : MonoBehaviour
{
    private BossHealth health;
    private Animator anim;
    [Header("¶¯»­×´Ì¬")]
    [HideInInspector] public bool IsHide;
    [HideInInspector] public bool IsDead;
    [HideInInspector] public bool IsWalk;
    private void Awake()
    {
        IsHide = false;
        anim = GetComponent<Animator>();
        health = GetComponent<BossHealth>();
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
}
