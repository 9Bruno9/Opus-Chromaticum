using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleSceneChanger : MonoBehaviour
{
    public void ChangeScene(string destination)
    {
        GameManager.instance.getPosition = false;
        SceneManager.LoadScene(destination);
    }
}
