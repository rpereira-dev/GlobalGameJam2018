using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskAction : Task {

    public int actionnable;
    private TaskMove move;

    public TaskAction(int actionnable) {
        this.actionnable = actionnable;
        this.move = new TaskMove(Game.Instance().GetActionnable(actionnable).transform.position);
    }

    override
    public bool Update(Blinded blinded) {
        if (this.move != null && this.move.Update(blinded)) {
            this.move = null;
            return (false);
        }
        return (this.DoAction(Game.Instance(), blinded));
    }


    private static bool door2_oppened = false;

    public bool DoAction(Game game, Blinded blinded) {
            switch (this.actionnable) {
                case (Game.LEVIER_1):
                    if (door2_oppened) {
                        break;
                    }
                    door2_oppened = true;
                    game.door2.transform.Rotate(0, 0, -90);
                    game.door2.transform.Translate(0.5f, 0.5f, 0.0f);
                game.PlayBlockSound();
                break;

                default:
                    game.log("this actionnable is not implemeted yet");
                    break;
        }
        return (true);
    }
}
