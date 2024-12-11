using UnityEngine;
using System.Collections.Generic;

public class DialogueListe : MonoBehaviour {
    private List<DialogueEntry> dialogues;

    void Awake() {
        dialogues = new List<DialogueEntry>();
    }
    
    DialogueEntry GetDialogue(string id) {
        return dialogues.Find(dialogue => dialogue.Id == id);
    }

    public void AddDialogue(string id, string nom, string text) {
        dialogues.Add(new DialogueEntry("test1", "Onilaf", "Voici comment marche les dialogues", SuiteType.Dialogue, "test2"));
        dialogues.Add(new DialogueEntry("test2", "Onilaf", "Ce message fait suite à l'ancien", SuiteType.Question, "test3"));
        dialogues.Add(new DialogueEntry("test3", "Onilaf", "Voici comment marche les dialogues", SuiteType.Dialogue, "test3"));
        dialogues.Add(new DialogueEntry("test4", "Onilaf", "Voici comment marche les dialogues", SuiteType.Dialogue, "test4"));
    }
}

public struct DialogueEntry {
    private string id;
    private string nom;
    private string text;
    private SuiteType suite;
    private string idSuite;

    public DialogueEntry(string id, string nom, string text, SuiteType suite, string idSuite = null) {
        this.id = id;
        this.nom = nom;
        this.text = text;
        this.suite = suite;
        this.idSuite = idSuite;
    }
    
    public string Id { get { return id; } }
}

public enum SuiteType {
    Dialogue,
    Question,
    Fin,
}