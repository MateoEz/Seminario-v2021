using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByTime : MonoBehaviour
{
    private float myTime;
    [SerializeField]
    private float timeToDestroy;

    [SerializeField]
    private float timeToDestroyRigid;
    [SerializeField]
    private float timeToDestroyCollider;

    [SerializeField]
    private bool destroy;

    [SerializeField]
    private bool destroyRigidBody;

    [SerializeField]
    private bool destroyCollider;

    private void Update()
    {
        myTime += Time.deltaTime;
        if (myTime >= timeToDestroy) MyDestroy();
        if (myTime >= timeToDestroyCollider)DestroyCollider();
        if (myTime >= timeToDestroyCollider)DestroyRigidBody();
    }
    public void MyDestroy()
    {
        if (!destroy) return;
        Destroy(gameObject);
    }
    public void DestroyCollider()
    {
        if (!destroyCollider) return;
        foreach (Collider c in GetComponents<Collider>())
        {
            c.enabled = false;
        }
    }
    public void DestroyRigidBody()
    {
        if (!destroyRigidBody) return;
        var rb = GetComponent<Rigidbody>();
        Destroy(rb);
    }
}
