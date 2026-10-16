using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class AssignVoiceClips
{
    [MenuItem("Dead Air/Assign Voice Clips")]
    public static void Assign()
    {
        foreach (VoiceLines voices in Object.FindObjectsByType<VoiceLines>(FindObjectsSortMode.None))
        {
            Undo.RecordObject(voices, "Assign Voice Clips");
            foreach (VoiceLines.Line line in voices.lines)
            {
                AudioClip match = null;
                foreach (string guid in AssetDatabase.FindAssets("t:AudioClip", new[] { "Assets/_DeadAir/Audio/Voice" }))
                {
                    string path = AssetDatabase.GUIDToAssetPath(guid);
                    if (System.IO.Path.GetFileNameWithoutExtension(path) == line.id)
                        match = AssetDatabase.LoadAssetAtPath<AudioClip>(path);
                }
                line.clip = match;
                if (match == null) Debug.LogWarning("No voice file for " + line.id + "; using a generated placeholder.");
            }
            EditorUtility.SetDirty(voices);
            EditorSceneManager.MarkSceneDirty(voices.gameObject.scene);
        }
    }
}
