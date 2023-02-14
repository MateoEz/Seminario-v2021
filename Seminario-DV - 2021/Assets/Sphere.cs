using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere : MonoBehaviour
{
    [SerializeField] int life;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.GetComponent<PlayerView>()) return;
        other.gameObject.GetComponent<PlayerView>().GetHp(life);
        Destroy(this.gameObject);
    }
}
