using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskAction : Task {

    public GameObject actionnable;
    private TaskMove move;

    public TaskAction(GameObject actionnable) {
        this.actionnable = actionnable;
        this.move = new TaskMove(this.actionnable.transform.position);
    }

    override
    public bool Update(Blinded blinded) {
        if (this.move != null && this.move.Update(blinded)) {
            this.move = null;
            return (false);
        }
        return (this.DoAction(Game.Instance(), blinded));
    }

    public bool DoAction(Game game, Blinded blinded) {
        if (this.actionnable == game.GetActionnable(0)) {
            game.log("PRESSED BUTTON");
            /* game.door ...*/
        } else if (this.actionnable == game.GetActionnable(1)) {
            game.log("PRESSED LEVIER");

        }
        return (false);
    }
}
