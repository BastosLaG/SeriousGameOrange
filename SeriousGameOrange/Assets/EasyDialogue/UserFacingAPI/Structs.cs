using UnityEngine;

namespace EasyDialogue {
    public struct DialogueLine {
        public Character Character;
        public string Text;

        public string[] PlayerResponces;
        public bool HasPlayerResponses() => PlayerResponces is { Length: > 0 };
    };
    
    [System.Serializable]
    public struct NodeDialogueOption {
        public string text;
        public Character associatedCharacter;
        public AnswerType goodAnswer; // -1 = not a good answer, 0 = neutral, 1 = good answer
        public string idMemory;
        
        public bool isExpanded;
        public Vector2 scrollPos;
    };
    
    public enum AnswerType {
        None,
        Good,
        Bad
    }
}
