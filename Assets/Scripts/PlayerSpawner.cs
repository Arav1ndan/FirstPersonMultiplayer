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
    public float respawnTime = 5f;
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
    public void Die(string damager)
    {
       
        UIManager.instance.deathText.text = "You were Killed by "+ damager;
        //PhotonNetwork.Destroy(player);


        //SpawnPlayer();

        MatchManager.instance.UpdateStatsSend(PhotonNetwork.LocalPlayer.ActorNumber,1,1);
        if(player != null){
            StartCoroutine(DieCo());
        }
        
    }
    public IEnumerator DieCo()
    {
        PhotonNetwork.Instantiate(deathEffect.name,player.transform.position,Quaternion.identity);
        PhotonNetwork.Destroy(player);

        UIManager.instance.DeathScreen.SetActive(true);

        yield return new WaitForSeconds(respawnTime);

        UIManager.instance.DeathScreen.SetActive(false);
        SpawnPlayer();
    }
}
