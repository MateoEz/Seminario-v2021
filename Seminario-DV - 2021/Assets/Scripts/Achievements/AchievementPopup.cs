using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AchievementPopup : MonoBehaviour
{
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text descriptionText;

    [SerializeField] private GameObject popupContainer;
    
    private static readonly int HideTrigger = Animator.StringToHash("Hide");

    public void Setup(string title, string description)
    {
        titleText.SetText(title);
        descriptionText.SetText(description);

        Show();
    }
    private void Show()
    {
        popupContainer.SetActive(true);
        StartCoroutine(DeactivateCoroutine(4f));
    }
    
    private IEnumerator DeactivateCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        popupContainer.GetComponent<Animator>().SetTrigger(HideTrigger);
        yield return new WaitForSeconds(4.5f);
        popupContainer.SetActive(false);

    }

}
