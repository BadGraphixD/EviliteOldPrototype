using System;

[Serializable]
public class SlotData {

    public String filePath;
    
    public String name;
    public float progress;
    public GameManager.Level level;
    public String entityData;
    public String mapData;

    public static String Serialize(SlotData slotData) {
        String str = "";

        str += "name=" + slotData.name + "\n";
        str += "progress=" + slotData.progress.ToString() + "\n";
        str += "level=" + ((int)slotData.level).ToString() + "\n";
        str += "entities=" + slotData.entityData + "\n";
        str += "mapData=" + slotData.mapData;

        return str;
    }

    public static SlotData Deserialize(String data) {
        SlotData slotData = new SlotData();
        String[] entries = data.Split('\n');

        slotData.name = SerializationUtils.GetEntryValue(entries, "name");
        slotData.progress = float.Parse(SerializationUtils.GetEntryValue(entries, "progress"));
        slotData.level = (GameManager.Level)int.Parse(SerializationUtils.GetEntryValue(entries, "level"));
        slotData.entityData = SerializationUtils.GetEntryValue(entries, "entities");
        slotData.mapData = SerializationUtils.GetEntryValue(entries, "mapData");

        return slotData;
    }

}