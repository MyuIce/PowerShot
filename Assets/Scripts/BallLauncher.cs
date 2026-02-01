using UnityEngine;
using System.Collections;


/// <summary>
/// ボールのマネージャークラス
/// ・リスタート処理
/// ・RetryShot処理
/// ・照準線表示
/// ・FOV表示制御
/// </summary>
/// 
public class BallLauncher : MonoBehaviour
{
    [Header("弾の設定")]
    [SerializeField]
    public PlayerData playerData;

    [SerializeField]
    public GameObject bulletPrefab;
    public float speed = 10.0f;
    
    [Header("照準線の設定")]
    [SerializeField]
    private LineRenderer aimLine; // 照準線を表示するLineRenderer
    [SerializeField]
    private float aimLineLength = 10f; // 照準線の長さ
    
    private Rigidbody rb;
    private Vector3 initialPosition = new Vector3(0, 1, -8); //銃口の位置
    private bool canShot = false;
    private GameObject currentBullet; // 現在発射されている弾のインスタンス

    private float timeShot = 0f;
    private GameManager gameManager;
    public Camera mainCamera;

    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        canShot = true;
  
        // LineRendererが設定されていない場合、自動で取得または作成
        if (aimLine == null)
        {
            aimLine = GetComponent<LineRenderer>();
            if (aimLine == null)
            {
                aimLine = gameObject.AddComponent<LineRenderer>();
                // LineRendererの初期設定
                aimLine.startWidth = 0.1f;
                aimLine.endWidth = 0.1f;
                aimLine.material = new Material(Shader.Find("Sprites/Default"));
                aimLine.startColor = Color.yellow;
                aimLine.endColor = Color.red;
            }
        }
        
        // 最初は非表示
        aimLine.enabled = false;

        // 壁を再生成
        if (wallManager != null)
        {
            wallManager.ResetWalls();
        }

        if (countdownText != null)
        {
            countdownText.text = "";
        }
    }


    void OnCollisionEnter(Collision collision)
    {
       
    }
    
    void Update()
    {
        // 照準線の更新
        if (mainCamera != null)
        {
            UpdateAimLine();
        }
        BallRestart();
    }
    
    /// <summary>
    /// 照準線を更新
    /// </summary>
    void UpdateAimLine()
    {
        if (canShot && aimLine != null && mainCamera != null)
        {
            // 照準線を表示
            aimLine.enabled = true;
            
            // マウス位置への方向を計算
            Vector3 shootDirection = GetAimDirection();

            // 照準線の始点と終点を設定
            aimLine.positionCount = 2;
            aimLine.SetPosition(0, initialPosition);
            aimLine.SetPosition(1, initialPosition + shootDirection * aimLineLength);
        }
        else
        {
            // 発射後や発射不可時は照準線を非表示
            if (aimLine != null)
            {
                aimLine.enabled = false;
            }
        }
    }

    /// <summary>
    /// マウス位置に基づいて発射方向を計算
    /// </summary>
    private Vector3 GetAimDirection()
    {
        // カメラからマウス位置へのレイを取得
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        
        // 地面との交点を計算（銃口と同じ高さの平面）
        Plane groundPlane = new Plane(Vector3.up, initialPosition);
        float distance;
        
        if (groundPlane.Raycast(ray, out distance))
        {
            Vector3 mouseWorldPos = ray.GetPoint(distance);
            return (mouseWorldPos - initialPosition).normalized;
        }
        return initialPosition;
    }
    
    

    [SerializeField]
    private WallManager wallManager; // WallManagerへの参照

    public void ResetBall()
    {
        // 次の発射を許可
        canShot = true;
        playerData.Initialize();
        
        // 壁を再生成
        if (wallManager != null)
        {
            wallManager.ResetWalls();
        }
    }

    [SerializeField]
    private TMPro.TextMeshProUGUI countdownText; // カウントダウン表示用テキスト

    /// <summary>
    /// 弾のカウントダウン
    /// </summary>
    /// <returns></returns>
    IEnumerator Wait5Seconds()
    {
        float timeLeft = 5f;
        while (timeLeft > 0)
        {
            if (countdownText != null)
            {
                countdownText.text = Mathf.Ceil(timeLeft).ToString("F0");
            }
            yield return null;
            timeLeft -= Time.deltaTime;
        }

        if (countdownText != null)
        {
            countdownText.text = "";
        }
        
        // 発射した弾のインスタンスを破壊
        if (currentBullet != null)
        {
            Destroy(currentBullet);
            currentBullet = null;
        }
        
        ResetBall();
    }

    
    public void BallRestart()
    {
        // クリア画面時は発射できない
        if (gameManager != null && gameManager.IsGameClear) return;
        if (canShot != true) return;

        if(Input.GetMouseButtonDown(0))
        {
            
            
            if (bulletPrefab == null)
            {
                Debug.LogError("bulletPrefabが設定されていません！Inspectorで設定してください。");
                return;
            }
            
            // 弾を銃口位置に生成
            currentBullet = Instantiate(bulletPrefab, initialPosition, Quaternion.identity);
            
            Rigidbody bulletRb = currentBullet.GetComponent<Rigidbody>();
            
            if (bulletRb != null)
            {
                // マウス方向を取得
                Vector3 shootDirection = GetAimDirection();
                
                // 弾にPlayerDataを設定
                Attack attackComponent = currentBullet.GetComponent<Attack>();
                if (attackComponent != null)
                {
                    attackComponent.playerData = playerData;
                }

                bulletRb.AddForce(shootDirection * speed, ForceMode.Impulse);
                
                canShot = false;
                StartCoroutine(Wait5Seconds());
            }

        }
    }
}