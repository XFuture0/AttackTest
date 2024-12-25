using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcePool : MonoBehaviour
{
    public GameObject Ice1;
    public GameObject Ice2;
    public GameObject Ice3;
    private float Face;
    [Header("¹ã²¥")]
    public FloatEventSO GetIceFaceEvent;
    [Header("ÊÂ¼þ¼àÌý")]
    public TransFormEventSO SetIceBoxEvent;
    private void OnEnable()
    {
        StartCoroutine(SetIce());
        SetIceBoxEvent.OnRaiseTransFormEvent += OnSetIceBox;
    }

    private void OnSetIceBox(Transform boss)
    {
        Face = boss.localScale.x;   
    }
    private IEnumerator SetIce()
    {
        yield return new WaitForSeconds(0.2f);
        if (Face == -1)
        {
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
        if (Face == 1)
        {
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        }
        Ice1.SetActive(true);
        GetIceFaceEvent.RaiseFloatEvent(Face);
        yield return new WaitForSeconds(0.5f);
        Ice2.SetActive(true);
        GetIceFaceEvent.RaiseFloatEvent(Face);
        yield return new WaitForSeconds(0.5f);
        Ice3.SetActive(true);
        GetIceFaceEvent.RaiseFloatEvent(Face);
        yield return new WaitForSeconds(8f);
        Destroy(gameObject);
    }
    private void OnDisable()
    {
        SetIceBoxEvent.OnRaiseTransFormEvent -= OnSetIceBox;
    }
}
