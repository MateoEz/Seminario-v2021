using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
public class ManagerCubeStory : MonoBehaviour
{


    [SerializeField] GameObject particleECube;
   
     List<Image> story = new List<Image>();
     List<Image> alreadyShown = new List<Image>();
    [SerializeField] GameObject textStory;
    bool start;
    int count;
    void Start()
    {
        start = false;
        count = 4;
       
    }
    void Update()
    {
        if (start)
        {
            Move();
        }
    }


    void Move()
    {
        for (int i = 0; i < count; i++)
        {
            var firstChild = transform.GetChild(0);
            if (i >= count) start = false;
            if (firstChild.GetComponent<Animator>() == null) return;
            firstChild.GetComponent<Animator>().SetTrigger("in");
            firstChild.transform.GetChild(i).GetComponent<Animator>().SetTrigger("in");

        }        
    }

    [SerializeField] private AudioClip narrativeClip;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerView>() || other.gameObject.GetComponent<DashPlayerFeedback>())
        {
            if (particleECube)
            {
                particleECube.gameObject.SetActive(true);
                if (Input.GetKey(KeyCode.E))
                {
                    
                    
                    start = true;
                    
                    if (AchievementsManager.Instance != null)
                    {
                        AchievementsManager.Instance.TrackAchievement("history_boxes");
                    }
                    if (textStory)
                    {
                        //SetActiveStory(PickRandomText());
                        
                        textStory.gameObject.SetActive(true);
                        if (narrativeClip)
                        {
                            //AudioMaster.Instance.PlayClip(narrativeClip);
                        }
                        
                        StartCoroutine("DeactiveText");
                    }              
                    Destroy(particleECube);
                }
            }

        }
    }

    private void SetActiveStory(int index)
    {
        alreadyShown.Add(story[index]);
        story[index].gameObject.SetActive(true);
        StartCoroutine("DeactiveText", index);
    }
    private int PickRandomText()
    {
        var rnd = Random.Range(0, story.Count);
        Debug.Log(rnd +"random");
        return rnd;
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerView>() || other.gameObject.GetComponent<DashPlayerFeedback>())
        {
            if (particleECube)
            {
                particleECube.gameObject.SetActive(false);
            }

        }
    }


    IEnumerator DeactiveText()
    {
        yield return new WaitForSeconds(5f);
        textStory.gameObject.SetActive(false);
    }
}
