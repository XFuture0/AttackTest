using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHit : MonoBehaviour
{
    [Header("π„≤•")]
    public FloatEventSO AttackPlayerEvent;
    [Header("Boss…À∫¶")]
    public float BossAttackCount;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            AttackPlayerEvent.RaiseFloatEvent(BossAttackCount);
        }
    }
}
