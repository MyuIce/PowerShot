using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
    public void OnClickStartButton()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void OnClickTitleButton()
    {
        SceneManager.LoadScene("TitleScene");
    }
    public void OnClickExitButton()
    {
        Application.Quit();
    }
    public void OnClickRetryButton()
    {
        SceneManager.LoadScene("GameScene");
    }
}
