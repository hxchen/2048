using UnityEditor;
using UnityEngine;

/// <summary>
/// Utility class for deleting the PlayerPrefs from within the editor.
/// </summary>
public class DeletePlayerPrefs {
    [MenuItem("Tools/2048/Delete PlayerPrefs", false, 1)]
    public static void DeleteAllPlayerPrefs() {
        PlayerPrefs.DeleteAll();
    }
}
