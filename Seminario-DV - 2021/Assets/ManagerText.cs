using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerText : MonoBehaviour
{
    [SerializeField]
    GameObject[] textsArray;

    [SerializeField] private AudioClip[] narrativeClips;
    [SerializeField] private AudioSource audioSource;
    int currentIndex;
    bool goToNext;
    public float timeToChangeText;
    float tick;
    private void Start()
    {
        currentIndex = 0;
        tick = 0;
    }

    private bool flag;
    private void Update()
    {
        if (currentIndex > 2) return;

        tick += Time.deltaTime;

        if (tick >= 2f && currentIndex == 0)
        {
            textsArray[0].gameObject.SetActive(true);
            if (!flag)
            {
                audioSource.PlayOneShot(narrativeClips[0]);
                flag = true;
            }
        }
        if (tick >= timeToChangeText)
        {

            if (!goToNext)
            {
                textsArray[currentIndex].GetComponent<Animator>().SetTrigger("fadein");
                currentIndex = currentIndex + 1;
            }
            goToNext = true;
            if (tick >= timeToChangeText + 3.8f)
            {
                textsArray[currentIndex - 1].gameObject.SetActive(false);
                textsArray[currentIndex].gameObject.SetActive(true);
                audioSource.PlayOneShot(narrativeClips[currentIndex]);
                tick = 0;
                goToNext = false;
            }
        }
    }

}