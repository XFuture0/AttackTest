using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HurtCount_Red : MonoBehaviour
{
    private Rigidbody2D rb;
    public TextMeshProUGUI Count;
    [Header("¹ã²¥")]
    public VoidEventSO GetBossPoEvent;
    [Header("ÊÂ¼þ¼àÌý")]
    public TransFormEventSO ReturnBossPoEvent;
    public FloatEventSO GetPlayerHurtCountEvent;
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
        ReturnBossPoEvent.OnRaiseTransFormEvent += OnReturnBossPo;
        GetPlayerHurtCountEvent.OnRaiseFloatEvent += OnGetHurtCount;
    }
    private void OnGetHurtCount(float HurtCount)
    {
        Count.text = HurtCount.ToString();
        GetBossPoEvent.RaiseEvent();
    }
    private void OnReturnBossPo(Transform Boss)
    {
        if (Boss.localScale.x == -1)
        {
            rb.velocity = new Vector2(-1f, 1f);
            Invoke("OnDistory", 0.5f);
        }
        if (Boss.localScale.x == 1)
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
        ReturnBossPoEvent.OnRaiseTransFormEvent -= OnReturnBossPo;
        GetPlayerHurtCountEvent.OnRaiseFloatEvent -= OnGetHurtCount;
    }
}
