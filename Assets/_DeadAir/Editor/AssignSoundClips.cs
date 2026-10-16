using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class AssignSoundClips
{
    [MenuItem("Dead Air/Assign Sound Clips")]
    public static void Assign()
    {
        foreach (SoundClips sounds in Object.FindObjectsByType<SoundClips>(FindObjectsSortMode.None))
        {
            Undo.RecordObject(sounds, "Assign Sound Clips");
            foreach (SoundClips.Sound sound in sounds.sounds)
            {
                sound.clip = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/_DeadAir/Audio/SFX/" + sound.role + ".wav");
                if (sound.clip == null) Debug.LogWarning("No sound file for " + sound.role + "; using a generated placeholder.");
            }
            EditorUtility.SetDirty(sounds);
            EditorSceneManager.MarkSceneDirty(sounds.gameObject.scene);
        }
    }
}
