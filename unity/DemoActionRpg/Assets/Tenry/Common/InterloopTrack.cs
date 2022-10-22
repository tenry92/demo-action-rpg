using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

using Tenry.Utils.Editor;
#endif

namespace Tenry.Common {
  [CreateAssetMenu(menuName = "Tenry/Interloop Track", fileName = "Track")]
  public class InterloopTrack : ScriptableObject {
    [SerializeField]
    private AudioClip clip;

    [SerializeField]
    [Tooltip("Loop start point in seconds.")]
    private double loopStart = 0;

    [SerializeField]
    [Tooltip("Loop end point in seconds.")]
    private double loopEnd = 0;

    public AudioClip Clip => clip;

    public double LoopStart => loopStart;

    public double LoopEnd => loopEnd;

    public double IntroLength => loopStart;

    public double LoopLength => loopEnd - loopStart;

#if UNITY_EDITOR
    private void OnValidate() {
      if (loopStart != 0 || loopEnd != 0) {
        return;
      }

      SetLoopPointsFromClip();
    }

    private void SetLoopPointsFromClip() {
      if (clip == null) {
        return;
      }

      var assetPath = AssetDatabase.GetAssetPath(clip.GetInstanceID());

      if (!OggInfo.TryParseInfoFromFile(PathUtility.ToAbsolutePath(assetPath), out var info)) {
        return;
      }

      var frequency = info.AudioSampleRate;
      var metaLoopStart = -1;
      var metaLoopLength = -1;

      if (info.TryGetMetadata("LOOPSTART", out var startString)) {
        metaLoopStart = System.Int32.Parse(startString);
      }

      if (info.TryGetMetadata("LOOPLENGTH", out var lengthString)) {
        metaLoopLength = System.Int32.Parse(lengthString);
      }

      if (metaLoopStart >= 0 && metaLoopLength > 0) {
        loopStart = ((float) metaLoopStart) / frequency;
        loopEnd = ((float) metaLoopStart + metaLoopLength) / frequency;
      }
    }
#endif
  }
}
