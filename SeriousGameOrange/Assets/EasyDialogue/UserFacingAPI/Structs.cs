using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EasyDialogue
{
    /// <summary>
    /// Stores all useful information about a line of dialogue
    /// </summary>
    public struct dialogue_line
    {
        public Character character;
        public string text;

        public string[] playerResponces;
        public bool HasPlayerResponses() => playerResponces != null && playerResponces.Length > 0;
    };

    //NOTE(chris):This struct is used for housing the info in the graph-view
    //TODO(chris): Do we need all the same options for a player response as an NPC? Probably not. separate those out to be more sparce.
    [System.Serializable]
    public struct node_dialogue_option
    {
        public string text;
        public Character associatedCharacter;

        //NOTE(chris):This is for Node editor visibility
        public bool isExpanded;
        public Vector2 scrollPos;
        //TODO(chris): Add in optional data to send (play animation, play sfx, etc.)
    };
}