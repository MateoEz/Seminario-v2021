using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torreta : MonoBehaviour
{

    [SerializeField]
    private float timeToSpawn;

    [SerializeField]
    private float waitBetweenRounds;

    [SerializeField]
    private int ID;

    [SerializeField]
    private GameObject circleBullet;

    [SerializeField]
    private GameObject spawnPoint;


    [SerializeField]
    private GameObject particleTorret;

    [SerializeField]
    private GameObject particleTorret2;

    [SerializeField]
    private GameObject breakedGem;

    [SerializeField]
    private GameObject gem;

    [SerializeField]
    List<GameObject> partsGem = new List<GameObject>();

    [SerializeField]
    private float explosionForce;

    [SerializeField] AudioClip loadingAudio;
    [SerializeField] AudioClip shootingAudio;
    [SerializeField] AudioSource myAudioSource;
    public static bool activated;
    public bool shooting;
    public bool dead;
    private float myTime;
    public bool reloading;
    bool soundCharging;
    private void Start()
    {
        activated = false;
        dead = false;
    }
    void Update()
    {
        if (activated && !dead && !shooting)
        {
            Reload();
           
        }
        else
        {
            particleTorret.SetActive(false);
            particleTorret2.SetActive(false);
            reloading = false;
            myTime = 0;
        }

    }
    public void Reload()
    {
        reloading = true;
        particleTorret.SetActive(true);
        particleTorret2.SetActive(true);
        myTime += Time.deltaTime;
        ChargeSound();
        if (myTime >= timeToSpawn)
        {
            reloading = false;
            soundCharging = false;
            myTime = 0;        
            Spawn();
        }
    }
    private void ChargeSound()
    {
        if (soundCharging) return;
        soundCharging = true;
        myAudioSource.PlayOneShot(loadingAudio);
    }
    public void Spawn()
    {
        myAudioSource.PlayOneShot(shootingAudio);
        var bullet = Instantiate(circleBullet);
        bullet.transform.GetComponent<TorretaBullet>().SetOwner(transform);
        bullet.transform.position = spawnPoint.transform.position;
        shooting = true;
    }
    public void StopShooting()
    {
        shooting = false;
    }
    public void ChangeMesh()
    {
        gem.gameObject.SetActive(false);
        breakedGem.SetActive(true);
        foreach (var part in partsGem)
        {
            Rigidbody rb = part.GetComponent<Rigidbody>();
            rb.AddExplosionForce(explosionForce, part.transform.position, explosionForce, 3, ForceMode.Force);
        }
    }
}

