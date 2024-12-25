using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HurtCount_Gold : MonoBehaviour
{
    private Rigidbody2D rb;
    public TextMeshProUGUI Count;
    [Header("¹ã²¥")]
    public VoidEventSO GetPlayerPoEvent;
    [Header("ÊÂ¼þ¼àÌý")]
    public TransFormEventSO ReturnPlayerPoEvent;
    public FloatEventSO GetBossHurtCountEvent;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        Count.transform.localScale *= 1.001f;
    }
    private void OnEnable()
    {
        GetPlayerPoEvent.RaiseEvent();
        ReturnPlayerPoEvent.OnRaiseTransFormEvent += OnReturnPlayerPo;
        GetBossHurtCountEvent.OnRaiseFloatEvent += OnGetHurtCount;
    }

    private void OnGetHurtCount(float HurtCount)
    {
        Count.text = HurtCount.ToString();
    }

    private void OnReturnPlayerPo(Transform Player)
    {
        if (Player.localScale.x == -1)
        {
            rb.velocity = new Vector2(-1f, 1f);
            Invoke("OnDistory", 0.5f);
        }
        if (Player.localScale.x == 1)
        {
            rb.velocity = new Vector2(1f, 1f);
            Invoke("OnDistory", 0.5f);
        }
    }

    private void OnDistory()
    {
        Destroy(gameObject);
    }
    private void OnDisable()
    {
        ReturnPlayerPoEvent.OnRaiseTransFormEvent -= OnReturnPlayerPo;
        GetBossHurtCountEvent.OnRaiseFloatEvent -= OnGetHurtCount;
    }
}
