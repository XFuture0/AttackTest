using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class WaterBallBox : MonoBehaviour
{
    public GameObject WaterBall;
    [Header("生成计时器")]
    public float time;
    private float time_Count;
    private void Awake()
    {
        time_Count = time;
    }
    private void Update()
    {
        if(time_Count > 0)
        {
            time_Count -= Time.deltaTime;
        }
        if(time_Count <= 0)
        {
            float n = UnityEngine.Random.Range(-19.5f,0);
            var SetPosition = new Vector3(transform.position.x + n,transform.position.y,transform.position.z);
            Instantiate(WaterBall,SetPosition,Quaternion.identity,transform);
            time_Count = time;
        }
    }
}
