using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeysWaypoints : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float rotationSpeed;
    [SerializeField] float minimumDistance;
    int currentIndex;
    bool goBack;
    [SerializeField] List<Transform> waypoints = new List<Transform>();
    [SerializeField] GameObject runa;
    //[SerializeField] Material materialRunaGris;
    [SerializeField] Material materialRunaGlow;

    public string tag;
    private void Start()
    {
    }
    void Update()
    {
        Movement();
    }


    public void Movement()
    {
        Vector3 deltaVector = waypoints[currentIndex].position - transform.position;
        Vector3 direction = deltaVector.normalized;

        transform.forward = Vector3.Lerp(transform.forward, direction, Time.deltaTime * rotationSpeed);
        transform.position += transform.forward * speed * Time.deltaTime;

        float distance = deltaVector.magnitude;
        if (distance < minimumDistance)
        {
            currentIndex++;

        }
        if (currentIndex >= waypoints.Count)
        {
            runa = GameObject.FindGameObjectWithTag(tag).transform.GetChild(0).gameObject;
            // circleDoorOpen.SetActive(true);
            // circleDoorNormal.SetActive(false);
            ChangeMaterial();
            ManagerKeys.instance.keys++;
            ManagerKeys.instance.keysNames.Add(runa.name);
            Destroy(this.gameObject);
        }
    }


    public void ChangeMaterial()
    {
        var runaRnd = runa.GetComponent<Renderer>();
        runaRnd.material = materialRunaGlow;
    }
}
