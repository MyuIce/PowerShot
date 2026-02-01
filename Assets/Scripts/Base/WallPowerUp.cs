using UnityEngine;
using TMPro;

/// <summary>
/// 壁に当たるとプレイヤーのパワーを上げるスクリプト
/// </summary>
public class WallPowerUp : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private int powerIncreaseAmount = 5;
    [SerializeField] private TextMeshProUGUI powerText;

    void Start()
    {
        // powerTextが設定されていない場合、名前で検索
        if (powerText == null)
        {
            GameObject textObj = GameObject.Find("PowerText");
            if (textObj != null)
            {
                powerText = textObj.GetComponent<TextMeshProUGUI>();
                Debug.Log("PowerText を名前 'PowerText' で取得しました", this);
            }
            else
            {
                Debug.LogError("名前 'PowerText' のオブジェクトが見つかりません！", this);
            }
        }
        
        // 初期値を表示
        if (powerText != null && playerData != null)
        {
            powerText.text = $"Power: {playerData.CurrentPower}";
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            
            if (playerData != null)
            {
                int oldPower = playerData.CurrentPower;
                playerData.IncreasePower(powerIncreaseAmount);
                // パワーテキストを更新
                if (powerText != null)
                {
                    powerText.text = $"Power: {playerData.CurrentPower}";
                }
                
                Debug.Log($"壁に当たりました！パワー {oldPower} → {playerData.CurrentPower} (+{powerIncreaseAmount})", this);
            }
        }
    }
}
