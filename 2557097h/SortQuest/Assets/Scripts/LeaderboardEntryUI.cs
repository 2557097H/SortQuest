using TMPro;
using UnityEngine;

public class LeaderboardEntryUI : MonoBehaviour
{
    public TextMeshProUGUI positionText;
    public TextMeshProUGUI playerNameText;
    public TextMeshProUGUI scoreText;

    public void SetEntry(int position, string playerName, float score)
    {
        positionText.text = position.ToString() + ".";
        playerNameText.text = playerName;
        scoreText.text = score.ToString() + "s";
    }
}
