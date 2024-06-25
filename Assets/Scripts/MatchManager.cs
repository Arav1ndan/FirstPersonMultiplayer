using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using ExitGames.Client.Photon;
using System.Collections.Generic;
public class MatchManager : MonoBehaviourPunCallbacks, IOnEventCallback
{
    public static MatchManager instance;
    private void Awake()
    {
        instance = this;
    }
    public enum EventCodes : byte
    {
        NewPlayer,
        ListPlayers,
        UpdateStat
    }
    public List<PlayerInfo> allPlayers = new List<PlayerInfo>();
    private int index;

    void Start()
    {
        if(!PhotonNetwork.IsConnected){
            SceneManager.LoadScene(0);
        }else{
            NewPlayerSend(PhotonNetwork.NickName);

            
        }
    }
    void Update()
    {
       
    }
    // public override void OnLeftRoom()
    // {
    //     SceneManager.LoadScene(0);
    // }
    public void OnEvent(EventData photonEvent)
    {
        if(photonEvent.Code < 200)
        {
            EventCodes theEvent = (EventCodes)photonEvent.Code;
            object[] data = (object[])photonEvent.CustomData;
            Debug.Log("Received event " + theEvent);

            switch(theEvent)
            {
                case EventCodes.NewPlayer:

                    NewPlayerReceive(data);

                    break;

                case EventCodes.ListPlayers:

                    ListPlayersReceive(data);

                    break;

                case EventCodes.UpdateStat:

                    UpdateStatsReceive(data);

                    break;

                // case EventCodes.NextMatch:

                //     NextMatchReceive();

                //     break;

                // case EventCodes.TimerSync:

                //     TimerReceive(data);

                    //break;
            }
        }
    }

    public override void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }
    public override void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }


    public void NewPlayerSend(string username)
    {
        object[] package = new object[4];
        package[0] = username;
        package[1] = PhotonNetwork.LocalPlayer.ActorNumber;
        package[2] = 0;
        package[3] = 0;


        PhotonNetwork.RaiseEvent(
            (byte)EventCodes.NewPlayer,
            package,
            new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient },
            new SendOptions { Reliability = true }
            );
    }
    public void NewPlayerReceive(object[] dataReceived)
    {
        PlayerInfo player = new PlayerInfo((string)dataReceived[0], (int)dataReceived[1], (int)dataReceived[2], (int)dataReceived[3]);

        allPlayers.Add(player);

        ListPlayersSend();
    }
     public void ListPlayersSend()
    {
        // object[] package = new object[allPlayers.Count + 1];

        // package[0] = state;

        // for(int i = 0; i < allPlayers.Count; i++)
        // {
        //     object[] piece = new object[4];

        //     piece[0] = allPlayers[i].name;
        //     piece[1] = allPlayers[i].actor;
        //     piece[2] = allPlayers[i].kills;
        //     piece[3] = allPlayers[i].deaths;

        //     package[i + 1] = piece;
        // }

        // PhotonNetwork.RaiseEvent(
        //     (byte)EventCodes.ListPlayers,
        //     package,
        //     new RaiseEventOptions { Receivers = ReceiverGroup.All },
        //     new SendOptions { Reliability = true }
        //     );
    }
    public void ListPlayersReceive(object[] dataReceived)
    {
        
    }
     public void UpdateStatsSend(int actorSending, int statToUpdate, int amountToChange)
    {

    }
    public void UpdateStatsReceive(object[] dataReceived)
    {
        
    }
}
[System.Serializable]
public class PlayerInfo
{
    public string name;
    public int actor, kills, death;


    public PlayerInfo(string _name, int _actor, int _kills, int _deaths )
    {
        name = _name;
        actor = _actor;
        kills = _kills;
        death = _deaths;
    }
}