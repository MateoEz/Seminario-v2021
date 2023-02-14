using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemTorret : MonoBehaviour, IDamageable
{

    Animator anim;
    public Torreta torret;

  
    [SerializeField]
    float life;

    void Start()
    {
        anim = GetComponent<Animator>(); 
    }

    void Update()
    {
        if (torret.reloading)
        {
            anim.SetBool("InRange", true);
        }
        else anim.SetBool("InRange", false);
    }
    public void GetDamaged(int damage)
    {
        life -= 20;
        if (life <= 0)
        {

            torret.dead = true;
            ChangeMesh();
        }      
    }

   
    public void ChangeMesh()
    {
        torret.ChangeMesh();
    }
}
