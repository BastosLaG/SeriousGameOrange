using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    [SerializeField] private int LevelTravel;
    public bool isLock = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isLock)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                SceneManager.LoadScene(LevelTravel);
            }
        }
    }
}
