using System;
using UnityEngine;

public class Blinded {

    private GameObject obj;
    private Animator anim;
    private Rigidbody rb;
    private Task task;

    public Blinded(GameObject obj) {
        this.obj = obj;
        this.anim = obj.GetComponent<Animator>();
        this.rb = obj.GetComponent<Rigidbody>();
        this.anim.SetBool("blindedIsWalking", false);
    }

    public void Update (Game game) {
        if (this.task != null) {
            if (this.task.Update(this)) {
                this.task = null;
            }
        }
	}

    public Animator GetAnimator() {
        return (this.anim);
    }

    public void setTask(Task task) {
        this.task = task;
    }

    public GameObject AsGameObject() {
        return (this.obj);
    }

    /** return true if the blinded man an reach the given activable */
    public bool canReach(GameObject hovered) {
        if (hovered == null) {
            return (false);
        }
        return (hovered.transform.position - this.AsGameObject().transform.position).magnitude < 0.5f;
    }
}
