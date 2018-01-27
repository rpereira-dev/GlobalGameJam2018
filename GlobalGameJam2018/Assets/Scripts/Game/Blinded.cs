using UnityEngine;

public class Blinded {

    private GameObject obj;
    private Task task;

    public Blinded(GameObject obj) {
        this.obj = obj;
    }
	
	public void Update (Game game) {
		if (this.task != null) {
            if (this.task.Update(this)) {
                this.task = null;
            }
        }
	}
}
