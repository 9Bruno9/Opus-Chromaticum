using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathPanel : MonoBehaviour
{
    [SerializeField] bool DeathPanelActive = false;
    public GameObject Deathpanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Deathpanel.SetActive(DeathPanelActive);
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void RestartButton()
    {
        GameManager.instance.LoadButton(false);
    }
}
