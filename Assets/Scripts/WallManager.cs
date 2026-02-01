using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 壁を生成するクラス
/// 
/// ResetWall() -> 壁を消去(ClearWall())し、再生成(SpawnRandomWall())
/// ClearWall() -> 壁を消去
/// SpawnRandomWall() -> ランダムな位置に壁を生成(while)
/// </summary>
public class WallManager : MonoBehaviour
{
    [Header("設定")]
    [SerializeField] private GameObject wallPrefab; // 生成する壁のプレハブ
    [SerializeField] private int spawnCount = 4;    // 生成する数
    [SerializeField] private Vector2 spawnAreaMin = new Vector2(-4, 2); // 生成範囲の最小値 (X, Z)
    [SerializeField] private Vector2 spawnAreaMax = new Vector2(4, 8);  // 生成範囲の最大値 (X, Z)
    [SerializeField] private float fixedYValue = 0.5f; // 壁のY座標（高さ）
    [SerializeField] private Vector3 wallScale = Vector3.one; // 壁のスケール
    [SerializeField] private bool randomizeRotation = true; // 回転をランダムにするかどうか

    private List<GameObject> activeWalls = new List<GameObject>();

    /// <summary>
    /// 壁をリセットして再生成する処理
    /// </summary>
    public void ResetWalls()
    {
        // 1. 既存の壁を全て削除
        ClearWalls();

        // 2. 新しい壁を生成
        for (int i = 0; i < spawnCount; i++)
        {
            SpawnRandomWall();
        }
    }

    /// <summary>
    /// 既存の壁をすべて削除
    /// </summary>
    private void ClearWalls()
    {
        foreach (var wall in activeWalls)
        {
            if (wall != null)
            {
                Destroy(wall);
            }
        }
        activeWalls.Clear();
    }

    /// <summary>
    /// ランダムな位置に壁を1つ生成
    /// </summary>
    private void SpawnRandomWall()
    {
        if (wallPrefab == null) return;

        // ランダムな位置を決定 (試行回数を設けて重なりを防ぐ)
        Vector3 spawnPos = Vector3.zero;
        bool validPosition = false;
        int attempts = 0;

        while (!validPosition && attempts < 10)
        {
            float x = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
            float z = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
            
            spawnPos = new Vector3(x, fixedYValue, z);

            // 簡易的な重なりチェック (半径1.0f以内に他のコライダーがなければOKとする)
            if (!Physics.CheckSphere(spawnPos, 1.0f))
            {
                validPosition = true;
            }
            attempts++;
        }

        Quaternion rotation = Quaternion.identity;
        if (randomizeRotation)
        {
            rotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
        }

        // 生成
        GameObject newWall = Instantiate(wallPrefab, spawnPos, rotation);
        newWall.transform.localScale = wallScale; // スケールを設定
        activeWalls.Add(newWall);
    }
}
