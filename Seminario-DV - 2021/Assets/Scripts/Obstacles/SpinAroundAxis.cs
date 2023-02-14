using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAroundAxis : MonoBehaviour
{

    [SerializeField] private float AxisX;
    [SerializeField] private float AxisY;
    [SerializeField] private float AxisZ;
    [SerializeField] private float speed;

    private void Update()
    {
        Spin();
    }

    public void Spin()
    {
        transform.Rotate(new Vector3(AxisX, AxisY, AxisZ) * speed * Time.deltaTime);
    }
}
