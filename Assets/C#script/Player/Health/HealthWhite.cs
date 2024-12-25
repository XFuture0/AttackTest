using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthWhite : MonoBehaviour
{
    public GameObject GreenHealth;
    private void Update()
    {
        if(transform.localScale.x > GreenHealth.transform.localScale.x)
        {
            transform.localScale = new Vector3(transform.localScale.x * 0.997f,transform.localScale.y,transform.localScale.z);
        }
    }
}
