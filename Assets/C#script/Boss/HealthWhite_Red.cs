using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthWhite_Red : MonoBehaviour
{
    public GameObject RedHealth;
    private void Update()
    {
        if (transform.localScale.x > RedHealth.transform.localScale.x)
        {
            transform.localScale = new Vector3(transform.localScale.x * 0.997f, transform.localScale.y, transform.localScale.z);
        }
    }
}
