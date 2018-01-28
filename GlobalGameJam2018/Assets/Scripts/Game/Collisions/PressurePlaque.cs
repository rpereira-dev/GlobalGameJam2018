using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlaque : MonoBehaviour {

    public static string ACTIVE = "active";
    public static string UNACTIVE = "unactive";
    private int touchedBy;

    void Start() {
        this.SetColor(Color.red);
        this.touchedBy = 0;
    }

    public void OnTriggerEnter(Collider other) {
        if (this.IsCollider(other)) {
            this.tag = ACTIVE;
            this.SetColor(Color.green);
            Game.Instance().CheckPressurePlaque();
            ++this.touchedBy;
        }
    }

    public void OnTriggerExit(Collider other) {
        if (this.IsCollider(other)) {
            --this.touchedBy;
        }
        if (this.touchedBy <= 0) {
            this.tag = UNACTIVE;
            this.SetColor(Color.red);
            this.touchedBy = 0;
        }
    }
    
    private bool IsCollider(Collider other) {
        Game game = Game.Instance();
        return (other == game.blindedObject.GetComponent<Collider>() || other == game.block1.GetComponent<Collider>() || other == game.block2.GetComponent<Collider>());
    }

    private void SetColor(Color color) {
        this.GetComponent<Renderer>().material.color = color;
    }
}
