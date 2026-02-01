using UnityEngine;
using TMPro;

public class TitleManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI highScoreText;
    private const string HighScoreKey = "HighScore";

    void Start()
    {
        int highScore = PlayerPrefs.GetInt(HighScoreKey, 0);
        if (highScoreText != null)
        {
            highScoreText.text = highScore.ToString("D4");
        }
    }
    
    // デバッグ用：ハイスコアリセット
    public void ResetHighScore()
    {
        PlayerPrefs.DeleteKey(HighScoreKey);
        if (highScoreText != null)
        {
            highScoreText.text = "0000";
        }
    }
}
