using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData")]
public class PlayerData : ScriptableObject
{
    public int InitialPower;
    
    [HideInInspector]
    public int CurrentPower;

    /// <summary>
    /// ScriptableObject が有効になったときに自動的にリセット
    /// </summary>
    private void OnEnable()
    {
        CurrentPower = InitialPower;
    }

    /// <summary>
    /// ゲーム開始時に呼び出して、CurrentPowerを初期化
    /// </summary>
    public void Initialize()
    {
        CurrentPower = InitialPower;
    }

    /// <summary>
    /// パワーを増加させる
    /// </summary>
    public void IncreasePower(int amount)
    {
        CurrentPower += amount;
        Debug.Log($"パワーアップ！ +{amount} (現在のパワー: {CurrentPower})");
    }
}
