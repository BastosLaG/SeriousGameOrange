using EasyDialogue;
using UnityEngine;
using UnityEngine.Events;

public class StartDialogue : MonoBehaviour {
    public EasyDialogueGraph graphToPlay;
    [SerializeField] private bool canBeReplayed;
    [SerializeField] private UnityEvent callback;
    
    private bool hasBeenPlayed;
    private void OnTriggerEnter2D(Collider2D other) {
        if(hasBeenPlayed && !canBeReplayed) return;
        if(other.CompareTag("Player")) {
            hasBeenPlayed = true;
            DialoguesManager.Instance.StartDialogueEncounter(ref graphToPlay, callback);
        }
    }
    
    public void UnlockTheDoor() {
        NextLevel[] doors = FindObjectsByType<NextLevel>(FindObjectsSortMode.None);
        foreach (NextLevel door in doors) {
            door.Unlock();
        }
    }
}
