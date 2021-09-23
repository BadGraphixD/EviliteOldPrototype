using UnityEngine;
using System;

public class Frog : Entity {

    [SerializeField] private float health;
    [SerializeField] private int age;
    [SerializeField] private String name;

    public override String Serialize() {
        DataEntryList data = new DataEntryList();

        data.AddEntry("health=" + health.ToString());
        data.AddEntry("age=" + age.ToString());
        data.AddEntry("name=" + name);

        return data.GetString();
    }

    public override void Deserialize(String data) {
        String[] entries = SerializationUtils.UnpackEntryData(data);

        health = float.Parse(SerializationUtils.GetEntryValue(entries, "health"));
        age = int.Parse(SerializationUtils.GetEntryValue(entries, "age"));
        name = SerializationUtils.GetEntryValue(entries, "name");
    }

}