using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveSystem
{
    public static void SaveInput(InputReplay inputReplay, int index)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = string.Format("{0}/Save Inputs/SaveInput {1}.bin", Application.dataPath, index);

        FileStream stream = new FileStream(path, FileMode.Create);

        SaveData data = new SaveData(inputReplay);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static SaveData LoadInput(int index)
    {
        string path = string.Format("{0}/Save Inputs/SaveInput {1}.bin", Application.dataPath, index);

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.Log("Save file not found in" + path);
            return null;
        }
    }
}
