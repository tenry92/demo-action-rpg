using UnityEditor;
using UnityEngine;

namespace Tenry.DemoActionRpg.Editor {
  [CustomEditor(typeof(ObjectSpawner))]
  public class ObjectSpawnerEditor : UnityEditor.Editor {
    public override void OnInspectorGUI() {
      DrawDefaultInspector();

      if (Application.isPlaying) {
        if (GUILayout.Button("Spawn")) {
          var spawner = target as ObjectSpawner;
          spawner.Spawn();
        }
      }
    }
  }
}
