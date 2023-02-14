using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetachPartsGolem : MonoBehaviour
{
    [SerializeField]
    private float explosionForce;

    [SerializeField]
    private float uplifitingPower;

    [SerializeField]
    private GameObject explosionPivot;
    private void Update()
    {
        DetachChildren();
    }

    public void DetachChildren()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            var childCollider = child.GetComponent<Collider>();
            if (childCollider != null) childCollider.enabled = true;
            var rb = child.GetComponent<Rigidbody>();
            if (rb != null) rb.AddExplosionForce(explosionForce, explosionPivot.transform.position, 15, uplifitingPower, ForceMode.Force);
            child.transform.parent = null;
        }
      
    }
}
