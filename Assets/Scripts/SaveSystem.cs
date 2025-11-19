using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem {

    public static void SaveGame(GameManager gameManager) { 
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/gamedata.pipsas";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(gameManager);
    
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerData LoadGame()
    {
        string path = Application.persistentDataPath + "/gamedata.pipsas";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("Save file not found in" + path);
            return null;
        }

    }

    // public static void DeleteGameData(){}
}
