using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{

    private PlayerController inputAction;
    private PlayerMove Move;
    private PlayerCheck playercheck;
    private Animator anim;
    private Rigidbody2D rb;
    private float CurrentSpeed;
    private bool isCatch;
    [HideInInspector]public bool CanMove;
    private void Awake()
    {
        inputAction = new PlayerController();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        Move = GetComponent<PlayerMove>();
        playercheck = GetComponent<PlayerCheck>();
        inputAction.Enable();
    }
    private void Update()
    {
        OnCancelBlock();
        if (Move.isJumping)
        {
            anim.SetBool("IsJumping", true);
        }
        if (playercheck.IsGround)
        {
            anim.SetBool("IsJumping", false);
        }
        anim.SetFloat("JumpSpeed", rb.velocity.y);
        PlayerSpeedAnim();
        if (playercheck.IsGround)
        {
            anim.Play("New State",1);
        }
    }
    public void OnCatch()
    {
        isCatch = !isCatch;
        anim.SetBool("Catch", isCatch);
    }
    public void AttackCombo()
    {
        anim.SetInteger("combo", Move.Combo);
        anim.SetTrigger("Attack");
        StartCoroutine(AttackStop());
    }
    private IEnumerator AttackStop()
    {
        yield return new WaitForSeconds(0.25f);
    }
    public void RollTime()
    {
        anim.SetTrigger("Roll");
    }
    public void DashTime()
    {
        anim.SetTrigger("Dashing");
    }
    private void PlayerSpeedAnim()
    {
        CurrentSpeed = math.abs(rb.velocity.x);
        if (CurrentSpeed > 10)
        {
            anim.SetBool("Run", true);
        }
        if (CurrentSpeed < 10)
        {
            anim.SetBool("Run", false);
        }
        anim.SetFloat("Speed", CurrentSpeed);
    }
    public void OnRockHit()
    {
        anim.SetTrigger("RockHit");
    }
    public void OnBlock()
    {
        CanMove = false;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        anim.SetTrigger("Block");
    }
    public void OnWinBlock()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        anim.SetTrigger("WinBlock");
    }
    private void OnCancelBlock()
    {
        if (CanMove && math.abs(rb.velocity.x) > 0)
        {
            CanMove = false;
            Move.IsBlocking = false;
            anim.Play("New State",3);
        }
    }
    public void PlayerisDead()
    {
        anim.SetBool("Dead", true);
    }
}
