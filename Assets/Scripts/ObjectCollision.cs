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

    void MergeObjects(ObjectCollision other)
    {
        Vector3 spawnPos = (transform.position + other.transform.position) / 2f;

        if (nextStageOptions != null && nextStageOptions.Count > 0)
        {
            GameObject chosen = nextStageOptions[Random.Range(0, nextStageOptions.Count)];
            GameObject result = Instantiate(chosen, spawnPos, Quaternion.identity);

            ObjectCollision resultBehavior = result.GetComponent<ObjectCollision>();
            if (resultBehavior != null)
            {
                if (resultBehavior.objectTag == ObjectTag.Destructible)
                {
                    Debug.Log("You merged into a destructible! Game over.");
                    ExplodeAndEndGame();
                    Destroy(result);
                    return;
                }
                else if (resultBehavior.objectTag == ObjectTag.Mergible)
                {
                    Debug.Log($"🌱 Merged into stage {resultBehavior.mergeStage} successfully!");
                    // Optional: add particle effect or animation here
                }
            }
        }

        // Clean up the original merging objects
        Destroy(gameObject);
        Destroy(other.gameObject);
    }
}

