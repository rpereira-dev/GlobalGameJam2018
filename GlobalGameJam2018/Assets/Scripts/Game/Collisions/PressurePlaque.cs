using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlaque : MonoBehaviour {

    public static string ACTIVE = "active";
    public static string UNACTIVE = "unactive";

    void Start() {
        this.SetColor(Color.red);
    }

    public void OnTriggerEnter(Collider other) {
        if (this.IsCollider(other)) {
            this.tag = ACTIVE;
            this.SetColor(Color.green);
            Game.Instance().CheckPressurePlaque();
        }
    }

    public void OnTriggerExit(Collider other) {
        if (this.IsCollider(other)) {
            this.tag = UNACTIVE;
            this.SetColor(Color.red);
        }
    }
    
    private bool IsCollider(Collider other) {
        Game game = Game.Instance();
        return (other == game.blindedObject.GetComponent<Collider>() || other == game.block1.GetComponent<Collider>());
    }

    private void SetColor(Color color) {
        this.GetComponent<Renderer>().material.color = color;
    }
}
