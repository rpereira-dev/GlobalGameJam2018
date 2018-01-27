using System;
using UnityEngine;

public class Value {
    private string name;
    private float value;

    public Value(string name, float defaultValue) {
        this.name = name;
        this.value = PlayerPrefs.GetFloat(name, defaultValue);
    }

    public void setValue(float value) {
        PlayerPrefs.SetFloat(this.name, value);
        this.value = value;
    }

    public float asFloat() {
        return (this.value);
    }

    public string getName() {
        return (this.name);
    }
}
