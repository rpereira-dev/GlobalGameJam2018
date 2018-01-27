using UnityEngine;
using UnityEngine.UI;

public class KeyBindingButton : MonoBehaviour {

    public Text     nameLabel;
    public Button   keyCode;

    // Use this for initialization
    void Start() {
        keyCode.onClick.AddListener(HandleClick);
    }

    public void AddButton(KeyBindingScrollList list, string nameID, string keyCode) {
        this.nameLabel.text                                 = nameID;
        this.keyCode.GetComponentInChildren<Text>().text    = keyCode;
    }

    public void HandleClick() {
        //TODO : make button enable and wait input
      //  scrollList.TryTransferItemToOtherShop(item);
    }
}