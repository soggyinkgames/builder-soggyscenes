namespace SoggyInkGames.SoggyPackage.Editor
{
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(YourScriptName))]
    public class SoggyPackageEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            YourScriptName script = (YourScriptName)target;

            if (GUILayout.Button("Do Something in Editor"))
            {
                Debug.Log("Editor button pressed on " + script.gameObject.name);
            }
        }
    }
}