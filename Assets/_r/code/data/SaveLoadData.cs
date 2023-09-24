using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


public class SaveLoadData : MonoBehaviour
{
    public string gameName = "Drude";
    
    public  int             currentSlot;
    public  List<string>    saveSlots; 
    private string          _pdp, _gamePath, _currentSavePath;
    private BinaryFormatter bf = new BinaryFormatter();
    
    public delegate void selfDelegate(SaveLoadData self);
    public static event selfDelegate InitData;
    public static void __InitData(SaveLoadData self)
    {
        if (InitData != null) InitData(self);
    }

    private void OnEnable()
    {
        __InitData(this);
    }

    void Awake()
    {
        _pdp = Application.persistentDataPath;
        LoadSaveSlots();
        if (currentSlot < 0) currentSlot = 0;
        _gamePath        = $"{_pdp}/{gameName}";
        _currentSavePath = $"{_gamePath}/Slot{currentSlot}";
    }

    public void CreateNewSaveSlot(int slot)
    {
  	  EnsureFolderExists($"{_gamePath}/Slot{slot}");
    }

    public string GetCurrentSavePath()
    {
        return _currentSavePath;
    }

    private void LoadSaveSlots()
    {
  	  // string[] _saveSlots = Directory.GetDirectories(_gamePath);
  	  // foreach (string s in _saveSlots) saveSlots.Add(s.Replace("\\", "/"));
    }

    public void LoadData(int slot)
    {
      Debug.Log("TODO"); 
    }
    
    
    
    // System.IO.Path.Combine(Application.streamingAssetsPath, videoFileName);
    
    // public void SavePartyData(PartyData data)
    // {
    //   string json = JsonUtility.ToJson(data);
    //   WriteToFile("/Party/", "party.data", json);
    // }
    //
    // public void LoadPartyData(PartyData data)
    // {
    //   string json = ReadFromFile("Party/", "party.data");
    //   JsonUtility.FromJsonOverwrite(json, data);
    // }
    //
    // public void SavePlayableData(PlayableCharacter data)
    // {
    //   string characterFile = data.characterName + ".data";
    //   string json          = JsonUtility.ToJson(data);
    //   WriteToFile("/PC/", characterFile, json);
    // }
    //
    // public void LoadPlayableData(PlayableCharacter data)
    // {
    //   string json = ReadFromFile("/PC/", data.characterName + ".data");
    //   JsonUtility.FromJsonOverwrite(json, data);
    // }
    //
    // public void SaveActiveJournalEntryData(JournalActiveEntrySaveData data)
    // {
    //   string journalFile = "journalMain.data";
    //   string json        = JsonUtility.ToJson(data);
    //   WriteToFile("/J/", journalFile, json);
    // }
    //
    // public void LoadActiveJournalEntryData(JournalActiveEntrySaveData data)
    // {
    //   string json = ReadFromFile("/J/", "journalMain.data");
    //   JsonUtility.FromJsonOverwrite(json, data);
    // }

    /////////////////////////////////////////////////////////////////////////
    //////////////////////////////// backend ////////////////////////////////
    /////////////////////////////////////////////////////////////////////////

    private void WriteToFile(string folderName, string fileName, string json)
    {
      EnsureFolderExists(folderName);
      string path = GetFilePath(folderName, fileName);

      FileStream fileStream = new FileStream(path, FileMode.Create);
      using(StreamWriter writer = new StreamWriter(fileStream))
      {
        writer.Write(json);
      }
    }

    private string ReadFromFile(string folderName, string fileName)
    {
      EnsureFolderExists(folderName);
      string path = GetFilePath(folderName, fileName);

      if(File.Exists(path))
      {
        using(StreamReader reader = new StreamReader(path))
        {
          string json = reader.ReadToEnd();
          return json;
        }
      }
      else
      {
        File.Create(_currentSavePath + folderName + fileName);
      }

      return "";
    }

    // Helper functions which check for files and paths existing.

    private void EnsureFolderExists(string folderName)
    {
      if (!Directory.Exists(_currentSavePath + folderName))
      {
          Directory.CreateDirectory(_currentSavePath + folderName);
      }
    }

    private string GetFilePath(string folderName, string fileName)
    {
      return _currentSavePath + folderName + fileName;
    }
    
    public T Load<T>(string filename) where T : class
    {
        string data = null;

        filename = Path.Combine(_currentSavePath, filename);

        if (File.Exists(filename) == false)
        {
            return null;
        }

        using (StreamReader reader = new StreamReader(filename, false))
        {
            try
            {
                data = reader.ReadToEnd();
            }
            catch (System.Exception ex)
            {
                Debug.LogError(ex.Message);
                data = null;
            }
        }
        
        return data == null ? null : JsonUtility.FromJson<T>(data);
    }
    
    public bool Save(string filename, object data)
    {
        string jsonData = JsonUtility.ToJson(data);
        bool   success  = true;

        using (StreamWriter writer = new StreamWriter(Path.Combine(_currentSavePath, filename), false))
        {
            try
            {
                writer.Write(jsonData);
                writer.Flush();
            }
            catch (System.Exception ex)
            {
                Debug.LogError(ex.Message);
                success = false;
            }
        }

        return success;
    }
}
