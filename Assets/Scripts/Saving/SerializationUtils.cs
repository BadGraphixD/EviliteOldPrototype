using System;
using System.Collections.Generic;

public class SerializationUtils {

    public static String GetEntryValue(String[] entries, String entryName) {
        foreach (String entry in entries) {
            String[] splitEntry = entry.Split(new[] { '=' }, 2);

            if (splitEntry[0].Equals(entryName)) {
                return splitEntry[1];
            }
        }
        return null;
    }

    public static String[] UnpackEntryData(String data) {
        int bracketsLayer = 0;
        List<String> entries = new List<String>();
        entries.Add("");

        foreach (char c in RemoveBrackets(data)) {
            if (c == '{') {
                bracketsLayer++;
            }
            else if (c == '}') {
                bracketsLayer--;
            }

            if (c == ',' && bracketsLayer == 0) {
                entries.Add("");
                continue;
            }

            entries[entries.Count - 1] += c;
        }

        return entries.ToArray();
    }

    public static String RemoveBrackets(String data) {
        return data.Substring(1, data.Length - 2);
    }
}

public class DataEntryList {
    String data = "{";
    int entryCount = 0;

    public void AddEntry(String entry) {
        if (entryCount > 0) {
            data += ",";
        }
        data += entry;
        entryCount++;
    }

    public String GetString() {
        return data + "}";
    }
}