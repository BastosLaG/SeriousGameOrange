using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;

namespace EasyDialogue {
    [CustomNodeEditor(typeof(EasyDialogueNode))]
    public class EasyDialogueNodeEditor : XNodeEditor.NodeEditor
    {
        private static GUILayoutOption[] textAreaOptions = new GUILayoutOption[]
{
            UnityEngine.GUILayout.Height(75),
            UnityEngine.GUILayout.MinWidth(100),
            UnityEngine.GUILayout.ExpandWidth(true),
            UnityEngine.GUILayout.ExpandHeight(false),
};
        private static GUILayoutOption[] scrollbarOptions = new GUILayoutOption[]
{
            UnityEngine.GUILayout.Height(75)
};

        private static string TruncateText(string _text, int _maxLen = 25)
        {
            if (_text.Length > _maxLen)
            {
                return $"{_text.Substring(0, _maxLen)}...";
            }
            else
            {
                return _text;
            }
        }

        private static string GetResponseText(string _input)
        {
            return $"Text: {TruncateText(_input)}";
        }

        EasyDialogueGraph currGraph;
        EasyDialogueNode easyDialogueNode;

        public override void AddContextMenuItems(GenericMenu menu)
        {
            base.AddContextMenuItems(menu);
            XNode.Node node = Selection.activeObject as XNode.Node;
            if (Selection.objects.Length == 1 && Selection.activeObject is EasyDialogueNode)
            {
                menu.AddItem(new GUIContent("Make Root Node"), false, () =>
                {
                    ((EasyDialogueGraph)target.graph).SetRootNode((EasyDialogueNode)node);
                });
            }
        }

        public override void OnCreate()
        {
            InitializeSerializedProperties();
        }

        public override void OnHeaderGUI()
        {
            currGraph = (EasyDialogueGraph)target.graph;
            if (currGraph.GetRootNode() == target)
            {
                UnityEngine.GUILayout.Label($"{target.name} (ROOT NODE)", XNodeEditor.NodeEditorResources.styles.nodeHeader, UnityEngine.GUILayout.Height(30));
            }
            else
            {
                UnityEngine.GUILayout.Label(target.name, XNodeEditor.NodeEditorResources.styles.nodeHeader, UnityEngine.GUILayout.Height(30));
            }
        }

        public override void OnBodyGUI()
        {
            if(easyDialogueNode == null)
            {
                InitializeSerializedProperties();
            }

            serializedObject.Update();

            DrawTopLevelPorts();
            string titleText = "Character Dialogue";
            string dialogueText = easyDialogueNode.characterDialogue.text;
            if (dialogueText != null && dialogueText != "")
            {
                titleText = TruncateText(dialogueText, 30);
            }

            easyDialogueNode.showCharacterDialogue = EditorGUILayout.BeginFoldoutHeaderGroup(easyDialogueNode.showCharacterDialogue, titleText);
            if (easyDialogueNode.showCharacterDialogue)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Associated Character");
                easyDialogueNode.characterDialogue.associatedCharacter = (Character) EditorGUILayout.ObjectField(easyDialogueNode.characterDialogue.associatedCharacter, typeof(Character), false);
                EditorGUILayout.EndHorizontal();

                GUIStyle style = new GUIStyle(EditorStyles.textArea);
                style.wordWrap = true;
                //easyDialogueNode.characterDialogue.scrollPos = EditorGUILayout.BeginScrollView(easyDialogueNode.characterDialogue.scrollPos, scrollbarOptions);
                string tmpText = EditorGUILayout.TextArea(easyDialogueNode.characterDialogue.text, style, textAreaOptions);
                //EditorGUILayout.EndScrollView();

                easyDialogueNode.characterDialogue.text = tmpText;
            }


            EditorGUILayout.EndFoldoutHeaderGroup();

            //TODO(chris): Draw has player responses bool
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Has Player Responses");
            easyDialogueNode.hasPlayerResponses = EditorGUILayout.Toggle(easyDialogueNode.hasPlayerResponses);
            EditorGUILayout.EndHorizontal();

            if (easyDialogueNode.hasPlayerResponses)
            {
                DrawPlayerResponses();
            }
            else
            {
                easyDialogueNode.ClearPlayerResponses();
            }
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawTopLevelPorts()
        {
            System.Collections.Generic.IEnumerable<XNode.NodePort> ports = easyDialogueNode.Ports;

            XNode.NodePort previousNodePort = null;
            XNode.NodePort nextNodePort = null;
            foreach (XNode.NodePort port in ports)
            {
                if (port.fieldName == "previousNodes")
                {
                    previousNodePort = port;
                }
                else if (port.fieldName == "nextNode")
                {
                    nextNodePort = port;
                }
            }

            //If we are the root node, we do NOT want a previous node port.
            if (currGraph.GetRootNode() == target)
            {
                if (!easyDialogueNode.hasPlayerResponses)
                {
                    XNodeEditor.NodeEditorGUILayout.PortField(nextNodePort);
                }
                else
                {
                    EditorGUILayout.Space(30);
                }
            }
            else
            {
                if(!easyDialogueNode.hasPlayerResponses)
                {
                    XNodeEditor.NodeEditorGUILayout.PortPair(previousNodePort, nextNodePort);
                }
                else
                {
                    XNodeEditor.NodeEditorGUILayout.PortField(previousNodePort);
                }
            }
        }

        private void DrawPlayerResponses()
        {
            if (easyDialogueNode.playerResponses.Count == 0)
            {
                Debug.Log($"Adding Player Response! New count = {easyDialogueNode.playerResponses.Count}");
                easyDialogueNode.AddPlayerResponse();
            }

            EditorGUILayout.LabelField("Player Responses");

            System.Collections.Generic.IEnumerable<XNode.NodePort> ports = easyDialogueNode.Ports;

            ++EditorGUI.indentLevel;
            for (int responceIndex = 0; responceIndex < easyDialogueNode.playerResponses.Count; ++responceIndex)
            {
                node_dialogue_option currResponse = easyDialogueNode.playerResponses[responceIndex];
                XNode.NodePort playerResponsePort = null;
                foreach (XNode.NodePort port in ports)
                {
                    if (port.fieldName == $"nextNode{responceIndex}")
                    {
                        playerResponsePort = port;
                    }
                }

                EditorGUILayout.BeginHorizontal();
                currResponse.isExpanded = EditorGUILayout.Foldout(easyDialogueNode.playerResponses[responceIndex].isExpanded, GetResponseText(easyDialogueNode.playerResponses[responceIndex].text), true);
                XNodeEditor.NodeEditorGUILayout.AddPortField(playerResponsePort);
                EditorGUILayout.EndHorizontal();

                if (EditorGUILayout.BeginFadeGroup(currResponse.isExpanded ? 1 : 0))
                {
                    currResponse.scrollPos = EditorGUILayout.BeginScrollView(currResponse.scrollPos, GUILayout.Height(75));
                    currResponse.text = EditorGUILayout.TextArea(currResponse.text, textAreaOptions);
                    EditorGUILayout.EndScrollView();
                }
                EditorGUILayout.EndFadeGroup();
                easyDialogueNode.playerResponses[responceIndex] = currResponse;
            }
            EditorGUILayout.BeginHorizontal();
            bool delete = GUILayout.Button("-");
            bool add = GUILayout.Button("+");
            EditorGUILayout.EndHorizontal();

            if(delete)
            {
                easyDialogueNode.RemovePlayerResponse(easyDialogueNode.playerResponses.Count-1);
            }
            if(add)
            {
                easyDialogueNode.AddPlayerResponse();
            }
            --EditorGUI.indentLevel;
            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        private void InitializeSerializedProperties()
        {
            easyDialogueNode = (EasyDialogueNode)target;
        }

        #region GUILine

        /// <summary>
        /// Draws a line in the node. Color is BLACK.
        /// </summary>
        /// <param name="i_height">box height.</param>
        protected void GuiLine(int i_height = 1)
        {

            UnityEngine.Rect rect = EditorGUILayout.GetControlRect(false, i_height);

            rect.height = i_height;

            EditorGUI.DrawRect(rect, new UnityEngine.Color(0.0f, 0.0f, 0.0f, 1));
        }

        /// <summary>
        /// Draws a line in the node with specified color.
        /// </summary>
        /// <param name="_color">color of box.</param>
        /// <param name="i_height">box height.</param>
        protected void GuiLine(UnityEngine.Color _color, int i_height = 1)
        {

            UnityEngine.Rect rect = EditorGUILayout.GetControlRect(false, i_height);

            rect.height = i_height;

            EditorGUI.DrawRect(rect, _color);
        }

        #endregion
    }
}
