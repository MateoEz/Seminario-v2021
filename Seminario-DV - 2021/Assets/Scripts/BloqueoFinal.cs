using System.Collections;
using System.Collections.Generic;
using AI.Core.StateMachine;
using AI.Enemies.ImplementingStateReader.States;
using UnityEngine;

public class BloqueoFinal : MonoBehaviour
{


    [SerializeField] GameObject block;
    [SerializeField] GameObject block2;


    [SerializeField] List<BaseEnemyWithStateReader> enemies = new List<BaseEnemyWithStateReader>();

    bool inside;
    bool deadAll;

    void Update()
    {
        if (deadAll)
        {
            Deactivate();
            deadAll = false;
        }
        if (inside)
        {
            EnemiesChecker();
        }
    }

    void Deactivate()
    {

        block.GetComponent<Animator>().SetTrigger("out");
        block2.GetComponent<Animator>().SetTrigger("out");
        StartCoroutine("DeactivateCoroutine");
    }


    IEnumerator DeactivateCoroutine()
    {
        yield return new WaitForSeconds(3f);
        block.gameObject.SetActive(false);
        block2.gameObject.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerView>() || other.gameObject.GetComponent<DashPlayerFeedback>())
        {
            block.gameObject.SetActive(true);
            block2.gameObject.SetActive(true);
            inside = true;
        }
    }

   /* private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerView>() || other.gameObject.GetComponent<DashPlayerFeedback>())
        {
            block.gameObject.SetActive(false);
            block2.gameObject.SetActive(false);
        }
    }*/

    void EnemiesChecker()
    {
        if (deadAll) return;
        for (int i = 0; i < enemies.Count; i++)
        {
            if (!enemies[i].IsDead)
            {
                return;
            }

        }
        deadAll = true;
        return;
    }
}
