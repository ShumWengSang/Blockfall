using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;


public static class BinarySerializor {

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
            return default(T);
        }
        BinaryFormatter bf = new BinaryFormatter();
        using (var readFile = File.OpenRead(readPath))
        {
            return (T)bf.Deserialize(readFile);
        }
    }
}
