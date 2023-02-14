using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CableFeedback : MonoBehaviour
{

    [SerializeField] List<GameObject> cables = new List<GameObject>();
    bool alreadyActivated;

    [SerializeField] MoveTo columna;
    [SerializeField] bool hasCables;
    [SerializeField] int timeToWait;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            NeedFeedBackFromCables();
        } 
    }

    public void NeedFeedBackFromCables()
    {
        if (alreadyActivated) return;
        if(hasCables) StartCoroutine(CableCorutine());
    }

    public IEnumerator CableCorutine()
    {
        
        alreadyActivated = true;
        for (int i = 0; i < cables.Count; i++)
        {
           if(cables[i])cables[i].GetComponentInChildren<Animator>().SetTrigger("Activated");
            if (i == cables.Count - 1) {
            if(columna) columna.moveColumn = true;
            }

            yield return new WaitForSeconds(timeToWait);
        }
    }
}
