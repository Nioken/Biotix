using UnityEditor;

[CustomEditor(typeof(Node))]
public class NodeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Node node = (Node)target;
        var serializedNode = new SerializedObject(node);
        SerializedProperty side = serializedNode.FindProperty("NodeSide");
        SerializedProperty units = serializedNode.FindProperty("UnitsCount");
        SerializedProperty greenMaterial = serializedNode.FindProperty("GreenMaterial");
        SerializedProperty redMaterial = serializedNode.FindProperty("RedMaterial");
        SerializedProperty whiteMaterial = serializedNode.FindProperty("WhiteMaterial");
        SerializedProperty unitPrefab = serializedNode.FindProperty("_unitPrefab");
        SerializedProperty selectionSprite = serializedNode.FindProperty("SelectionSprite");

        EditorGUILayout.PropertyField(side);
        EditorGUILayout.PropertyField(units);
        EditorGUILayout.PropertyField(greenMaterial);
        EditorGUILayout.PropertyField(redMaterial);
        EditorGUILayout.PropertyField(whiteMaterial);
        EditorGUILayout.PropertyField(unitPrefab);
        EditorGUILayout.PropertyField(selectionSprite);
        serializedNode.ApplyModifiedProperties();
        node.InitializeNode();
    }
}
