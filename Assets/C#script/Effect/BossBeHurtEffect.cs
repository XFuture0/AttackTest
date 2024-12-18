using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBeHurtEffect : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(WaitDestory());
    }
    private IEnumerator WaitDestory()
    {
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }
}
