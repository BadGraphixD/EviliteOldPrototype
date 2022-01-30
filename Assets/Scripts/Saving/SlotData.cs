using System;

[Serializable]
public class SlotData {

    public String filePath;
    
    public String name;
    public int seed;
    public String entityData;
    public String mapData;

    public static String Serialize(SlotData slotData) {
        String str = "";

        str += "name=" + slotData.name + "\n";
        str += "seed=" + slotData.seed.ToString() + "\n";
        str += "entities=" + slotData.entityData + "\n";
        str += "mapData=" + slotData.mapData;

        return str;
    }

    public static SlotData Deserialize(String data) {
        SlotData slotData = new SlotData();
        String[] entries = data.Split('\n');

        slotData.name = SerializationUtils.GetEntryValue(entries, "name");
        slotData.seed = int.Parse(SerializationUtils.GetEntryValue(entries, "seed"));
        slotData.entityData = SerializationUtils.GetEntryValue(entries, "entities");
        slotData.mapData = SerializationUtils.GetEntryValue(entries, "mapData");

        return slotData;
    }

}