using System;
using UnityEngine;

public class Key {
    private string name;
    private KeyCode keyCode;

    public Key(string name, string defaultKeyID) {
        this.name = name;
        this.keyCode = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(name, defaultKeyID));
    }

    public void setKeyCode(KeyCode keyCode) {
        PlayerPrefs.SetString(this.name, keyCode.ToString());
        this.keyCode = keyCode;
    }

    public KeyCode getKeyCode() {
        return (this.keyCode);
    }

    public string getKeyCodeName() {
        return (this.keyCode.ToString());
    }
    
    public string getName() {
        return (this.name);
    }

    public bool isPressed() {
        return (Input.GetKey(this.keyCode));
    }
}
