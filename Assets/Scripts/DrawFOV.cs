using UnityEngine;

/// <summary>
/// FOVを扇形に描画するクラス
/// </summary>
[RequireComponent(typeof(MeshFilter))]
public class DrawFOV : MonoBehaviour
{
    public float viewRadius = 8f;
    public float viewAngle = 90f;
    public int resolution = 30;

    Mesh mesh;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    
    void LateUpdate()
    {
        UpdateFOVMesh();
    }
    

    void UpdateFOVMesh()
    {
        int step = resolution;
        float angleStep = viewAngle / step;   

        Vector3[] vertices = new Vector3[step + 2];
        int[] triangles = new int[step * 3];

        vertices[0] = Vector3.zero;

        for (int i = 0; i <= step; i++)
        {
            float angle = -viewAngle / 2 + angleStep * i; 
            float rad = Mathf.Deg2Rad * angle;

            Vector3 dir = new Vector3(Mathf.Sin(rad), 0, Mathf.Cos(rad));
            vertices[i + 1] = dir * viewRadius;
        }

        int index = 0;
        for (int i = 0; i < step; i++)
        {
            triangles[index++] = 0;
            triangles[index++] = i + 1;
            triangles[index++] = i + 2;
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
    }

    /// <summary>
    /// マウス位置に基づいて、FOV範囲内の発射方向を取得
    /// </summary>
    /// <param name="ballPosition">ボールの位置</param>
    /// 
    public Vector3 GetShootDirection(Vector3 ballPosition)
    {
        // カメラからマウス位置へのレイを取得
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        // 地面との交点を計算（同じ高さの平面）
        Plane groundPlane = new Plane(Vector3.up, ballPosition);
        float distance;
        
        Vector3 targetDirection;
        
        if (groundPlane.Raycast(ray, out distance))
        {
            // マウス位置のワールド座標
            Vector3 mouseWorldPos = ray.GetPoint(distance);
            
            // 発射口からマウス位置への方向
            targetDirection = (mouseWorldPos - ballPosition).normalized;
        }
        else
        {
            // レイキャストが失敗した場合は正面方向
            targetDirection = transform.forward;
        }
        
        // ローカル座標系での角度を計算
        Vector3 localDir = transform.InverseTransformDirection(targetDirection);
        float angleToTarget = Mathf.Atan2(localDir.x, localDir.z) * Mathf.Rad2Deg;

        // FOV範囲内にクランプ
        float halfAngle = viewAngle / 2f;
        angleToTarget = Mathf.Clamp(angleToTarget, -halfAngle, halfAngle);

        // クランプされた角度をワールド方向に変換
        float rad = angleToTarget * Mathf.Deg2Rad;
        Vector3 clampedLocalDir = new Vector3(Mathf.Sin(rad), 0, Mathf.Cos(rad));
        Vector3 clampedWorldDir = transform.TransformDirection(clampedLocalDir);

        return clampedWorldDir;
    }
}
