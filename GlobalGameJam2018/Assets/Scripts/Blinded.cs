using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blinded : MonoBehaviour {

    private Task task;
	
	void Update () {
		if (this.task != null) {
            if (this.task.Update(this)) {
                this.task = null;
            }
        }
	}
}
