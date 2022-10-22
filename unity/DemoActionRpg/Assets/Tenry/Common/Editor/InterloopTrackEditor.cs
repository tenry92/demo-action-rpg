using UnityEngine;
using UnityEditor;

using Tenry.Utils;

namespace Tenry.Common.Editor {
  [CustomEditor(typeof(InterloopTrack))]
  public class InterloopTrackEditor : UnityEditor.Editor {
    private InterloopTrack Track => target as InterloopTrack;

    private float GetRelativeOffset(double offset) {
      return ((float) offset) / Track.Clip.length;
    }

    private void DrawRelativeLine(Rect r, float offset, Color color) {
      r = new Rect(r.x + r.width * offset, r.y, 1f, r.height);
      EditorGUI.DrawRect(r, color);
    }

    public Rect DrawBar() {
      Rect r = EditorGUILayout.GetControlRect();
      EditorGUI.DrawRect(r, MoreColors.SteelBlue);

      DrawRelativeLine(r, GetRelativeOffset(Track.LoopStart), MoreColors.Coral);
      DrawRelativeLine(r, GetRelativeOffset(Track.LoopEnd), MoreColors.OrangeRed);

      return r;
    }

    public void DrawBar(float offset) {
      var r = DrawBar();
      DrawRelativeLine(r, GetRelativeOffset(offset), MoreColors.Red);
    }

    public override void OnInspectorGUI() {
      DrawDefaultInspector();

      if (Track == null) {
        return;
      }

      DrawBar();
    }
  }
}
