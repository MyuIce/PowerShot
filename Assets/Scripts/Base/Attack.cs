using UnityEngine;

/// <summary>
/// 敵に当たると攻撃処理を呼び出すクラス
/// </summary>
public class Attack : MonoBehaviour
{
    [SerializeField] public PlayerData playerData; 
    [SerializeField] private int scoreMultiplier = 5;
    private GameManager gameManager;

    private void Start()
    {
        // GameManagerをシーンから検索してアタッチ
        gameManager = FindFirstObjectByType<GameManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            SoundsManager.Instance?.PlayShotSE();

            Damage damage = collision.gameObject.GetComponent<Damage>();
            if (damage != null)
            {
                int currentPower = playerData.CurrentPower;
                damage.TakeDamage(currentPower);

                // Powerの5倍をスコアとして加算
                if (gameManager != null)
                {
                    gameManager.AddScore(currentPower * scoreMultiplier);
                }
            }
        }
    }
}
