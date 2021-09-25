using UnityEngine;

public class Utils {

    public static int floor(float f) {
        return (int)Mathf.Floor(f);
    }

    public static T selectRandom<T>(T[] arr) {
        return arr[UnityEngine.Random.Range(0, arr.Length)];
    }

}