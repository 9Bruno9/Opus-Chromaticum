using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
namespace MainMenuScripts
{
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField] private GameObject playButton;
        [SerializeField] private GameObject newSaveButton;
        [SerializeField] private GameObject continueButton;
        
        //TODO: remove the tag [SerializeField]. This is used only for debug
        [SerializeField] private bool _isSaveFilePresent;
        private GameManager gameManager;
        

        private void Start()
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            Debug.LogWarning("Add code to check if a save is present");
            // modificare _isSave FilePresent 

            string path = Application.persistentDataPath + "/gamedata.pipsas";
            if (File.Exists(path))
            {
                _isSaveFilePresent = true; 
            }
            else
            {
                _isSaveFilePresent = false;
            }

            playButton.SetActive(!_isSaveFilePresent);
            newSaveButton.SetActive(_isSaveFilePresent);
            continueButton.SetActive(_isSaveFilePresent);
        }

        public void ChangeScene(string destinationScene)
        {
            SceneManager.LoadScene(destinationScene);
        }

        /// <summary>
        /// Start a new save
        /// </summary>
        public void NewGame()
        {
            if (_isSaveFilePresent)
            {
                DeleteGameData();
                gameManager.GameManagerInizialize();
                SceneManager.LoadScene("Tutorial");
                
            }
            else
            {
                gameManager.GameManagerInizialize();
                SceneManager.LoadScene("Tutorial");
                
            } 
            
        }
        
        /// <summary>
        /// Load a save file and play on it
        /// </summary>
        public void LoadFromSave()
        {
            gameManager.LoadButton(true);
            

            Debug.LogWarning("Add code to load save from file");
        }
        
        public void QuitApplication()
        {
            Application.Quit();
        }

        public static void DeleteGameData()
        {
            string path = Application.persistentDataPath + "/gamedata.pipsas";
            if (File.Exists(path))
            {
                File.Delete(path);
                Debug.Log("Salvataggio eliminato con successo.");
            }
            else
            {
                Debug.LogWarning("Nessun file di salvataggio trovato da eliminare in: " + path);
            }
        }


    }
}


