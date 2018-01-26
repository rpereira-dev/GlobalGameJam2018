using UnityEngine;
using System;

/** Camera and bird controller */
public class BirdCameraController : MonoBehaviour {

    /** the bird */
    public GameObject bird;

    /** distance from the bird */
    public float distance;
    public float rotSpeed;
    private float phi = 0;
    private float theta = 0;

    void Update() {

        /** camera distance */
        float dwheely = Input.mouseScrollDelta.y;
        this.distance += dwheely * 0.1f;

        /** camera rotation */
        this.phi   += Input.GetAxis("Mouse X") * rotSpeed;
        this.theta -= Input.GetAxis("Mouse Y") * rotSpeed;

        float rx = (float)(this.theta - Math.PI / 2);
        float ry = (float)(this.phi - Math.PI);
        float rz = 0;
        transform.rotation = Quaternion.Euler(rx, ry, rz);
        transform.position = bird.transform.position - this.distance * transform.forward;

        bird.transform.rotation = transform.rotation;
    }
}