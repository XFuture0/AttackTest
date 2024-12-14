using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHit : MonoBehaviour
{
    [Header("�㲥")]
    public FloatEventSO AttackPlayerEvent;
    [Header("Boss�˺�")]
    public float BossAttackCount;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            AttackPlayerEvent.RaiseFloatEvent(BossAttackCount);
        }
    }
}
