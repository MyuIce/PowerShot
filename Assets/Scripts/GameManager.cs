using UnityEngine;
using TMPro;
public class GameManager : MonoBehaviour
{
    public int score = 0;
    public TextMeshProUGUI scoreText; // スコアを表示するテキスト
    public TextMeshProUGUI clearScoreText;
    
    //Power加算
    public void AddScore()
    {
        score += 10;
        UpdateScoreText();
    }
    

    // 指定した値だけスコアを加算するオーバーロード
    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreText();
    }

    private int highScore = 0;
    private const string HighScoreKey = "HighScore";

    private void Start()
    {
        highScore = PlayerPrefs.GetInt(HighScoreKey, 0);
    }

    private void UpdateScoreText()
    {
        string formattedScore = score.ToString("D4"); // スコアを4桁の文字列に変換
        scoreText.text = "Score: " + formattedScore;
        clearScoreText.text = "Score: " + formattedScore;
        CheckHighScore();
    }

    private void CheckHighScore()
    {
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt(HighScoreKey, highScore);
            PlayerPrefs.Save();
        }
    }

    [SerializeField] private GameObject gameClearUI; // クリア画面のUI
    public bool IsGameClear { get; private set; } = false;

    // ゲームクリア処理
    public void GameClear()
    {
        if (gameClearUI != null)
        {
            gameClearUI.SetActive(true);
            IsGameClear = true;

            //クリア時の弾のリセット
            BallLauncher launcher = FindObjectOfType<BallLauncher>();
            launcher.ResetBall();
        }
    }
}