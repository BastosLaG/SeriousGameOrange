using UnityEngine;

public class Settings : MonoBehaviour
{
    [SerializeField] GameObject settingsPanel;

    public void OnSettingsClick()
    {
        settingsPanel.SetActive(true);
    }
}
