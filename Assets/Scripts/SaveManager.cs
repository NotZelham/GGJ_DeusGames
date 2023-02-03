using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public SaveManager instance;

    public List<SaveDataBase> saveDatas = new List<SaveDataBase>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void Register(SaveDataBase _data)
    {
        saveDatas.Add(_data);
    }

    public void Save()
    {
        foreach (SaveDataBase saveData in saveDatas)
        {
            saveData.Save();
        }
    }
}
