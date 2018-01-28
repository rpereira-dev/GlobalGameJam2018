using UnityEngine;
using System;

/** Camera and bird controller */
public class CameraController {

    private static CameraController instance;

    /** the bird */
    public GameObject cam;
    public GameObject bird;
    public GameObject selection;
    public Animator anim;

    public CameraController(GameObject cam, GameObject bird, GameObject selection) {
        instance = this;

        this.cam = cam;
        this.bird = bird;
        this.selection = selection;
        this.anim = this.bird.GetComponent<Animator>();
    }

    /** bird speed */
    private float speed = 0.05f;

    /** polar coordinates */
    public float min_distance = 0.2f;
    public float distance = 1;
    public float phi = 0;
    public float theta = 0;

    /** camera rotation relatively to the bird */
    public float alpha = (float)(35.0f / 360.0f * (2.0f * Math.PI));
    public float beta = (float)(10.0f / 360.0f * (2.0f * Math.PI));

    /** raycast result */
    private static int LAYER_MASK = ~(1 << 8); /** bird layer is 8 */
    private RaycastHit hitInfo;

    public void Start() {
        instance = this;
    }

    public RaycastHit GetHit() {
        return (this.hitInfo);
    }

    public static CameraController Instance() {
        return (instance);
    }

    public void LateUpdate(Game game) {
        this.UpdateInput();
        this.UpdateRotation();
        this.UpdateSelection();
    }

    private void UpdateInput() {
        /** input here */
        Vector3 f = bird.transform.forward;
        bool isMoving = false;
        bool isOnGround = false;

        /** check ground */
        RaycastHit hit;
        if (Physics.Raycast(this.bird.transform.position, Vector3.down, out hit, 0.1f)) {
            bird.transform.position = bird.transform.position + new Vector3(0, -hit.distance + 0.01f, 0);
            isOnGround = true;

            if (Controls.getKey(Controls.STRAFE_UP).isPressed()) {
                bird.transform.position += this.speed * Vector3.up * 2.0f;
                isOnGround = false;
            }

            if (Controls.getKey(Controls.MOVE_FORWARD).isPressed()) {
                bird.transform.position += this.speed * f * 0.2f;
                isMoving = true;
            } else if (Controls.getKey(Controls.MOVE_BACKWARD).isPressed()) {
                bird.transform.position -= this.speed * f * 0.2f;
                isMoving = true;
            }
        } else {
            if (Controls.getKey(Controls.STRAFE_LEFT).isPressed()) {
                bird.transform.position += this.speed * new Vector3(-f.z, 0, f.x);
                isMoving = true;
            } else if (Controls.getKey(Controls.STRAFE_RIGHT).isPressed()) {
                bird.transform.position += this.speed * new Vector3(f.z, 0, -f.x);
                isMoving = true;
            }

            if (Controls.getKey(Controls.STRAFE_UP).isPressed()) {
                bird.transform.position += this.speed * Vector3.up;
                isOnGround = false;
            } else if (Controls.getKey(Controls.STRAFE_DOWN).isPressed()) {
                bird.transform.position -= this.speed * Vector3.up;
                isOnGround = false;
            }

            if (Controls.getKey(Controls.MOVE_FORWARD).isPressed()) {
                bird.transform.position += this.speed * f;
                isMoving = true;
            } else if (Controls.getKey(Controls.MOVE_BACKWARD).isPressed()) {
                bird.transform.position -= this.speed * f;
                isMoving = true;
            }
        }

        /** bird is floating */
        bird.transform.position += (float)Math.Sin(Time.time * this.speed * 64f) * 0.003f * Vector3.up;

        /** camera distance */
        this.distance -= Math.Sign(Input.mouseScrollDelta.y) * Controls.getValue(Controls.ZOOM_SPEED).asFloat();

        /** position 3rd person camera */
        this.phi += Input.GetAxis("Mouse X") * Controls.getValue(Controls.ROT_SPEED).asFloat();
        this.theta -= Input.GetAxis("Mouse Y") * Controls.getValue(Controls.ROT_SPEED).asFloat();
        this.theta = Mathf.Clamp(this.theta, -70, 70);

        this.anim.SetBool("birdIsMoving", isMoving);
        this.anim.SetBool("birdIsOnGround", isOnGround);
    }

    /** handle bird and camera rotation */
    private void UpdateRotation() {

        float rx = this.theta;
        float ry = this.phi;
        float rz = 0;
        cam.transform.rotation = Quaternion.Euler(rx, ry, rz);
        bird.transform.rotation = cam.transform.rotation;

        /** 1st person */
        if (this.distance < this.min_distance) {
            this.SetFirstPerson();
        } else {
            this.SetThirdPerson(this.distance);

            /** check collisions */
            if (Physics.Raycast(bird.transform.position, (cam.transform.position - bird.transform.position).normalized, out hitInfo, this.distance, LAYER_MASK)) {
                if (hitInfo.distance < this.min_distance) {
                    this.SetFirstPerson();
                } else if (hitInfo.distance < this.distance) {
                    this.SetThirdPerson(hitInfo.distance * 0.9f);
                }
            }
        }
    }

    /** set camera to first person */
    private void SetFirstPerson() {
        cam.transform.position = bird.transform.position + 0.1f * (Vector3.up + bird.transform.forward);
        bird.transform.rotation = cam.transform.rotation;
    }

    /** set camera to second person and offset toh have bird bottom left */
    private void SetThirdPerson(float distanceFromBird) {
        /** else set 3rd person, and then offset bird to bot left screen */
        cam.transform.position = bird.transform.position - distanceFromBird * cam.transform.forward;

      /*  Vector3 horizontal = new Vector3(cam.transform.forward.z, 0, -cam.transform.forward.x).normalized;
        cam.transform.position += horizontal * (float)Math.Tan(this.alpha) * distanceFromBird;
        */
        Vector3 vertical = Vector3.up;
        cam.transform.position += vertical * (float)Math.Sin(this.beta) * distanceFromBird;
    }

    /** update looking-at selection of the bird */
    private void UpdateSelection() {
        if (Physics.Raycast(bird.transform.position, bird.transform.forward, out hitInfo, Mathf.Infinity, LAYER_MASK)) {
            this.selection.SetActive(hitInfo.distance > 0.5f);
            this.selection.transform.position = hitInfo.point;

            Game game = Game.Instance();
            GameObject hovered = game.GetHoveredActionnable();
            this.selection.GetComponent<Renderer>().material.color = (hovered != null) ? new Color(0, 1, 0, 0.3f) : new Color(1, 0, 0, 0.3f);

            int hoveredIndex = -1;
            if (hovered != null) {
                for (int index = 0; index < Game.Instance().GetActionnables().Length; index++) {
                    if (game.GetActionnable(index) == hovered) {
                        hoveredIndex = index;
                        break;
                    }
                }
            }
            if (hovered != null && Controls.getKey(Controls.BIRD_ACTION).isPressed()) {

                if ((this.bird.transform.position - hovered.transform.position).magnitude > 2.0f) {
                    game.stackMessage(1.0f, "The bird is out of range!", Color.red);
                } else if (!game.IsBirdActivable(hoveredIndex)) {
                    game.stackMessage(1.0f, "This item cannot be activated by the bird", Color.red);
                } else {
                    game.birdPlaySound();
                    this.BirdAction(hoveredIndex);
                }
            }

            if (Controls.getKey(Controls.BLINDED_ACTION).isPressed()) {
                if (hovered != null) {
                    if (!game.GetBlinded().canReach(hovered)) {
                        game.stackMessage(1.0f, "The blinded is out of range!", Color.red);
                    } else if (game.IsBirdActivable(hoveredIndex)) {
                        game.stackMessage(1.0f, "This item cannot be activated by the blinded", Color.red);
                    } else {
                        game.birdPlaySound();
                        game.GetBlinded().setTask(new TaskAction(hoveredIndex));
                    }
                } else {
                    game.birdPlaySound();
                    game.GetBlinded().setTask(new TaskMove(hitInfo.point));
                }
            }
        }
    }


    private static bool door1_oppened = false;
    private static bool door2_oppened = false;

    private void BirdAction(int actionnable) {
        Game game = Game.Instance();

        switch (actionnable) {
            case Game.BUTTON_1:
                if (!door1_oppened) {
                    door1_oppened = true;
                    game.door1.transform.Rotate(0, 0, -90);
                    game.door1.transform.Translate(0.5f, 0.5f, 0.0f);
                }
                break;
            case Game.BUTTON_2:
                if (!door2_oppened) {
                    door2_oppened = true;
                    game.block1.GetComponent<Rigidbody>().useGravity = true;
                    game.log("BLOCK FALL");
                }
                break;
            default:
                break;
        }
    }
}