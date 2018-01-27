using UnityEngine;
using System;

/** Camera and bird controller */
public class CameraController {

    private static CameraController instance;

    /** the bird */
    public GameObject cam;
    public GameObject bird;
    public GameObject selection;

    public CameraController(GameObject cam, GameObject bird, GameObject selection) {
        instance = this;

        this.cam = cam;
        this.bird = bird;
        this.selection = selection;
    }

    /** bird speed */
    private float speed = 1.0f;

    /** polar coordinates */
    public float min_distance = 0.2f;
    public float distance = 1;
    public float phi = 0;
    public float theta = 0;

    /** camera rotation relatively to the bird */
    public float alpha = (float)(35.0f / 360.0f * (2.0f * Math.PI));
    public float beta  = (float)(10.0f / 360.0f * (2.0f * Math.PI));

    /** raycast result */
    private static int LAYER_MASK = ~(1 << 8); /** bird layer is 8 */
    private RaycastHit hitInfo;

    public void Start() {
        instance = this;
    }

    public static CameraController Instance() {
        return (instance);
    }

    public void Update(Game game) {
        this.UpdateInput();
        this.UpdateRotation();
        this.UpdateSelection();
    }

    private void UpdateInput() {
        /** input here */
        Vector3 f = bird.transform.forward;

        if (Controls.getKey(Controls.STRAFE_UP).isPressed()) {
            bird.transform.position += this.speed * Vector3.up;
        } else if (Controls.getKey(Controls.STRAFE_DOWN).isPressed()) {
            bird.transform.position -= this.speed * Vector3.up;
        }
        if (Controls.getKey(Controls.MOVE_FORWARD).isPressed()) {
            bird.transform.position += this.speed * f;
        } else if (Controls.getKey(Controls.MOVE_BACKWARD).isPressed()) {
            bird.transform.position -= this.speed * f;
        }
        if (Controls.getKey(Controls.STRAFE_LEFT).isPressed()) {
            bird.transform.position += this.speed * new Vector3(-f.z, 0, f.x);
        } else if (Controls.getKey(Controls.STRAFE_RIGHT).isPressed()) {
            bird.transform.position += this.speed * new Vector3(f.z, 0, -f.x);
        }

        /** bird is floating */
        bird.transform.position += (float)Math.Sin(Time.time * this.speed * 64f) * 0.003f * Vector3.up;

        /** camera distance */
        this.distance -= Math.Sign(Input.mouseScrollDelta.y) * Controls.getValue(Controls.ZOOM_SPEED).asFloat();

        /** position 3rd person camera */
        this.phi += Input.GetAxis("Mouse X") * Controls.getValue(Controls.ROT_SPEED).asFloat();
        this.theta -= Input.GetAxis("Mouse Y") * Controls.getValue(Controls.ROT_SPEED).asFloat();
    }

    /** handle bird and camera rotation */
    private void UpdateRotation() {
    
        float rx = (float)(this.theta - Math.PI / 2);
        float ry = (float)(this.phi - Math.PI);
        float rz = 0;
        cam.transform.rotation = Quaternion.Euler(rx, ry, rz);
        bird.transform.rotation = cam.transform.rotation;

        /** 1st person */
        if (this.distance < this.min_distance) {
            this.setFirstPerson();
        } else {
            this.setThirdPerson(this.distance);

            /** check collisions */
            if (Physics.Raycast(bird.transform.position, (cam.transform.position - bird.transform.position).normalized, out hitInfo, this.distance, LAYER_MASK)) {
                if (hitInfo.distance < this.min_distance) {
                    this.setFirstPerson();
                } else if (hitInfo.distance < this.distance) {
                    this.setThirdPerson(hitInfo.distance);
                }
            }
        }
    }

    /** set camera to first person */
    private void setFirstPerson() {
        cam.transform.position = bird.transform.position;
        bird.transform.rotation = cam.transform.rotation;
    }

    /** set camera to second person and offset toh have bird bottom left */
    private void setThirdPerson(float distanceFromBird) {
        /** else set 3rd person, and then offset bird to bot left screen */
        cam.transform.position = bird.transform.position - distanceFromBird * cam.transform.forward;

        Vector3 horizontal = new Vector3(cam.transform.forward.z, 0, -cam.transform.forward.x).normalized;
        cam.transform.position += horizontal * (float)Math.Tan(this.alpha) * distanceFromBird;

        Vector3 vertical = Vector3.up;
        cam.transform.position += vertical * (float)Math.Sin(this.beta) * distanceFromBird;
    }

    /** update looking-at selection of the bird */
    private void UpdateSelection() {
        if (Physics.Raycast(bird.transform.position, bird.transform.forward, out hitInfo, Mathf.Infinity, LAYER_MASK)) {
            this.selection.transform.position = hitInfo.point;
        }
    }
}