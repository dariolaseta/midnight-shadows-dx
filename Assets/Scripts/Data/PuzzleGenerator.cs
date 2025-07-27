using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PuzzleGenerator : MonoBehaviour
{
    public static PuzzleGenerator Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void GenerateLockCode(int[] combination)
    {
        combination[0] = Random.Range(0, 10);
        combination[1] = Random.Range(0, 10);
    }

    public void SpawnGameObject(List<GameObject> gameObjects, int count)
    {
        if (count > gameObjects.Count)
        {
            Debug.LogError("Game object count exceeds number of objects");
            return;
        }
        
        for (int i = 0; i < count; i++)
        {
            gameObjects[i].SetActive(true);
        }
    }
}
