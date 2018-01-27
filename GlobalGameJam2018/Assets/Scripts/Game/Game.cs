using UnityEngine;
using System;

public class Game : MonoBehaviour {

    private static Game gameInstance;

    private const int STATE_MAIN_MENU           = 0;
    private const int STATE_MAIN_MENU_OPTIONS   = 1;
    private const int STATE_INGAME              = 2;
    private const int STATE_INGAME_MENU         = 3;
    private const int STATE_INGAME_MENU_OPTIONS = 4;
    private const int STATE_GAME_EXIT           = 5;

    private int state;
    private CameraController cameraController;
    private Blinded blinded;
    private System.Random rng;

    public Canvas mainMenu;
    public Canvas pauseMenu;
    public Canvas optionMenu;
    public GameObject cam;
    public GameObject world;
    public GameObject birdObject;
    public GameObject blindedObject;
    public GameObject birdSelectionObject;
    public AudioSource[] birdSounds;

    public void Start() {
        gameInstance = this;

        Controls.Setup();
        this.cameraController = new CameraController(this.cam, this.birdObject, this.birdSelectionObject);
        this.blinded = new Blinded(blindedObject);
        this.rng = new System.Random();

        this.SetState(STATE_MAIN_MENU);
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
        switch (this.state) {
            case STATE_MAIN_MENU:
                this.world.SetActive(false);
                this.optionMenu.enabled = false;
                this.pauseMenu.enabled = false;
                this.mainMenu.enabled = true;
                break;
            case STATE_MAIN_MENU_OPTIONS:
                this.world.SetActive(false);
                this.optionMenu.enabled = true;
                this.pauseMenu.enabled = false;
                this.mainMenu.enabled = false;
                break;
            case STATE_INGAME:
                if (prevState == STATE_MAIN_MENU) {
                    this.RestartGame();
                }
                this.world.SetActive(true);
                this.optionMenu.enabled = false;
                this.pauseMenu.enabled = false;
                this.mainMenu.enabled = false;
                break;
            case STATE_INGAME_MENU:
                this.world.SetActive(true);
                this.optionMenu.enabled = false;
                this.pauseMenu.enabled = true;
                this.mainMenu.enabled = false;
                break;
            case STATE_INGAME_MENU_OPTIONS:
                this.world.SetActive(false);
                this.optionMenu.enabled = true;
                this.pauseMenu.enabled = false;
                this.mainMenu.enabled = false;
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

}
