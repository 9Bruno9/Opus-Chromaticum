using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private string sceneToLoad; // nome della scena da caricare

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.isAMenuOpen = false;
            GameManager.instance.getPosition = false;
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}