using EasyDialogue;
using UnityEngine;

public class GoodOrBad : MonoBehaviour {
    [SerializeField] private StartDialogue startDialogue;
    [SerializeField] private EasyDialogueGraph bad;
    
    private void Start() {
        if (DialoguesManager.Instance.memory.Contains("ClientBadAnswer")) {
            startDialogue.graphToPlay = bad;
        }
    }
}
