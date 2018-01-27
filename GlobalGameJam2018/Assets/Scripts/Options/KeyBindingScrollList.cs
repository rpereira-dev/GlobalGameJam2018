using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class KeyBindingScrollList : MonoBehaviour {

    public Transform contentPanel;

    // Use this for initialization
    public void Start() {
        this.SetDisplay(true);
    }

    private void SetDisplay(bool value) {
        for (int i = 0; i < Controls.KEY_MAX; i++) {
            //TODO : get button, setup, and add

        }
    }
}