using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class ObjectCollision : MonoBehaviour
{
    public enum ObjectTag
    {
        Mergible,
        Destructible,
        Habitable,
        Unhabitable
    }

    public ObjectTag objectTag;
    public int mergeStage = 0;
    public MenuManager menuManager;
    public List<GameObject> nextStageOptions;
    public bool isFinalStage = false;

    private void OnCollisionEnter(Collision collision)
    {
        // Try to get the ObjectCollision script from the other object
        ObjectCollision other = collision.gameObject.GetComponent<ObjectCollision>();
        if (other == null) return;

        // Case 1: Both are destructible OR one is destructible → explode and end game
        if ((objectTag == ObjectTag.Destructible && other.objectTag == ObjectTag.Destructible) ||
            (objectTag != other.objectTag)) // one is mergible, the other is destructible
        {
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
        
        Destroy(gameObject);
    }

    void MergeObjects(ObjectCollision other)
    {
        if (nextStageOptions.Count == 0)
        {
            Debug.LogWarning("No next stage options available!");
            return;
        }

        GameObject nextPrefab = nextStageOptions[Random.Range(0, nextStageOptions.Count)];
        Vector3 spawnPosition = (transform.position + other.transform.position) / 2f;
        Quaternion spawnRotation = Quaternion.identity;

        GameObject newObject = Instantiate(nextPrefab, spawnPosition, spawnRotation);

        ObjectCollision newObjectCollision = newObject.GetComponent<ObjectCollision>();

        if (newObjectCollision != null && newObjectCollision.isFinalStage)
        {
            if (newObjectCollision.objectTag == ObjectTag.Habitable)
            {
                menuManager.WinGame();
            }
            else if (newObjectCollision.objectTag == ObjectTag.Unhabitable)
            {
                menuManager.LoseGame();
            }
        }

        Debug.Log($"Spawned {newObject.name} at {spawnPosition}");

        Destroy(gameObject);
        Destroy(other.gameObject);
    }
}

