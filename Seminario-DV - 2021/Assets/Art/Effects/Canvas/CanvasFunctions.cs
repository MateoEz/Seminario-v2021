using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CanvasFunctions : MonoBehaviour
{
    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void Restart()
    {      
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
