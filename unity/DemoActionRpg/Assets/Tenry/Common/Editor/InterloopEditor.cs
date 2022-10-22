using UnityEditor;
using UnityEngine;

namespace Tenry.Common.Editor {
  [CustomEditor(typeof(Interloop))]
  public class InterloopEditor : UnityEditor.Editor {
    private InterloopTrackEditor trackEditor;

    public override void OnInspectorGUI() {
      DrawDefaultInspector();

      var interloop = target as Interloop;

      if (interloop == null || interloop.Track == null) {
        return;
      }

      UnityEditor.Editor editor = trackEditor;
      UnityEditor.Editor.CreateCachedEditor(interloop.Track, typeof(InterloopTrackEditor), ref editor);
      trackEditor = editor as InterloopTrackEditor;

      if (trackEditor != null) {
        trackEditor.DrawBar((float) interloop.OffsetInSeconds);
      }

      if (!Application.isPlaying || !interloop.isActiveAndEnabled) {
        return;
      }

      GUILayout.BeginHorizontal();

      GUI.enabled = !interloop.IsPlaying;
      if (GUILayout.Button("Play")) {
        interloop.Play();
      }

      GUI.enabled = interloop.IsPlaying;
      if (GUILayout.Button("Pause")) {
        interloop.Pause();
      }

      if (GUILayout.Button("Stop")) {
        interloop.Stop();
      }

      GUILayout.EndHorizontal();

      GUI.enabled = true;

      if (interloop.IsPlaying) {
        Repaint();
      }
    }
  }
}
