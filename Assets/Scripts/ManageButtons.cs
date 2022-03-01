using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManageButtons : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("score", 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Lab1");
    }

    public void RestartGame()
    {
        StartGame();
    }

    public void backToHomeScene()
    {
        SceneManager.LoadScene("Lab1_start");
    }

    public void IniciaCreditos()
    {
        SceneManager.LoadScene("Lab1_creditos");
    }
}
