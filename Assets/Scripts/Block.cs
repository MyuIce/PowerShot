using UnityEngine;

public class Block : MonoBehaviour
{
    private GameManager gameManager; // GameManager スクリプトへの参照

    // Use this for initialization
    void Start () {
		// GameManagerをシーンから検索してアタッチ
		gameManager = FindFirstObjectByType<GameManager>();
	}
	// Update is called once per frame
	void Update () {
	}

	void OnCollisionEnter(Collision collison)
	{
		if (gameManager != null)
		{
			gameManager.AddScore(); // スコア加算
		}
		Destroy (gameObject);
	}
}