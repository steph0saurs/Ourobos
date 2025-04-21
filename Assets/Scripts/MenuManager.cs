using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
        public float delayTime = 3f;
  
        public void PlayGame()
        {
            StartCoroutine(LoadSceneWithDelay("EmilySceneAgain"));
        }

        public void LoseGame()
        {
            StartCoroutine(LoadSceneWithDelay("BadEnd"));
        }

        public void WinGame()
        {
            StartCoroutine(LoadSceneWithDelay("GoodEnd"));
        }

 

        private IEnumerator LoadSceneWithDelay(string sceneName)
        {
            yield return new WaitForSeconds(delayTime);
            SceneManager.LoadScene(sceneName);
        }
    }