using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using Data_Blocks;

public static class BinarySerializor{

    public static bool SerializeToBinary<T> (string writePath, object serializableObject, bool overwrite = true)
    {
        if(File.Exists(writePath) && overwrite == false)
        {
            return false;
        }
        BinaryFormatter bf = new BinaryFormatter();
        using (var writeFile = File.Create(writePath))
        {
            bf.Serialize(writeFile, serializableObject);
            
        }
        return true;
    }

    public static T DeserializeFromBinary<T>(string readPath)
    {
        if (!File.Exists(readPath))
        {
            Debug.LogError("File Not found!!! Path is " + readPath);
            return default(T);
        }
        BinaryFormatter bf = new BinaryFormatter();
        
        using (var readFile = File.OpenRead(readPath))
        {
            return (T)bf.Deserialize(readFile);
        }


    }
}

public class wwwAndroidReader : MonoBehaviour
{
    public Scene_Block resultObject;
    
    private static wwwAndroidReader instance = null;
    public static wwwAndroidReader Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject singleton = new GameObject();
                instance = singleton.AddComponent<wwwAndroidReader>();
            }
            return instance;
        }
    }

    public void StartRead(string path)
    {
        StartCoroutine(LoadFileOnAndroid(path));
    }

    public IEnumerator LoadFileOnAndroid(string filePath)
    {
        filePath.Trim();

        {

            WWW file = new WWW(filePath);

            yield return file;
            MemoryStream mem = new MemoryStream(file.bytes);
            BinaryFormatter bf = new BinaryFormatter();
            resultObject = (Scene_Block)bf.Deserialize(mem);
            mem.Close();
        }
    }
}