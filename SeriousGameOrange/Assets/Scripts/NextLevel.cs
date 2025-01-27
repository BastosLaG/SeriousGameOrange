using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour {
    [SerializeField] private int LevelTravel;
    public bool isLock = true;

    private void OnTriggerStay2D(Collider2D other) {
        if (other.CompareTag("Player") && !isLock) {
            if (Input.GetKeyDown(KeyCode.E)) {
                SceneManager.LoadScene(LevelTravel);
            }
        }
    }

    public void Unlock() {
        isLock = false;
    }
    
    public void WhichMap(int level) {
        LevelTravel = level;
    }
}