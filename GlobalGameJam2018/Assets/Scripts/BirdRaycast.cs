using UnityEngine;

public class BirdRaycast : MonoBehaviour {

    enum BIRD_ORDER {
        MOVE,
        ACTION
    };

    /** the point raycasted */
    private RaycastHit hit;

    void FixedUpdate() {
        Physics.Raycast(transform.position, transform.forward, out hit);
        if (Input.GetKeyDown(KeyCode.C)) {
            print(hit);
        } else if (Input.GetKeyDown(KeyCode.V)) {
            print(hit);
        }
    }
}
