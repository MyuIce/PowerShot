using UnityEngine;


/// <summary>
/// カメラの切り替えクラス
/// </summary>
public class ViewManager : MonoBehaviour
{
    [SerializeField]
    public Camera[] cameras;
    private int currentCameraIndex = 0;
    
    private int InitialCameraIndex = 0;
    void Start()
    {
        cameras[InitialCameraIndex].gameObject.SetActive(true);
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            SwitchCamera();
        }
    }

    void SwitchCamera()
    {
        int nextCameraIndex = currentCameraIndex + 1;
        if(nextCameraIndex >= cameras.Length)
        {
            nextCameraIndex = 0;
        }

        cameras[currentCameraIndex].gameObject.SetActive(false);
        cameras[nextCameraIndex].gameObject.SetActive(true);
        currentCameraIndex = nextCameraIndex;
       
       
    }


    
}
