using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

[System.Serializable]
public class PlayerData {
    public int[] colorUnits;
    public int[] materialUnits;
    public float health;
    public float[] playerPosition = new float[3];

    public string scene;
    public bool[] obs = new bool[4];

    public bool[] BossDestroyed = new bool[3];
    public bool[] puzzleResolved = new bool[3];

    public bool whiteAvailable;
    public bool blackAvailable;

    [Header("Options Data")]
    public float sensitivity;
    public float musicVolume;
    public float sfxVolume;


    public PlayerData(GameManager data) {
        
        health = data.savedHealth;
        colorUnits = new int[8]; 
        materialUnits = new int[8]; 
        scene = SceneManager.GetActiveScene().name;

         whiteAvailable = data.WhiteAvailable;
         blackAvailable = data.BlackAvailable;

        for (int k = 0; k < 4; k++)
        {
            obs[k] = data.obstacleDestroyed[k];
        }
        
        for (int j = 0; j < 3; j++) 
        {
            BossDestroyed[j] = data.BossDestroyed[j];
            puzzleResolved[j] = data.puzzleResolved[j];
            
        }

        playerPosition[0] = data.playerPosition.x;
        playerPosition[1] = data.playerPosition.y;
        playerPosition[2] = data.playerPosition.z;

        for (int i = 0; i <= 7; i++)
        {
            
            colorUnits[i] = data.savedInventory[i].colorUnits;
            materialUnits[i] = data.savedInventory[i].materialUnits;
        }
        
        sensitivity = data.sensitivity;
        musicVolume = data.musicVolume;
        sfxVolume = data.sfxVolume;
    }

}
