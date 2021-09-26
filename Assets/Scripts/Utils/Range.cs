
public class Range {
    public float min, max;

    public float Length {
        get { return max - min; }
    }

    public Range(float min, float max) {
        this.min = min;
        this.max = max;
    }
}

public class RangeInt {
    public int min, max;

    public int Length {
        get { return (max - min) + 1; }
    }

    public RangeInt(int min, int max) {
        this.min = min;
        this.max = max;
    }
}