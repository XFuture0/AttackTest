using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice : MonoBehaviour
{
    private bool IsShoot;
    private Rigidbody2D rb;
    [Header("·ÉÐÐËÙ¶È")]
    public float Speed;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        if (IsShoot)
        {
            rb.velocity = new Vector3(-Speed * Time.deltaTime,0,0);
        }
    }
    private void OnEnable()
    {
        IsShoot = true;
    }
}
