using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class KeyBindingScrollList : MonoBehaviour {

    public EventSystem system;
    public Transform contentPanel;
    public GameObject prefab;
    private Button selectedBtn;
    private int selectedKey;
    private GameObject[] keyConfig;

    // Use this for initialization
    public void Start() {
        this.selectedKey = -1;

        this.keyConfig = new GameObject[Controls.KEY_MAX];
        for (int i = 0; i < Controls.KEY_MAX; i++) {
            this.keyConfig[i] = Instantiate(prefab);
            this.keyConfig[i].transform.Translate(0, -(i + 1) * 50, 0);
            this.keyConfig[i].transform.SetParent(contentPanel, false);
        }
        this.UpdateDisplay();
    }

    public void Update() {
        if (this.selectedKey < 0) {
            return;
        }

        foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode))) {
        if (Input.GetKey(vKey)) {
                system.SetSelectedGameObject(null);
                Key key = Controls.getKey(this.selectedKey);
                key.setKeyCode(vKey);
                this.UpdateDisplay();
                this.selectedKey = -1;
                break;
            }
        }
    }

    private void UpdateDisplay() {

        for (int i = 0; i < Controls.KEY_MAX; i++) {
            GameObject keyConfig = this.keyConfig[i];

            Text label = keyConfig.transform.GetChild(0).GetComponent<Text>();
            label.text = Controls.getKey(i).getName();

            GameObject btnObj = keyConfig.transform.GetChild(1).gameObject;
            Button btn = btnObj.GetComponent<Button>();
            Text key = btnObj.transform.GetChild(0).GetComponent<Text>();
            key.text = Controls.getKey(i).getKeyCodeName();

            int j = i;
            btn.onClick.AddListener(delegate {
                TaskOnClick(btn, j);
            });
        }
    }

    private void TaskOnClick(Button btn, int j) {
        this.selectedBtn = btn;
        this.selectedKey = j;
    }
}