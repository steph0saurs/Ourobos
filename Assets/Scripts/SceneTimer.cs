using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTimer : MonoBehaviour
{
    public float delay = 5f; 
    public string scene1 = "Scene1Name"; // First scene option
    public string scene2 = "Scene2Name"; // Second scene option

    void Start()
    {
        Invoke("LoadRandomScene", delay);
    }

    void LoadRandomScene()
    {
        
        string sceneToLoad = Random.Range(0, 2) == 0 ? scene1 : scene2;
        SceneManager.LoadScene(sceneToLoad);
    }
}

