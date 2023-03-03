using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorretaBullet : MonoBehaviour
{
    [SerializeField]
    private float lifeTime;

    [SerializeField]
    private Vector3 knockedForce;

    public Transform myOwner;
    void Update()
    {
        Grow();
    }
    private float maxXValue = 109f;
    public void Grow()
    {
        transform.localScale += new Vector3(.5f, .5f, 0.1f);
        if (transform.localScale.x >= maxXValue)
        {
            Destroy(this.gameObject);
        }
    }
    
    public void SetOwner(Transform owner)
    {
        myOwner = owner;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<DashPlayerFeedback>()) return;
        var playerView = other.gameObject.GetComponent<PlayerView>();
        if (playerView && !playerView.IsKnocked())
        {
            playerView.GetKnockedBack(knockedForce);
            playerView.GetDamaged(20);           
            Debug.Log("colisiono");
            Destroy(this.gameObject);
        }       
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 19)
        {
            Destroy(this.gameObject);
        }
        if (other.gameObject.GetComponent<TorretaDestroyer>())
        {
            Destroy(this.gameObject);
        }
    }
    private void OnDestroy()
    {
        if(myOwner) myOwner.GetComponent<Torreta>().StopShooting();
    }
}
