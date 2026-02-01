using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 敵のダメージを受けるクラス
/// </summary>
public class Damage : MonoBehaviour
{
    [SerializeField] private EnemyData enemyData;

    public int currentHP;
    public Slider hpSlider;

    private GameManager gameManager;

    void Start()
    {
        currentHP = enemyData.MAXHP;
        hpSlider.minValue = 0;
        hpSlider.maxValue = enemyData.MAXHP;
        hpSlider.value = currentHP;
        
        Debug.Log($"敵のHP初期化: MaxHP={enemyData.MAXHP}, CurrentHP={currentHP}", this);
        Debug.Log($"スライダー設定: Min={hpSlider.minValue}, Max={hpSlider.maxValue}, Value={hpSlider.value}, Direction={hpSlider.direction}", this);

        // GameManagerを探す
        gameManager = FindFirstObjectByType<GameManager>();
    }

    public void TakeDamage(int damage)
    {
        int oldHP = currentHP;
        currentHP -= damage;
        currentHP = Mathf.Clamp(currentHP, 0, enemyData.MAXHP);
        
        Debug.Log($"ダメージ処理: {damage}ダメージ, HP {oldHP} → {currentHP}", this);
        
        if (hpSlider != null)
        {
            hpSlider.value = currentHP;
        }

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (gameManager != null)
        {
            gameManager.GameClear();
        }
        gameObject.SetActive(false);
    }
}
