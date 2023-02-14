using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalDashOpen : MonoBehaviour
{
    [SerializeField]private Material opened;
    [SerializeField]private Material closed;
    public Renderer rnd;
    public bool itsOpen;
    [SerializeField] BoxCollider collider;
    [SerializeField] CableFeedback feeback;

    private void Start()
    {
      //  collider = GetComponent<BoxCollider>();
    }
    
    public void ChangeTheLock()
    {
        itsOpen = !itsOpen;
    }
    private void FixedUpdate()
    {
        CheckForChanges();
    }

    public void CheckForChanges()
    {
        if (itsOpen) ChangeOpenMat();
        else if (!itsOpen) ChangeClosedMat();
    }
    private void ChangeOpenMat()
    {
        feeback.NeedFeedBackFromCables();
        rnd.material = opened;
        collider.isTrigger = true;
    }
    private void ChangeClosedMat()
    {
        rnd.material = closed;
        collider.isTrigger = false;
    }
}
