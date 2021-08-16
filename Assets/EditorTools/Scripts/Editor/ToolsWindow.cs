using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace EditorTools
{
    public class ToolsWindow : EditorWindow
    {
        [MenuItem("G2W/Window/Editor Tools")]
        public static void OpenJsonToolWindow()
        {
            ToolsWindow window = (ToolsWindow)GetWindow(typeof(ToolsWindow), false, "Editor Tools");
            window.Show();
        }

        void OnEnable()
        {
            toolsBase = new List<ITools>();
        }

        private int toolbarIndex = 0;
        private string[] toolbarStrings = { "1", "2", "3" };

        private void OnGUI()
        {
            List<ITools> toolsBaseList = GetToolList();
            if (toolsBaseList != null && toolsBaseList.Count > 0)
            {
                toolbarStrings = new string[toolsBaseList.Count];
                for (int i = 0; i < toolbarStrings.Length; i++)
                {
                    toolbarStrings[i] = toolsBaseList[i].GetName;
                }

                toolbarIndex = GUILayout.Toolbar(toolbarIndex, toolbarStrings);

                if (toolsBaseList[toolbarIndex] != null)
                {
                    toolsBaseList[toolbarIndex].DoUpdate();
                }
            }
        }

        private static List<ITools> toolsBase = new List<ITools>();
        private static List<ITools> GetToolList()
        {
            ITools tool = ScriptableObject.CreateInstance<ContentSegregateTools>();
            if(toolsBase.Any(arg => arg.GetName == tool.GetName) == false)
            {
                toolsBase.Add(tool);
            }

            return toolsBase;
        }
    }
}
