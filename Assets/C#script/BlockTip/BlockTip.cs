using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class BlockTip : MonoBehaviour
{
    public TextMeshProUGUI BlockTips;
    private void OnEnable()
    {
        Invoke("OnDestory", 2f);
    }
    private void Update()
    {
        BlockTips.color = new Color(1,1,1,BlockTips.color.a * 0.99f);
    }
    private void OnDestory()
    {
        Destroy(this.gameObject);
    }
}
