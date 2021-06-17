using UnityEditor;

[CustomEditor(typeof(UIManager))]
public class UIManagerEditor : Editor
{
    SerializedProperty creditsWindowEnabled;
    SerializedProperty winWindowEnabled;
    SerializedProperty gameOverWindowEnabled;
    SerializedProperty helpWindowEnabled;

    protected virtual void OnEnable()
    {
        creditsWindowEnabled = this.serializedObject.FindProperty("creditsWindowEnabled");
        winWindowEnabled = this.serializedObject.FindProperty("winWindowEnabled");
        gameOverWindowEnabled = this.serializedObject.FindProperty("gameOverWindowEnabled");
        helpWindowEnabled = this.serializedObject.FindProperty("helpWindowEnabled");
    }

    public override void OnInspectorGUI()
    {
        this.serializedObject.Update();

        UIManager manager = (UIManager)target;

        base.OnInspectorGUI();

        switch (manager.type)
        {
            case SceneType.MainMenu:
                EditorGUILayout.PropertyField(creditsWindowEnabled);
                break;
            case SceneType.Game:
                EditorGUILayout.PropertyField(winWindowEnabled);
                EditorGUILayout.PropertyField(gameOverWindowEnabled);
                EditorGUILayout.PropertyField(helpWindowEnabled);
                break;
        }

        this.serializedObject.ApplyModifiedProperties();
    }
}