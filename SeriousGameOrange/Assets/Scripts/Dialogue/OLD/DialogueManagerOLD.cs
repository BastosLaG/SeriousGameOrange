using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour {
    public static DialogueManager Instance { get; private set; }
    
    [SerializeField] private Camera cam;
    [SerializeField] private RectTransform canvas;
    [SerializeField] private Transform followObject;
    
    [SerializeField] private RectTransform dialogueBox;
    [SerializeField] private TextMeshProUGUI dialogueNom;
    [SerializeField] private TextMeshProUGUI dialogueText;

    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }
    
    void Update() {
        // DEBUG: Mettre ces 2 lignes à la place de followObject.parent = where; dans SpeakDialogue
        Vector3 localPos = followObject.position - cam.transform.position;
        dialogueBox.localPosition = new Vector3(localPos.x * 106f, (localPos.y + 2.8f) * 106f, 0);
    }
    
    public void SpeakDialogue(Transform where, bool right, string who, string what) {
        followObject.parent = where;
        dialogueBox.localScale = new Vector3(right ? 1 : -1, 1, 1);
        dialogueNom.text = who;
        dialogueText.text = what;
        dialogueBox.gameObject.SetActive(true);
    }
}