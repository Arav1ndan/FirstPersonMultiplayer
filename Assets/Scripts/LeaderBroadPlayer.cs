using UnityEngine;
using TMPro;
public class LeaderBroadPlayer : MonoBehaviour
{
    public TMP_Text playerNameText, killsText, deathsText;

    public void SetDetails(string name, int kills, int deaths)
    {
        playerNameText.text = name;
        killsText.text = kills.ToString();
        deathsText.text = deaths.ToString();
    }
}
