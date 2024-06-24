using UnityEngine;
using Photon.Pun;
public class DeactiveIfNotMine : MonoBehaviourPunCallbacks
{
   
    void Start()
    {
        if(!photonView.IsMine)
        {
            gameObject.SetActive(false);
        }
    }

    
}
