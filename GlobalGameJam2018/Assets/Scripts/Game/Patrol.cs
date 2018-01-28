// Patrol.cs
using UnityEngine;
using UnityEngine.AI;
using System.Collections;


public class Patrol : MonoBehaviour {

    private bool direction = false;

    public void Start() {
        this.transform.position.Set(-3, 0, 21.5f);
        this.transform.rotation = Quaternion.Euler(0, 90, 0);
    }

    public void Update() {
        if (this.transform.position.x > 9.5f && !direction) {
            direction = true;
            this.transform.rotation = Quaternion.Euler(0, -90, 0);
        } else if (this.transform.position.x < -2.5f && direction) {
            direction = false;
            this.transform.rotation = Quaternion.Euler(0, 90, 0);
        }

        this.transform.position += this.transform.forward * 0.02f;
    }
}