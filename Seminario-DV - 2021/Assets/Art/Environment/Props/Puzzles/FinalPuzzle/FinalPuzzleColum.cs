using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalPuzzleColum : MonoBehaviour
{
    [SerializeField] FinalPuzzleManager manager;
    [SerializeField] GameObject simbol;
    [SerializeField] Material turnOnSimbol;
    [SerializeField] Material turnOffSimbol;
    [SerializeField] Material wrongSimbol;
    [SerializeField] ParticleSystem particle;
    [SerializeField] int sequenceID;

    public bool activated;
    public bool imIn;
    private void Update()
    {
        if (imIn)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                TurnOn();
            }
        }
        CheckIfImOn();
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out PlayerView playerView) || other.TryGetComponent(out DashPlayerFeedback dashPlayerFeedback))
        {           
            particle.gameObject.SetActive(true);
            imIn = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerView playerView) || other.TryGetComponent(out DashPlayerFeedback dashPlayerFeedback))
        {
            particle.gameObject.SetActive(false);
            imIn = false;
        }
    }
    public void CheckIfImOn()
    {
        if (activated)
        {
            simbol.GetComponent<MeshRenderer>().material = turnOnSimbol;
        }
    }
    public void TurnOn()
    {
        if (activated) return;
        simbol.GetComponent<MeshRenderer>().material = turnOnSimbol;
        manager.AddToSequence(this, sequenceID);
        activated = true;
    }

    public void TurnRed()
    {
        simbol.GetComponent<MeshRenderer>().material = wrongSimbol;
        activated = false;
        StartCoroutine("ResetCorutine");
    }
    public void ResetColor()
    {
        simbol.GetComponent<MeshRenderer>().material = turnOffSimbol;
    }
    public IEnumerator ResetCorutine()
    {
        yield return new WaitForSeconds(2);
        ResetColor();
    }
}
