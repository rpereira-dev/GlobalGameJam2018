using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestForce : MonoBehaviour
{

	// Use this for initialization
	IEnumerator Start () {
        yield return new WaitForSeconds(3.0f);

        GetComponent<Rigidbody>().AddForce(200*Vector3.down);
	}

    private void Update() {
        //print(GetComponent<Rigidbody>().velocity.magnitude);
    }
}
