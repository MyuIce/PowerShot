using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundsManager : MonoBehaviour
{
    public static SoundsManager Instance { get; private set; }

    [Header("SE Clips")]
    public AudioClip buttonSE;
    public AudioClip cursorSE;
    public AudioClip shotSE;//壁での反射＋ヒット

    

    private AudioSource audioSource;

    private void Awake()
    {
        // シングルトンの設定
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = GetComponent<AudioSource>();
        audioSource.enabled = true; // 念のため

        
    }

    // インスタンスメソッド
    public void PlayButtonSE()
    {
        if (Instance != this && Instance != null)
        {
            Instance.PlayButtonSE();
            return;
        }
        PlaySE(buttonSE);
    }

    public void PlayCursorSE()
    {
        if (Instance != this && Instance != null)
        {
            Instance.PlayCursorSE();
            return;
        }
        PlaySE(cursorSE);
    }

    private void PlaySE(AudioClip clip)
    {
        if (clip == null) return;

        // もし自分がInstanceで、audioSourceがなぜかnullなら再取得を試みる
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        if (audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
        
    }

    public void PlayShotSE()
    {
        if (Instance != this && Instance != null)
        {
            Instance.PlayShotSE();
            return;
        }
        PlaySE(shotSE);
    }
}
