using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum ObjectTag { Mergible, Destructible, Habitable, Unhabitable }

public class ObjectCollision : MonoBehaviour
{
    public ObjectTag objectTag;
    public int mergeStage = 0;
    public MenuManager menuManager; 

    // A list of possible prefabs to spawn on a successful merge
    public List<GameObject> nextStageOptions;

    private void OnCollisionEnter(Collision collision)
    {
        // Try to get the ObjectCollision script from the other object
        ObjectCollision other = collision.gameObject.GetComponent<ObjectCollision>();
        if (other == null) return;

        // Case 1: Both are destructible OR one is destructible → explode and end game
        if ((objectTag == ObjectTag.Destructible && other.objectTag == ObjectTag.Destructible) ||
            (objectTag != other.objectTag)) // one is mergible, the other is destructible
        {
            ExplodeAndEndGame();
            Destroy(other.gameObject);
            return;
        }

        // Case 2: Both are mergible and at the same stage → merge
        if (objectTag == ObjectTag.Mergible &&
            other.objectTag == ObjectTag.Mergible &&
            mergeStage == other.mergeStage)
        {
            MergeObjects(other);
        }
    }

    void ExplodeAndEndGame()
    {
        Debug.Log("Boom! Game over.");
        // TODO: Insert game-over logic here
        Destroy(gameObject);
    }

    void MergeObjects(ObjectCollision other)
    {
        // Make sure there's at least one next stage prefab
        if (nextStageOptions.Count == 0)
        {
            Debug.LogWarning("No next stage options available!");
            return;
        }

        // Pick a random prefab for the next stage
        GameObject nextPrefab = nextStageOptions[Random.Range(0, nextStageOptions.Count)];

        // Find a good spawn position — average of both object positions
        Vector3 spawnPosition = (transform.position + other.transform.position) / 2f;

        // Optional: calculate average rotation or use Quaternion.identity
        Quaternion spawnRotation = Quaternion.identity;

        // Spawn the new merged object
        GameObject newObject = Instantiate(nextPrefab, spawnPosition, spawnRotation);
        
        ObjectCollision newObjectCollision = newObject.GetComponent<ObjectCollision>();

        if (newObjectCollision != null)
        {
            if (newObjectCollision.objectTag == ObjectTag.Habitable)
            {
                // Delay, then trigger win
                StartCoroutine(TriggerSceneAfterDelay("GoodEnd"));
            }
            else if (newObjectCollision.objectTag == ObjectTag.Unhabitable)
            {
                // Delay, then trigger lose
                StartCoroutine(TriggerSceneAfterDelay("chickenjockey"));
            }
        }
        
        Debug.Log($"Spawned {newObject.name} at {spawnPosition}");

        // Destroy the original objects
        Destroy(gameObject);
        Destroy(other.gameObject);
        }
    
    private IEnumerator TriggerSceneAfterDelay(string sceneName)
    {
        yield return new WaitForSeconds(3f); // or your custom delay
        SceneManager.LoadScene(sceneName);
    }

}

