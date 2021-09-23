using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;

public class SlotFileManager {

    private const String fileType = ".evilite";

    public static void SaveSlotData(SlotData slotData) {
        FileStream fs = new FileStream(slotData.filePath, FileMode.Create);

        using (StreamWriter writer = new StreamWriter(fs)) {
            writer.Write(SlotData.Serialize(slotData));
        }

        fs.Close();
    }

    public static SlotData[] LoadAllSlotDataFiles() {
        List<SlotData> saveSlots = new List<SlotData>();

        createSlotFileDirectory();

        foreach (String filePath in Directory.GetFiles(fileDirectory())) {
            String data = loadData(filePath);
            SlotData slotData = SlotData.Deserialize(data);
            slotData.filePath = filePath;
            saveSlots.Add(slotData);
        }

        return saveSlots.ToArray();
    }

    public static SlotData LoadNewLevelSlotData(GameManager.Level level, String name, float progress) {
        String path = Application.dataPath + "/Resources/Levels/";

        switch (level) {
        case GameManager.Level.OVERWORLD:
            path += "overworld";
            break;
        case GameManager.Level.CATACOMBS:
            path += "catacombs";
            break;
        case GameManager.Level.TEMPLE:
            path += "temple";
            break;
        }

        path += fileType;

        SlotData slotData = SlotData.Deserialize(loadData(path));
        slotData.filePath = createValidFilePath(name);
        slotData.name = name;
        slotData.progress = progress;

        return slotData;
    }

    private static String loadData(String path) {
        FileStream fs = new FileStream(path, FileMode.Open);
        String data;

        using (StreamReader reader = new StreamReader(fs)) {
            data = reader.ReadToEnd();
        }

        fs.Close();
        return data;
    }

    private static String fileDirectory() {
        return Application.persistentDataPath + "/save_slots";
    }

    private static String createValidFilePath(String name) {
        String extendedName = name;//name.Substring(0, name.Length - 1); // todo: check why this is neccesary

        while(true) {
            if (!fileExists(createFilePath(extendedName))) {
                break;
            }
            else {
                extendedName += "-";
            }
        }

        return createFilePath(extendedName);
    }

    private static String createFilePath(String name) {
        return fileDirectory() + "/" + name + fileType;
    }

    private static bool fileExists(String path) {
        return File.Exists(path);
    }

    private static void createSlotFileDirectory() {
        if (!Directory.Exists(fileDirectory())) {
            Directory.CreateDirectory(fileDirectory());
        }
    }

}
