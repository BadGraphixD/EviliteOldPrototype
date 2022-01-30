using UnityEngine;
using System;

public class Frog : Entity {

    public override String Serialize() {
        DataEntryList data = new DataEntryList();

        data.AddEntry("posx=" + transform.position.x.ToString());
        data.AddEntry("posy=" + transform.position.y.ToString());

        return data.GetString();
    }

    public override void Deserialize(String data) {
        String[] entries = SerializationUtils.UnpackEntryData(data);

        transform.position = new Vector3(
            float.Parse(SerializationUtils.GetEntryValue(entries, "posx")),
            float.Parse(SerializationUtils.GetEntryValue(entries, "posy")),
            0
        );
    }

}