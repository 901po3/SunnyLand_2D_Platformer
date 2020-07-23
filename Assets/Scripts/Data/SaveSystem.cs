/*
 * Class: SaveSystem
 * Date: 2020.7.22
 * Last Modified : 2020.7.22
 * Author: Hyukin Kwon 
 * Description: save data into custom binary file
*/

using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveVolumeData(AudioManager audioManager)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/audioData.Hyukin";
        FileStream stream = new FileStream(path, FileMode.Create);

        AudioData audioData = new AudioData(audioManager);

        //write data to the file
        formatter.Serialize(stream, audioData);
        stream.Close();
    }

    public static AudioData LoadVolumeData()
    {
        string path = Application.persistentDataPath + "/audioData.Hyukin";
        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            //read data from the file
            AudioData audioData = formatter.Deserialize(stream) as AudioData;
            stream.Close();

            Debug.Log("Loaded");
            return audioData;
        }
        else
        {
            Debug.Log("Save file not found in " + path);
            return null;
        }
    }
}
