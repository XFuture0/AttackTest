using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBall : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    public GameObject WaterBox;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer == 3)
        {
            anim.SetTrigger("Boom");
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }
    public void OnDestory()
    {
        Destroy(WaterBox);
    }
}
