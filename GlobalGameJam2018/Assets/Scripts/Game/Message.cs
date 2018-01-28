using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Message {
    public float time;
    public string msg;
    public Color color;

    public Message(float time, string msg, Color color) {
        this.time = time;
        this.msg = msg;
        this.color = color;
    }
}
