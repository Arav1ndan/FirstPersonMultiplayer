using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;

    private void Awake()
    {
        instance = this;
    }
    public Transform[] SpawnPoints;
    void Start()
    {
        foreach(Transform Spawn in SpawnPoints){
            Spawn.gameObject.SetActive(false);
            SpawnPoints.AddRange(Spawn);
        }
    }

    
    void Update()
    {
        
    }
   public Transform GetSpawnPoint()
    {
       if (SpawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points set.");
            return null;
        }

        return SpawnPoints[Random.Range(0, SpawnPoints.Length)];
       
    }
}
