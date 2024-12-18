using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice : MonoBehaviour
{
    private bool IsShoot;
    private Rigidbody2D rb;
    private Vector3 Own;
    public Transform Boss;
    [Header("飞行速度")]
    public float Speed;
    [Header("伤害")]
    public float IceAttackCount;
    [Header("广播")]
    public FloatEventSO AttackPlayerEvent;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    {
        Own = this.transform.position;
        IsShoot = true;
    }
    private void FixedUpdate()
    {
        if (IsShoot)
        {
            rb.velocity = new Vector3(Boss.localScale.x * Speed * Time.deltaTime,0,0);
        }
        StartCoroutine(WaitTime());
    }
    private IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(10f);
        this.gameObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            AttackPlayerEvent.RaiseFloatEvent(IceAttackCount);
            this.transform.position = Own;
            this.gameObject.SetActive(false);
        }
    }
}
