using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleRoscaManager : MonoBehaviour
{
  [SerializeField] List<Answer> answers = new List<Answer>();
    [SerializeField] MoveTo columna;
    [SerializeField] GameObject particleSand;
    [SerializeField] TriggerPuzzle triggertPuzzle;
    [SerializeField] PuzzleRosca rosca;
    [SerializeField] PuzzleRosca rosca1;
    [SerializeField] PuzzleRosca rosca2;
    [SerializeField] GameObject Cilinder;
    [SerializeField] CableFeedback cables;
    public int correctAnswers;
    private float waitToWin;
    private bool puzzleCompleted;
    private bool dontCheckAnymore;
    public Material completedPuzzleMaterial;

    private void Start()
    {
        particleSand.SetActive(false);

    }
    private void Update()
    {
        CheckAnswers();
        if (correctAnswers >= 3 && !dontCheckAnymore)
        {
            waitToWin += Time.deltaTime;
            if(waitToWin >= 1.6f)
            {
            Debug.Log("Puzzle Resuelto");
                puzzleCompleted = true;
                dontCheckAnymore = true;
                var rend = Cilinder.gameObject.GetComponent<Renderer>();
                rend.material = completedPuzzleMaterial;
                particleSand.SetActive(true);
                StartCoroutine("StopSand");
            }
        }
        if (puzzleCompleted)
        {
            cables.NeedFeedBackFromCables();
            triggertPuzzle.PuzzleCompleted();
            rosca.PuzzleCompleted();
            rosca1.PuzzleCompleted();
            rosca2.PuzzleCompleted();
            //columna.moveColumn = true;
            puzzleCompleted = false;
        }       
    }

    public void CheckColors()
    {
       
    }
    public void CheckAnswers()
    {
 
        correctAnswers = 0;
        foreach (var item in answers)
        {
            if (item.IsAnswerCorrect())
            {
                correctAnswers++;
            }
        }

    }

    public IEnumerator StopSand()
    {
       yield return new WaitForSeconds(10f);
        Destroy(particleSand.gameObject);
    }
}
