using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class ObjectSpawner : MonoBehaviour
{
    public List<GameObject> spawnablePrefabs; // Your spawnable objects
    public Transform spawnPoint;              // Where the objects should spawn

    private bool canSpawn = true;

    void Update()
    {
        if (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame && canSpawn)
        {
            SpawnRandomObject();
        }

        // Optional: add fallback for testing in editor
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame && canSpawn)
        {
            SpawnRandomObject();
        }
    }

    void SpawnRandomObject()
    {
        if (spawnablePrefabs.Count == 0) return;

        int index = Random.Range(0, spawnablePrefabs.Count);
        GameObject prefab = spawnablePrefabs[index];

        Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        canSpawn = false;

        // Wait until button is released to allow next spawn
        StartCoroutine(WaitForButtonRelease());
    }

    private System.Collections.IEnumerator WaitForButtonRelease()
    {
        yield return new WaitUntil(() =>
            Gamepad.current == null || !Gamepad.current.buttonSouth.isPressed
        );
        canSpawn = true;
    }
}

