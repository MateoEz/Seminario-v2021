using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FinalPuzzleManager : MonoBehaviour
{

    [SerializeField] List<int> answer = new List<int>();
    [SerializeField] List<int> answersToCheck = new List<int>();
    [SerializeField] List<FinalPuzzleColum> colums = new List<FinalPuzzleColum>();

    [SerializeField] GameObject puzzleWall;
    [SerializeField] Material puzzleCompletedMat;

    [SerializeField] MoveTo door;
    [SerializeField] CableFeedback cables;

    public bool on;
    public int correctAnswer;
    bool completed;
    private void Update()
    {
        if (!on) return;
        ShowSequence();
        if (answersToCheck.Count ==answer.Count)
        {
            CheckSequence();
        }
    }
    public void AddToSequence(FinalPuzzleColum colum,int id)
    {
        colums.Add(colum);
        answersToCheck.Add(id);
    }

    public void CheckSequence()
    {
        if (completed) return;
        for (int i = 0; i < answer.Count; i++)
        {
            if (answer[i].Equals(answersToCheck[i]))
            {
                correctAnswer++;
                Debug.Log("ta bien este");
            }
            else Debug.Log("ta mal");
        }
        if (correctAnswer >=answer.Count)
        {
            CorrectAnswer();
            Debug.Log("completado");
        }
        else
        {
            WrongAnswer();
            Debug.Log("fracasado");
        }

    }

    public void CorrectAnswer()
    {
        AudioMaster.Instance.PlayClip("PuzzleCompleted", .8f);
        completed = true;
        //door.moveColumn = true;
        puzzleWall.GetComponent<MeshRenderer>().material = puzzleCompletedMat;
        cables.NeedFeedBackFromCables();
    }

    public void ClearSequence()
    {
        colums.Clear();
        answersToCheck.Clear();
    }

    public void WrongAnswer()
    {
        foreach (var item in colums)
        {
            item.TurnRed();
        }
        ClearSequence();
    }

    public void ResetMats()
    {
        foreach (var item in colums)
        {
            item.ResetColor();
        }
    }

    [SerializeField] List<GameObject> simbols = new List<GameObject>();
    bool activatedSequece;
    public void ShowSequence()
    {
        if (activatedSequece) return;
        float myTick = 0;
        for (int i = 0; i < simbols.Count; i++)
        {
           
            StartCoroutine(BrightObject(simbols[i] ,myTick));
            myTick += 2;
        }
        activatedSequece = true;
    }
    IEnumerator BrightObject(GameObject simbol,float tick)
    {
        yield return new WaitForSeconds(tick);
        Debug.Log("le hago el brillo a " + simbol.name);
        simbol.GetComponent<Animator>().SetTrigger("BrightTime");
        if (simbol == simbols[simbols.Count-1])
        {
            yield return new WaitForSeconds(3f);
            foreach (var item in simbols)
            {
                item.GetComponent<Animator>().SetTrigger("DarkTime");
            }
            yield return new WaitForSeconds(1f);
            activatedSequece = false;
        }
    }
}
