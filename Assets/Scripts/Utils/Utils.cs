using UnityEngine;

public class Utils {
    
    public delegate void Callback();

    public static int FloorToInt(float f) {
        return (int)Mathf.Floor(f);
    }

    public static T SelectRandom<T>(T[] arr) {
        return arr[UnityEngine.Random.Range(0, arr.Length)];
    }

}