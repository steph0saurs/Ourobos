using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class ObjectSpawner : MonoBehaviour
{
    public List<GameObject> spawnablePrefabs;
    public Transform[] spawnPoints;
    public int numberToSpawn = 5;

    private InputDevice rightHand;

    void Start()
    {
        // Get the right-hand controller
        var devices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, devices);
        if (devices.Count > 0)
        {
            rightHand = devices[0];
        }
    }

    void Update()
    {
        // Check if A button is pressed (on right controller)
        if (rightHand.isValid && rightHand.TryGetFeatureValue(CommonUsages.primaryButton, out bool aPressed) && aPressed)
        {
            SpawnRandomObjects();
        }
    }

    void SpawnRandomObjects()
    {
        for (int i = 0; i < numberToSpawn; i++)
        {
            GameObject prefab = spawnablePrefabs[Random.Range(0, spawnablePrefabs.Count)];
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        }
    }
}

