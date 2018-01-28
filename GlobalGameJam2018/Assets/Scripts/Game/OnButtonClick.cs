using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnButtonClick : MonoBehaviour {

    public void SetGameState(int state) {
        Game game = Game.Instance();
        if (game.GetState() == Game.STATE_INGAME) {
            return;
        }
        Game.Instance().SetState(state);
    }

    public void onOptionBack() {
        Game game = Game.Instance();
        if (game.GetState() == Game.STATE_INGAME) {
            return;
        }
        Game.Instance().OnBack();
    }
}
