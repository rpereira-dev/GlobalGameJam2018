using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnButtonClick : MonoBehaviour {

    public void SetGameState(int state) {
        Game.Instance().SetState(state);
    }

    public void onOptionBack() {
        Game.Instance().onBack();
    }
}
