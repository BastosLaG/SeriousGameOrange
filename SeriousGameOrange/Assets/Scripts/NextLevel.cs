using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour {
    [SerializeField] private int LevelTravel;
    public bool isLock = true;
    public bool playerInTrigger;

    private void Update() {
        if (playerInTrigger && !isLock && Input.GetKeyDown(KeyCode.E)) {
            SceneManager.LoadScene(LevelTravel);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            playerInTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            playerInTrigger = false;
        }
    }

    public void Unlock() {
        isLock = false;
    }
    
    public void WhichMap(int level) {
        LevelTravel = level;
    }
}