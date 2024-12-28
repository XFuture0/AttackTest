using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice : MonoBehaviour
{
    private bool IsShoot;
    private bool isOpenHurt;
    public Rigidbody2D rb;
    private float Face;
    private bool IsGetFace;
    [Header("飞行速度")]
    public float Speed;
    [Header("伤害")]
    public float IceAttackCount;
    [Header("广播")]
    public FloatEventSO AttackPlayerEvent;
    [Header("事件监听")]
    public FloatEventSO GetIceFaceEvent;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    {
        GetIceFaceEvent.OnRaiseFloatEvent += OnGetFace;
    }

    private void OnGetFace(float face)
    {
        if (!IsGetFace)
        {
            IsGetFace = true;
            Face = face;
        }
    }

    public void OnShoot()
    {
        IsShoot = true;
    }
    public void OnHurt()
    {
        isOpenHurt = true;
    }
    private void FixedUpdate()
    {
        if (IsShoot)
        {
            rb.velocity = new Vector3(Face * Speed * Time.deltaTime,0,0);
        }
        StartCoroutine(WaitTime());
    }
    private IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (isOpenHurt)
        {
            if (other.tag == "Player")
            {
                isOpenHurt = false;
                var AttackCount = (int)UnityEngine.Random.Range(IceAttackCount,IceAttackCount * 1.3f);
                AttackPlayerEvent.RaiseFloatEvent(AttackCount);
                gameObject.SetActive(false);
            }
        }
    }
    private void OnDisable()
    {
        IsShoot = false;
        GetIceFaceEvent.OnRaiseFloatEvent -= OnGetFace;
    }
}
