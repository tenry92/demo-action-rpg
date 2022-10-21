using UnityEngine;
using UnityEditor;

using Tenry.Utils;

namespace Tenry.Common.Editor {
  [CustomEditor(typeof(Interloop))]
  public class InterloopEditor : UnityEditor.Editor {
    private static float GetRelativeOffset(AudioClip clip, double offset) {
      return ((float) offset) / clip.length;
    }

    private static void DrawRelativeLine(Rect r, float offset, Color color) {
      r = new Rect(r.x + r.width * offset, r.y, 1f, r.height);
      EditorGUI.DrawRect(r, color);
    }

    public override void OnInspectorGUI() {
      DrawDefaultInspector();

      var interloop = target as Interloop;

      if (interloop == null) {
        return;
      }

      Rect r = EditorGUILayout.GetControlRect();
      EditorGUI.DrawRect(r, MoreColors.SteelBlue);

      DrawRelativeLine(r, GetRelativeOffset(interloop.Clip, interloop.LoopStart), MoreColors.Coral);
      DrawRelativeLine(r, GetRelativeOffset(interloop.Clip, interloop.LoopEnd), MoreColors.OrangeRed);

      DrawRelativeLine(r, GetRelativeOffset(interloop.Clip, interloop.OffsetInSeconds), MoreColors.Red);

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
