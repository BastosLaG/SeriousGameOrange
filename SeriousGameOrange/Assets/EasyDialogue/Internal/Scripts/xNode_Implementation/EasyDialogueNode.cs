using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace EasyDialogue
{
    [NodeWidth(300)]
    public class EasyDialogueNode : Node
    {
        #region NodeAttributes

        [Input, UnityEngine.SerializeField]
        private EasyDialogueNode previousNodes;
        [Output, UnityEngine.SerializeField]
        private EasyDialogueNode nextNode;

        public CustomNodeInfo customNodeInfo;
        public node_dialogue_option characterDialogue;
        public bool hasPlayerResponses = false;
        public List<node_dialogue_option> playerResponses = new List<node_dialogue_option>();

        private const string dynamicNodePrefix = "nextNode";

        //NOTE(chris):This is for node editor things.
        public bool showCharacterDialogue = false;
        #endregion

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.Pure]
        private static string GetDynamicNodeName(int _index)
        {
            string result = $"{dynamicNodePrefix}{_index}";
            return result;
        }

        protected override void Init()
        {
            base.Init();

            for (int playerResponseIndex = 0;
                playerResponseIndex < playerResponses.Count;
                ++playerResponseIndex)
            {
                //NOTE(chris): Prevent generating warnings that I am recreating ports that already exist.
                if(GetPort(GetDynamicNodeName(playerResponseIndex)) == null)
                {
                    CreateDynamicPort(playerResponseIndex);
                }
            }
        }

        public override void OnCreateConnection(NodePort from, NodePort to)
        {
            base.OnCreateConnection(from, to);
            if(to.node is EasyDialogueNode && from.node is EasyDialogueNode)
            {
                EasyDialogueNode toNode = (EasyDialogueNode)to.node;
                if (toNode.characterDialogue.associatedCharacter == null)
                {
                    toNode.characterDialogue.associatedCharacter = ((EasyDialogueNode)from.node).characterDialogue.associatedCharacter;
                }
            }
        }

        public void AddPlayerResponse()
        {
            node_dialogue_option curr = new node_dialogue_option();
            curr.text = "";
            curr.isExpanded = false;
            playerResponses.Add(curr);
            CreateDynamicPort(playerResponses.Count-1);
        }

        public void RemovePlayerResponse(int _index)
        {
            string nodeName = GetDynamicNodeName(_index);
            RemoveDynamicPort(nodeName);
            playerResponses.RemoveAt(_index);
        }

        public void ClearPlayerResponses()
        {
            for(int playerResponseIndex = 0;
                playerResponseIndex < playerResponses.Count;
                ++playerResponseIndex)
            {
                RemoveDynamicPort(GetDynamicNodeName(playerResponseIndex));
            }
            playerResponses.Clear();
        }

        public Node GetNextNode(ushort _dialogueChoice = 0)
        {
            NodePort resultPort = GetPort("nextNode");
            if (hasPlayerResponses)
            {
                resultPort = GetPort(GetDynamicNodeName(_dialogueChoice));
            }

            Node result = null;
            List<Node> connectedNodes = null;
            if (resultPort != null)
            {
                connectedNodes = (List<Node>)GetValue(resultPort);
            }
            if(connectedNodes != null && connectedNodes.Count > 0)
            {
                result = connectedNodes[0];
            }

            return result;
        }

        // Return the correct value of an output port when requested
        public override object GetValue(NodePort port)
        {
            List<Node> result = null;
            List<NodePort> otherPorts = null;

            if (port.fieldName.StartsWith(dynamicNodePrefix))
            {
                otherPorts = GetOutputPort(port.fieldName).GetConnections();
            }
            else if (port.fieldName == "previousNodes")
            {
                otherPorts = GetInputPort(port.fieldName).GetConnections();
            }

            if (otherPorts != null && otherPorts.Count > 0)
            {
                result = new List<Node>(otherPorts.Count);
                for (int portIndex = 0;
                    portIndex < otherPorts.Count;
                    ++portIndex)
                {
                    result.Add(otherPorts[portIndex].node);
                }
            }

            return result;
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private void CreateDynamicPort(int _nodeNumber)
        {
            string nodeName = GetDynamicNodeName(_nodeNumber);
            NodePort newPort = AddDynamicOutput(typeof(EasyDialogueNode), ConnectionType.Override, TypeConstraint.Inherited, nodeName);
        }
    }
}