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
        public bool goodAnswer;
        
        public bool isExpanded;
        public Vector2 scrollPos;
    };
}
