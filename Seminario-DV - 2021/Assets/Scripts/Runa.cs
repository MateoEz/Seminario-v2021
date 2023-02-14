using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Runa : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    [SerializeField] Material materialRunaGlow;
    public void ChangeMaterial()
    {
        var runaRnd = GetComponent<MeshRenderer>();
        runaRnd.material = materialRunaGlow;
    }
}
