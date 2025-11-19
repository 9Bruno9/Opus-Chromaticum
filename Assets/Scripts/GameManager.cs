using System;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Inventory;
using System.IO;
using Unity.Mathematics;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public float savedHealth = 150;
    public float savedMaxHealth= 200;
    public Vector3 playerPosition;


    public float p2;
    public bool newgame = false;
    public bool nuovapartita = true;
    public bool[] obstacleDestroyed = new bool[4];
    
    public bool[] BossDestroyed = new bool[3];
    public bool[] puzzleResolved = new bool[3];

    public bool WhiteAvailable = false;
    public bool BlackAvailable = false;

    

    public Inventory.InventoryColor[] savedInventory;

    public bool isAMenuOpen;


    
    public void SetIsAMenuOpen(bool open)
    {
        isAMenuOpen = open;
    }
    public bool getPosition;

    [SerializeField] private AudioSource soundSource;
    
    [Header("Options Data")]
    public float sensitivity = 50f;
    public float musicVolume =0.5f;
    public float sfxVolume= 0.5f;

    [SerializeField] AlchemyColor Red;
    [SerializeField] AlchemyColor Blue;
    [SerializeField] AlchemyColor Yellow;
    [SerializeField] AlchemyColor Green;
    [SerializeField] AlchemyColor Orange;
    [SerializeField] AlchemyColor Purple;
    [SerializeField] AlchemyColor Black;
    [SerializeField] AlchemyColor White;
    

    private void Awake()
    {
        isAMenuOpen = false;
        
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 240;
        
        
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            // inizializzazione dati
            savedInventory = new Inventory.InventoryColor[8];

        }
        else
        {
            Destroy(gameObject);
        }

        
    }

    public void LoadButton(bool newgame)
    {
        
        Debug.Log("caricamento");
        isAMenuOpen = false;
        string path = Application.persistentDataPath + "/gamedata.pipsas";
        getPosition = false;
        if (File.Exists(path))
        {
            
            
            isAMenuOpen = false;
            
            nuovapartita = false; 
            PlayerData data = SaveSystem.LoadGame();
            
            
            playerPosition.Set(data.playerPosition[0], data.playerPosition[1], data.playerPosition[2]);
            getPosition = true;

            if (newgame)
            {
                // inizializzazione iniziale
                savedInventory = new Inventory.InventoryColor[8];
                savedInventory[0] = new InventoryColor(Red, 0, 0);
                savedInventory[1] = new InventoryColor(Blue, 0, 0);
                savedInventory[2] = new InventoryColor(Yellow, 0, 0);
                savedInventory[3] = new InventoryColor(Green, 0, 0);
                savedInventory[4] = new InventoryColor(Purple, 0, 0);
                savedInventory[5] = new InventoryColor(Orange, 0, 0);
                savedInventory[6] = new InventoryColor(Black, 0, 0);
                savedInventory[7] = new InventoryColor(White, 0, 0);

                for (int k = 0; k < BossDestroyed.Length; k++)
                {
                    BossDestroyed[k] = false;
                    puzzleResolved[k] = false;
                }

                for (int k = 0; k < obstacleDestroyed.Length; k++)
                {
                    obstacleDestroyed[k] = false;
                }
            }


            for (int k = 0; k < obstacleDestroyed.Length; k++)
            {
                Debug.Log("k uguale" + k);
                obstacleDestroyed[k] = data.obs[k];
            }

            for (int k = 0; k < BossDestroyed.Length; k++)
            {
                BossDestroyed[k] = data.BossDestroyed[k];
                puzzleResolved[k] = data.puzzleResolved[k];
                

            }


            BlackAvailable = data.blackAvailable;
            WhiteAvailable = data.whiteAvailable;

            for (int i = 0; i <= 7; i++)
            {

                Debug.Log("data " + data.colorUnits[i]);
                savedInventory[i].colorUnits = data.colorUnits[i];
                savedInventory[i].materialUnits = data.materialUnits[i];
            }
            savedHealth = data.health;

            sensitivity = data.sensitivity;
            musicVolume = data.musicVolume;
            sfxVolume = data.sfxVolume;
            
            
            if(getPosition)
                SceneManager.LoadScene(data.scene, LoadSceneMode.Single);
            
            //GameObject.Find("Player").transform.SetPositionAndRotation(playerPosition, quaternion.identity);
        }
    }



    public void ButtonSave()
    {
        Debug.Log("salvataggio iniziato");


        //playerPosition = GameObject.Find("Player").transform.position;

        var playerPos = GameObject.Find("Player").transform.position;
        playerPosition = new Vector3(playerPos.x, playerPos.y, playerPos.z);
        SaveSystem.SaveGame(this);
        
        soundSource.Play();
        
        Debug.Log("salvataggio completato");
    }

    public void GameManagerInizialize()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        isAMenuOpen = false;
    }





    public void Setblack()
    {
        BlackAvailable = true;
        puzzleResolved[0] = true;
    }

    public void SetWhite()
    {
        WhiteAvailable = true;
        puzzleResolved[1] = true;
    }

    public void DestroyBoss(int numberBoss)
    {
        BossDestroyed[numberBoss] = true;
    }

}


