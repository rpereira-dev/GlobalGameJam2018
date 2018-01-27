using UnityEngine;
using System;

/** Camera and bird controller */
public class BirdCameraController : MonoBehaviour {

    /** the bird */
    public GameObject bird;
    public GameObject selection;

    /** distance from the bird */
    public float speed = 0.2f;
    public float zoomSpeed = 0.2f;
    public float rotSpeed = 40.0f;

    /** polar coordinates */
    public static float MIN_DISTANCE = 0.4f;
    public float distance = 10.0f;
    private float phi = 0;
    private float theta = 0;

    /** camera rotation relatively to the bird */
    public float alpha;
    public float beta;

    /** raycast result */
    private RaycastHit hitInfo;

    public void Update() {
        this.UpdateInput();
        this.UpdateSelection();
    }

    /** handle bird input */
    private void UpdateInput() {

        /** input here */
        Vector3 f = bird.transform.forward;
        if (Input.GetKey(KeyCode.A)) {
            bird.transform.position += this.speed * Vector3.up;
        }
        if (Input.GetKey(KeyCode.E)) {
            bird.transform.position -= this.speed * Vector3.up;
        }
        if (Input.GetKey(KeyCode.Z)) {
            bird.transform.position += this.speed * f;
        }
        if (Input.GetKey(KeyCode.S)) {
            bird.transform.position -= this.speed * f;
        }
        if (Input.GetKey(KeyCode.D)) {
            bird.transform.position += this.speed * new Vector3(f.z, 0, -f.x);
        }
        if (Input.GetKey(KeyCode.Q)) {
            bird.transform.position += this.speed * new Vector3(-f.z, 0, f.x);
        }

        /** bird is floating */
        bird.transform.position += (float)Math.Sin(Time.time * this.speed * 64f) * 0.003f * Vector3.up;

        /** camera distance */
        this.distance -= Math.Sign(Input.mouseScrollDelta.y) * this.zoomSpeed;

        /** position 3rd person camera */
        this.phi += Input.GetAxis("Mouse X") * rotSpeed;
        this.theta -= Input.GetAxis("Mouse Y") * rotSpeed;

        float rx = (float)(this.theta - Math.PI / 2);
        float ry = (float)(this.phi - Math.PI);
        float rz = 0;
        transform.rotation = Quaternion.Euler(rx, ry, rz);
        transform.position = bird.transform.position - this.distance * transform.forward;

        /** 1st person */
        if (this.distance < MIN_DISTANCE) {
            print("INF A MIN_DISTANCE");
            transform.position = bird.transform.position;
            /** bird rotation */
            bird.transform.rotation = transform.rotation;
        } else {
            print(this.distance);

            /** else offset bird to bot left screen */
            Vector3 horizontal = new Vector3(transform.forward.z, 0, -transform.forward.x).normalized;
            transform.position += horizontal * (float)Math.Sin(this.alpha) * this.distance;

            Vector3 vertical = Vector3.up;
            transform.position += vertical * (float)Math.Sin(this.beta) * this.distance;

            /** bird rotation */
            bird.transform.rotation = transform.rotation;
        }
    }

    /** update looking-at selection of the bird */
    private void UpdateSelection() {
        var layerMask = ~(1 << 8); /** bird layer is 8 */
        if (Physics.Raycast(bird.transform.position, bird.transform.TransformDirection(Vector3.forward), out hitInfo, Mathf.Infinity, layerMask)) {
            this.selection.transform.position = hitInfo.point;
        }
    }

}