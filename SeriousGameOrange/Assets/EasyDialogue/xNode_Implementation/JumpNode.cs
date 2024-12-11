using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace EasyDialogue
{
    [NodeWidth(200)]
    public class JumpNode : Node
    {
        [Input, UnityEngine.SerializeField]
        private EasyDialogueNode previousNodes;
        [Output, UnityEngine.SerializeField]
        private EasyDialogueNode nextNode;

        public EasyDialogueNode jumpNode;
        public Character characterOverride = null;

        public EasyDialogueNode GetNextNode()
        {
            NodePort resultPort = GetPort("nextNode");

            EasyDialogueNode result = null;
            List<EasyDialogueNode> connectedNodes = (List<EasyDialogueNode>)GetValue(resultPort);
            if (connectedNodes != null && connectedNodes.Count > 0)
            {
                result = connectedNodes[0];
            }

            return result;
        }

        public override object GetValue(NodePort port)
        {
            List<EasyDialogueNode> result = null;
            List<NodePort> otherPorts = null;

            if (port.fieldName == "nextNode")
            {
                otherPorts = GetOutputPort(port.fieldName).GetConnections();
            }
            else if (port.fieldName == "previousNodes")
            {
                otherPorts = GetInputPort(port.fieldName).GetConnections();
            }

            if (otherPorts != null && otherPorts.Count > 0)
            {
                result = new List<EasyDialogueNode>(otherPorts.Count);
                for (int portIndex = 0;
                    portIndex < otherPorts.Count;
                    ++portIndex)
                {
                    result.Add((EasyDialogueNode)otherPorts[portIndex].node);
                }
            }

            return result;
        }
    }
}