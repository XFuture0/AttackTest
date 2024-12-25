using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBox : MonoBehaviour
{
    private void Update()
    {
        transform.localScale = transform.parent.localScale;
    }
}
