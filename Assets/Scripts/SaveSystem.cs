using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Static class used for saving and loading data
/// </summary>
public static class SaveSystem
{
    /// <summary>
    /// Call this function to save the player progress
    /// </summary>
    /// <param name="data">Send tha data in the form of Player Data</param>
    public static void SavePlayer(PlayerData data)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Path.Combine(Application.persistentDataPath, "Player Data.dat");
        FileStream stream = new FileStream(path, FileMode.OpenOrCreate);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    /// <summary>
    /// Call this function to load the player progress
    /// </summary>
    /// <returns>The Stored Player Value (null if nothing is stored)</returns>
    public static PlayerData LoadPlayer()
    {
        string path = Path.Combine(Application.persistentDataPath, "Player Data.dat");
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
            Debug.LogWarning("File not found. Returned null");
            return null;
        }
    }
}
