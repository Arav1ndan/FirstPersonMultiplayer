using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public static PlayerSpawner instance;
    private void Awake()
    {
        instance = this;
    }
    public GameObject PlayerPrefab;
    private GameObject player;

    public GameObject deathEffect;
    void Start()
    {
        if(PhotonNetwork.IsConnected)
        {
            SpawnPlayer();
        }
    }
    public void SpawnPlayer()
    {
        Transform SpawnPoints = SpawnManager.instance.GetSpawnPoint();
        player = PhotonNetwork.Instantiate(PlayerPrefab.name,SpawnPoints.position,SpawnPoints.rotation);
    }
    public void Die()
    {
        PhotonNetwork.Instantiate(deathEffect.name,player.transform.position,Quaternion.identity);
        PhotonNetwork.Destroy(player);

        SpawnPlayer();
    }
}
