using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ObjectTag { Mergible, Destructible }

public class ObjectCollision : MonoBehaviour
{
    public ObjectTag objectTag;
    public int mergeStage = 0;

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

    private void MergeObjects(ObjectCollision other)
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
        Debug.Log($"Spawned {newObject.name} at {spawnPosition}");

        // Destroy the original objects
        Destroy(gameObject);
        Destroy(other.gameObject);
    }

  
}

