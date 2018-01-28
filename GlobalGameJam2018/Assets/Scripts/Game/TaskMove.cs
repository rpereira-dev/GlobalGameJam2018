using System;
using UnityEngine;

public class TaskMove : Task {

    private Vector3 destination;
    private float speed = 0.02f;
    private bool rotated = false;

    public TaskMove(Vector3 destination) {
        this.destination = destination;
    }

    override
	public bool Update (Blinded blinded) {
        Vector3 d = this.destination - blinded.AsGameObject().transform.position;
        Vector3 move = new Vector3(d.x, 0, d.z).normalized;
        bool isWalking = false;

        if (!this.rotated) {
            this.RotateBlinded(blinded, move);
        } else {
            this.MoveBlinded(blinded, move);
            isWalking = true;
        }
        bool stop = d.magnitude < 0.2f;
        blinded.GetAnimator().SetBool("blindedIsWalking", isWalking && !stop);
        return (stop);
	}

    private void MoveBlinded(Blinded blinded, Vector3 move) {
        blinded.AsGameObject().transform.position += this.speed * move;
    }

    private void RotateBlinded(Blinded blinded, Vector3 move) {
        GameObject gb = blinded.AsGameObject();
        Vector3 forward = gb.transform.forward;
        Vector3 forwardProj = new Vector3(forward.x, 0, forward.z);
        float angle = Mathf.Acos(Vector3.Dot(move, forwardProj)) / (float)(2.0f * Math.PI) * 360.0f;
        this.rotated = angle < 0.5f;
        if (this.rotated) {
            return;
        }
        float dTheta = 0.250229f * Math.Abs(angle) - 0.00114325f * angle * angle; /** lagrange interpolation */
        gb.transform.Rotate(new Vector3(0, Math.Sign(angle) * dTheta, 0.0f));
        this.rotated = angle < 1.0f;
    }
}
