using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaremaTrack : MonoBehaviour
{
    public Transform B1;
    public Transform B2;
    private Vector3 LastPosition;
    private Vector3 CurrentPosition;
    private Vector3 OnePosition;
    private void Start()
    {
        CurrentPosition = transform.position;
        LastPosition = transform.position;
    }
    private void Update()
    {
        CurrentPosition = transform.position;
        OnePosition = (CurrentPosition - LastPosition);
        B1.position += new Vector3(OnePosition.x * 0.05f,0, 0);
        B2.position += new Vector3(OnePosition.x * 0.05f,0, 0);
        LastPosition = transform.position;
    }
}
