using UnityEngine;
using System;

[Serializable]
public class Timer {
    public float time;
    public float cooldown;
    
    public delegate void Callback();
    public Callback callback;

    public bool Finished {
        get { return cooldown <= 0f; }
    }
    public float Progress {
        get { return cooldown / time; }
    }
    
    public Timer(float time = 0f, Callback callback = null) {
        this.cooldown = this.time = time;
        this.callback = callback;
    }

    public void Update(float deltaTime) {
        bool finishedBefore = Finished;
        cooldown -= deltaTime;
        if (!finishedBefore && Finished) {
            callback?.Invoke();
        }
    }
    public void Update() {
        Update(Time.deltaTime);
    }
    public void Reset() {
        cooldown = time;
    }
}

[Serializable]
public class RepeatTimer {
    public float interval;
    public float cooldown;
    
    public delegate void Callback();
    public Callback callback;

    public float Progress {
        get { return cooldown / interval; }
    }

    public RepeatTimer(float interval = 0f, Callback callback = null) {
        this.cooldown = this.interval = interval;
        this.callback = callback;
    }

    public void Update(float deltaTime) {
        cooldown -= deltaTime;
        while (cooldown <= 0f) {
            cooldown += interval;
            callback?.Invoke();
        }
    }
    public void Update() {
        Update(Time.deltaTime);
    }
    public void Reset() {
        cooldown = interval;
    }
}

[Serializable]
public class RandomRepeatTimer : RepeatTimer {
    
    public Vector2 intervalRange;

    public RandomRepeatTimer(float minInterval = 0f, float maxInterval = 1f, Callback callback = null)
        : this(new Vector2(minInterval, maxInterval), callback) { }

    public RandomRepeatTimer(Vector2 intervalRange, Callback callback = null) {
        this.intervalRange = intervalRange;
        this.cooldown = this.interval = randomInterval();
        this.callback = callback;
    }
    
    new public void Update(float deltaTime) {
        cooldown -= deltaTime;
        while (cooldown <= 0f) {
            cooldown += (interval = randomInterval());
            callback?.Invoke();
        }
    }
    new public void Reset() {
        cooldown = interval = randomInterval();
    }

    protected float randomInterval() {
        return UnityEngine.Random.Range(intervalRange.x, intervalRange.y);
    }

}
