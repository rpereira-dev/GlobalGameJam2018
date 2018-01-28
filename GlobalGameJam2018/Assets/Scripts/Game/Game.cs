using UnityEngine;
using System;
using System.Collections.Generic;
public class Game : MonoBehaviour {

    private static Game gameInstance;

    public const int STATE_MAIN_MENU           = 0;
    public const int STATE_MAIN_MENU_OPTIONS   = 1;
    public const int STATE_INGAME              = 2;
    public const int STATE_INGAME_MENU         = 3;
    public const int STATE_INGAME_MENU_OPTIONS = 4;
    public const int STATE_GAME_EXIT           = 5;
    public const int STATE_CREDIT               = 6;

    private int state;
    private CameraController cameraController;
    private Blinded blinded;
    private System.Random rng;
    private GameObject hoveredActionnable;

    public Canvas mainMenu;
    public Canvas pauseMenu;
    public Canvas optionMenu;
    public Canvas creditMenu;
    public GameObject cam;
    public GameObject world;
    public GameObject birdObject;
    public GameObject blindedObject;
    public GameObject birdSelectionObject;
    public AudioSource[] birdSounds;
    public GameObject[] actionnable;
    public GameObject door1;
    public GameObject door2;
    public GameObject block1;
    public GameObject plaque1;
    public GameObject plaque2;
    public GameObject door3;
    public GameObject door4;

    public const int BUTTON_1 = 0;
    public const int LEVIER_1 = 1;
    public const int BUTTON_2 = 2;
    public const int BUTTON_3 = 3;

    private LinkedList<Message> messages = new LinkedList<Message>();

    public void Start() {
        gameInstance = this;

        Controls.Setup();
        this.hoveredActionnable = null;
        this.cameraController = new CameraController(this.cam, this.birdObject, this.birdSelectionObject);
        this.blinded = new Blinded(blindedObject);
        this.rng = new System.Random();

        this.SetState(STATE_MAIN_MENU);
    }

    public void stackMessage(float time, string msg, Color color) {
        this.messages.AddLast(new Message(time, msg, color, 0));
    }

    public void stackMessage(float time, string msg, Color color, float offy) {
        this.messages.AddLast(new Message(time, msg, color, offy));
    }

    public int GetState() {
        return (this.state);
    }

    public void OnGUI() {
        float dt = Time.deltaTime;
        if (this.messages.Count == 0) {
            return;
        }
        Message message = this.messages.First.Value;
        message.time -= dt;
        if (message.time <= 0) {
            this.messages.RemoveFirst();
        } else {
            var centeredStyle = GUI.skin.GetStyle("Label");
            centeredStyle.fontSize = 32;
            centeredStyle.normal.textColor = message.color;
            centeredStyle.alignment = TextAnchor.UpperCenter;
            float w = Screen.width * 0.7f;
            GUI.Label(new Rect((Screen.width - w) * 0.5f, Screen.height * message.offy, w, Screen.height * 0.2f), message.msg, centeredStyle);
        }
    }

    public static Game Instance() {
        return (gameInstance);
    }

    public Blinded GetBlinded() {
        return (this.blinded);
    }

    public void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            this.OnBack();
        }
    }

    public void birdPlaySound() {
        int soundID = this.rng.Next() % this.birdSounds.Length;
        AudioSource audio = this.birdSounds[soundID];
        audio.Play();
    }

    public void LateUpdate() {
        if (this.state == STATE_INGAME) {
            this.UpdateWorld();
        }
    }

    public void OnBack() {
        switch (this.state) {
            case STATE_MAIN_MENU:
                this.SetState(STATE_GAME_EXIT);
                break;

            case STATE_MAIN_MENU_OPTIONS:
                this.SetState(STATE_MAIN_MENU);
                break;

            case STATE_CREDIT:
                this.SetState(STATE_MAIN_MENU);
                break;

            case STATE_INGAME:
                this.SetState(STATE_INGAME_MENU);
                break;

            case STATE_INGAME_MENU:
                this.SetState(STATE_INGAME);
                break;

            case STATE_INGAME_MENU_OPTIONS:
                this.SetState(STATE_INGAME_MENU);
                break;
        }
    }

    public void UpdateWorld() {
        this.cameraController.LateUpdate(this);
        this.blinded.Update(this);
        this.UpdateHoveredObject();
    }

    public void UpdateHoveredObject() {
        this.hoveredActionnable = null;
        RaycastHit hit = this.cameraController.GetHit();
        if (hit.collider == null) {
            return;
        }
        for (int i = 0; i < this.actionnable.Length; i++) {
            GameObject obj = this.actionnable[i];
            if (obj.GetComponent<Collider>() == hit.collider) {
                this.hoveredActionnable = obj;
                break;
            }
        }
    }

    public void log(string msg) {
        print(msg);
    }

    public void SetState(int state) {
        int prevState = this.state;
        this.state = state;

        this.Refresh(prevState);
    }

    private void Refresh(int prevState) {
        Cursor.visible = true;

        switch (this.state) {
            case STATE_MAIN_MENU:
                this.world.SetActive(false);
                this.optionMenu.enabled = false;
                this.pauseMenu.enabled = false;
                this.mainMenu.enabled = true;
                this.creditMenu.enabled = false;
                break;
            case STATE_MAIN_MENU_OPTIONS:
                this.world.SetActive(false);
                this.optionMenu.enabled = true;
                this.pauseMenu.enabled = false;
                this.mainMenu.enabled = false;
                this.creditMenu.enabled = false;
                break;
            case STATE_CREDIT:
                this.world.SetActive(false);
                this.optionMenu.enabled = false;
                this.pauseMenu.enabled = false;
                this.mainMenu.enabled = false;
                this.creditMenu.enabled = true;
                break;
            case STATE_INGAME:
                if (prevState == STATE_MAIN_MENU) {
                    this.RestartGame();
                }
                this.world.SetActive(true);
                this.optionMenu.enabled = false;
                this.pauseMenu.enabled = false;
                this.mainMenu.enabled = false;
                this.creditMenu.enabled = false;
                Cursor.visible = false;
                break;
            case STATE_INGAME_MENU:
                this.world.SetActive(true);
                this.optionMenu.enabled = false;
                this.pauseMenu.enabled = true;
                this.creditMenu.enabled = false;
                this.mainMenu.enabled = false;
                break;
            case STATE_INGAME_MENU_OPTIONS:
                this.world.SetActive(false);
                this.optionMenu.enabled = true;
                this.pauseMenu.enabled = false;
                this.mainMenu.enabled = false;
                this.creditMenu.enabled = false;
                break;
            case STATE_GAME_EXIT:
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit ();
#endif
                break;
            default:
                break;
        }
    }

    public void RestartGame() {
        //TODO  replace each components
    }

    public GameObject[] GetActionnables() {
        return (this.actionnable);
    }

    public GameObject GetActionnable(int index) {
        return (this.actionnable[index]);
    }

    public GameObject GetHoveredActionnable() {
        return (this.hoveredActionnable);
    }

    public bool IsBirdActivable(int hoveredIndex) {
        return (hoveredIndex == BUTTON_1 || hoveredIndex == BUTTON_2 || hoveredIndex == BUTTON_3);
    }

    private bool door3_is_openned = false;

    public void CheckPressurePlaque() {
        if (this.plaque1.tag == PressurePlaque.ACTIVE && this.plaque2.tag == PressurePlaque.ACTIVE && !door3_is_openned) {
            door3_is_openned = true;
            this.door3.transform.Rotate(0, 0, -90);
            this.door3.transform.Translate(0.5f, 0.5f, 0.0f);
            this.block1.GetComponent<AudioSource>().Play();
            this.stackMessage(12.0f, "\"I think I hear someone not far from here ... Could it remains a guardian?\"", Color.white, 0.85f);
        }
    }
}
