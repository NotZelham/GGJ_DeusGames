using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Storage : MonoBehaviour, IInteractable
{
    [Header("Settings")]
    [SerializeField] private string displayName;
    [SerializeField] private string uid;
    [SerializeField] private int amountOfSlots = 12;

    List<Item> content;
    List<Slot> slots;

    void Update()
    {
        if (ManagersManager.instance.inputManager.Inputs.PlayerGround.Crouch.triggered)
        {
            Save();
        }
    }

    private void Start()
    {
        content = new List<Item>();

        for (int i = 0; i < amountOfSlots; i++)
        {
            content.Add(null);
        }

        Load();
    }

    public void Save()
    {
        string destination = Application.persistentDataPath + $"/Storages/{uid}.dat";
        FileStream file;

        if (!Directory.Exists(destination))
            Directory.CreateDirectory(Application.persistentDataPath + "/Storages");

        if (!File.Exists(destination))
        {
            using (FileStream fs = File.Create(destination))
            {
                string json = JsonUtility.ToJson(content);
                Byte[] info = new UTF8Encoding(true).GetBytes(json);
                fs.Write(info, 0, info.Length);
            }
        }
        else
        {
            using (FileStream fs = File.OpenWrite(destination))
            {
                string json = JsonUtility.ToJson(content);
                Byte[] info = new UTF8Encoding(true).GetBytes(json);
                fs.Write(info, 0, info.Length);
            }
        }
    }

    public void Load()
    {
        string destination = Application.persistentDataPath + $"/Storages/{uid}.dat";

        if (!File.Exists(destination)) 
            return;

        string json = File.ReadAllText(destination);
        content = JsonUtility.FromJson<List<Item>>(json);
    }

    public void Open()
    {
        InventoryController ic = InventoryController.Instance;

        slots = new List<Slot>();

        for (int i = ic.storage.grid.childCount - 1; i >= 0; i--)
        {
            Destroy(ic.storage.grid.GetChild(i).gameObject);
        }

        for (int i = 0; i < amountOfSlots; i++)
        {
            GameObject go = Instantiate(ic.storage.slotPrefab, ic.storage.grid);
            Slot s = go.GetComponent<Slot>();

            s.onSetItem += OnSlotChanged;
            slots.Add(s);

            if (content[i] != null)
                s.SetItem(content[i]);
        }

        ic.Open();
        ic.storage.Show();
    }

    public void OnSlotChanged(Slot _s)
    {
        for (int i = 0; i < slots.Count; i++)
            if (slots[i] == _s)
                content[i] = _s.GetItem();
    }

    public void DoInteraction()
    {
        Open();
    }

    public void SetInteractionText()
    {
        ActionUI.Instance.SetVisible();
        ActionUI.Instance.SetText($"Press E to open {displayName}");
    }
}
