/*
 * Class: SaveSystem
 * Date: 2020.7.22
 * Last Modified : 2020.7.24
 * Author: Hyukin Kwon 
 * Description: 데이터를 커스텀 바이너리 파일에 저장하고 로드한다.
*/

using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    //오디오 볼륨 데이터를 커스텀 바이너리 파일에 저장
    public static void SaveVolumeData(AudioManager audioManager)
    { 
        //경로 생성/지정 및 파일 스트림 생성
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/audioData.Hyukin";
        FileStream stream = new FileStream(path, FileMode.Create);

        AudioData audioData = new AudioData(audioManager);

        //오디오 데이터를 바이너리화 후 스트림에 저장
        formatter.Serialize(stream, audioData);
        stream.Close();
    }

    public static AudioData LoadVolumeData()
    {
        //해당 경로에 이미 저장 파일이 있으면 로드 아니면 null 리턴
        string path = Application.persistentDataPath + "/audioData.Hyukin";
        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            //스트림에서 바이너리 코드를 디시리어라이즈 후 오디오 데이터에 저장
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
