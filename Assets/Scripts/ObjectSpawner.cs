using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class ObjectSpawner : MonoBehaviour
{
    public List<GameObject> spawnablePrefabs;
    public Transform[] spawnPoints;

    private InputDevice rightHand;
    private bool wasAPressedLastFrame = false;

    void Start()
    {
        TryInitializeRightHand();
    }

    void Update()
    {
        // Re-check controller if it's not valid
        if (!rightHand.isValid)
        {
            TryInitializeRightHand();
        }

        // Check A button
        if (rightHand.TryGetFeatureValue(CommonUsages.primaryButton, out bool aPressed))
        {
            if (aPressed && !wasAPressedLastFrame)
            {
                SpawnOneRandomObject();
            }

            wasAPressedLastFrame = aPressed;
        }
    }

    void TryInitializeRightHand()
    {
        var devices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, devices);

        if (devices.Count > 0)
        {
            rightHand = devices[0];
        }
    }

    void SpawnOneRandomObject()
    {
        if (spawnablePrefabs.Count == 0 || spawnPoints.Length == 0) return;

        GameObject prefab = spawnablePrefabs[Random.Range(0, spawnablePrefabs.Count)];
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        Instantiate(prefab, spawnPoint.position, Quaternion.identity);
    }
}

