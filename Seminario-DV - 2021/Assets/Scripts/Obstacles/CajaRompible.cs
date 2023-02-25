using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class CajaRompible : MonoBehaviour, ITarget, IDamageable
{
    [SerializeField]
    private GameObject box;
    [SerializeField]
    private GameObject breakedBox;
    [SerializeField]
    private Collider boxCollider;
    [SerializeField]
    private List<Collider> boxParts = new List<Collider>();
    [SerializeField]
    private float explosionForce;
    [SerializeField]
    private float life;
    [SerializeField]
    GameObject spheres;

    [SerializeField]private float _colliderOffset;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _colliderOffset);
    }

    public void ChangeBoxes()
    {
        var random = Random.Range(0, 3);
        if (random == 1 || random == 2)
        {
            InstantiateSpheres();
        }
        AudioMaster.Instance.PlayClip("Romper caja");
        box.SetActive(false);
        breakedBox.SetActive(true);
        boxCollider.enabled = false;
        foreach (var part in boxParts)
        {
            Rigidbody rb = part.GetComponent<Rigidbody>();
            rb.AddExplosionForce(explosionForce, transform.position, 10,1, ForceMode.Force);
        }

        if (AchievementsManager.Instance != null)
        {
            AchievementsManager.Instance.TrackAchievement("destroy_boxes");
        }
       
    }

    void InstantiateSpheres()
    {
        if (spheres == null) return;
        var sp = Instantiate(spheres.gameObject);
        sp.transform.position = this.transform.position + sp.transform.up * 1.2f;
        for (int i = 0; i < spheres.transform.childCount; i++)
        {
            spheres.transform.GetChild(i).GetComponent<Rigidbody>().AddExplosionForce(explosionForce, spheres.transform.position, 10, 1, ForceMode.Force);
        }
    }
    public Transform GetTransform()
    {
        return transform;
    }

    public float GetColliderHalfExtent()
    {
        return _colliderOffset;
    }

    public void ShowFeedback()
    {
        // TODO change material.
    }

    public void HideFeedback()
    {
        //TODO Cambiar material.
    }

    public void GetDamaged(int damage)
    {
        life -= 20;
        if (life <= 0) ChangeBoxes();
    }
}
