using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
        public float delayTime = 2f;
        public Animator fadeAnimator; 
  
        public void PlayGame()
        {
            StartCoroutine(LoadSceneWithDelay("EmilySceneAgain"));
        }

        public void LoseGame()
        {
            fadeAnimator.Play("fadetoblackmainscreen"); 
            StartCoroutine(LoadSceneWithDelay("chickenjockey"));
        }

        public void WinGame()
        {
            fadeAnimator.Play("fadetoblackmainscreen"); 
            StartCoroutine(LoadSceneWithDelay("GoodEnd"));
        }

 

        public IEnumerator LoadSceneWithDelay(string sceneName)
        {
            yield return new WaitForSeconds(delayTime);
            SceneManager.LoadScene(sceneName);
        }
        
        
      
    }