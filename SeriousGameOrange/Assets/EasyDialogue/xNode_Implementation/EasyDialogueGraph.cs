using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace EasyDialogue
{
    [CreateAssetMenu(fileName = "EasyDialogueGraph", menuName = "EasyDialogue/DialogueGraph")]
    public class EasyDialogueGraph : NodeGraph
    {
        #region Attributes

        [SerializeField] public ILocalGraphContext localGraphContext;

        private EasyDialogueNode currNode = null;
        [SerializeField, HideInInspector]
        private EasyDialogueNode root = null;

        //TODO(chris):Figure out what size this should be, user config? Does this grow dynamically?
        Stack<JumpNode> jumpStack = new Stack<JumpNode>(2);
        Stack<Character> overrideCharacters = new Stack<Character>(2);

        #endregion

        public void AddOverrideCharacter(ref Character _c)
        {
            overrideCharacters.Push(_c);
        }

        public void InitializeGraph()
        {
            currNode = root;
        }

        public override Node AddNode(Type type)
        {
            Node node = base.AddNode(type);
            if (nodes.Count == 1)
            {
                root = (EasyDialogueNode) node;
            }
            return node;
        }
        
        public void SetRootNode(EasyDialogueNode _node)
        {
            root = _node;
            NodePort port = root.GetPort("previousNodes");
            port.ClearConnections();
        }

        [System.Diagnostics.Contracts.Pure]
        public EasyDialogueNode GetRootNode()
        {
            return root;
        }

        [System.Diagnostics.Contracts.Pure]
        public EasyDialogueNode GetCurrentNode()
        {
            return currNode;
        }

        public DialogueLine GetCurrentDialogueLine()
        {
            Character charOverride = null;
            if (overrideCharacters.Count > 0 && overrideCharacters.Peek() != null)
            {
                charOverride = overrideCharacters.Peek();
            }
            return CreateDialogueLine(ref currNode, ref charOverride, ref localGraphContext);
        }

        public bool GoToNextNode(ushort _dialogueOption = 0)
        {
            Node nextNode = currNode.GetNextNode(_dialogueOption);
            if(nextNode is JumpNode)
            {
                JumpNode jn = (JumpNode)nextNode;
                jumpStack.Push(jn);
                nextNode = jn.jumpNode;
                if(jn.characterOverride != null)
                {
                    overrideCharacters.Push(jn.characterOverride);
                }
            }

            bool valid = nextNode != null;

            if(valid)
            {
                Debug.Assert(nextNode is EasyDialogueNode, "Next Node is not of type EasyDialogueNode");
                currNode = (EasyDialogueNode)nextNode;
            }
            else
            {
                if(jumpStack.Count > 0)
                {
                    JumpNode jmpNode = jumpStack.Pop();
                    if(jmpNode.characterOverride != null)
                    {
                        overrideCharacters.Pop();
                    }
                    nextNode = jmpNode.GetNextNode();

                    //TODO(chris):This and the code directly above are exactly the same, can we condence/make safer?
                    valid = nextNode != null;
                    if (valid)
                    {
                        Debug.Assert(nextNode is EasyDialogueNode, "Next Node is not of type EasyDialogueNode");
                        currNode = (EasyDialogueNode)nextNode;
                    }
                    else
                    {
                        jumpStack.Clear();
                        overrideCharacters.Clear();
                    }
                }
            }
            return valid;
        }

        public static DialogueLine CreateDialogueLine(ref EasyDialogueNode _node, ref Character _overrideCharacter, ref ILocalGraphContext _glc)
        {
            DialogueLine result = new DialogueLine();
            result.Text = "";
            result.Character = _node.characterDialogue.associatedCharacter;
            if(_overrideCharacter != null)
            {
                result.Character = _overrideCharacter;
            }

            result.Text = _node.characterDialogue.text;
            if(_glc != null)
            {
                result.Text = _glc.Evaluate(ref result.Text);
            }

            if(_node.playerResponses == null || _node.playerResponses.Count == 0)
            {
                result.PlayerResponces = null;
            }
            else
            {
                result.PlayerResponces = new string[_node.playerResponses.Count];

                for(int playerResponseIndex = 0; 
                    playerResponseIndex < _node.playerResponses.Count;
                    ++playerResponseIndex)
                {
                    result.PlayerResponces[playerResponseIndex] = _node.playerResponses[playerResponseIndex].text;
                    if (_glc != null)
                    {
                        result.PlayerResponces[playerResponseIndex] = _glc.Evaluate(ref result.PlayerResponces[playerResponseIndex]);
                    }
                }
            }

            return result;
        }
    }
}