using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;
public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    
    void Awake()
    {
        instance = this;
    }
    public TMP_Text overheatedtext;
    public Slider weaponTempSlider;
    
    public GameObject DeathScreen;
    public TMP_Text deathText;
    public Slider healthBar;
    
    public TMP_Text killText, deathsText;
    
    public GameObject leaderboard;
    public LeaderBroadPlayer leaderBroadPlayerDisplay;
    public GameObject EndScreen;

    public TMP_Text timerText;


    public GameObject optionScreen;

    void Start()
    {
        
    }

   
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            ShowHideoptions();
        }

        if(optionScreen.activeInHierarchy && Cursor.lockState != CursorLockMode.None)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void ShowHideoptions()
    {
        if(!optionScreen.activeInHierarchy)
        {
            optionScreen.SetActive(true);
        }else
        {
            optionScreen.SetActive(false);
        }
    }
    public void ReturnToMainMenu()
    {
        PhotonNetwork.AutomaticallySyncScene = false;

        PhotonNetwork.LeaveRoom();
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}

