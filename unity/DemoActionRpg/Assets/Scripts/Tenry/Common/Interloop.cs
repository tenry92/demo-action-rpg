using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

using Tenry.Utils.Editor;
#endif

namespace Tenry.Common {
  public class Interloop : MonoBehaviour {
    #region Serialized Fields
    [SerializeField]
    private AudioClip clip;

    [SerializeField]
    [Tooltip("Loop start point in seconds.")]
    private double loopStart = 0;

    [SerializeField]
    [Tooltip("Loop end point in seconds.")]
    private double loopEnd = 0;
    #endregion

    // the source that is currently playing
    private AudioSource frontSource;

    // the source that will play next
    private AudioSource backSource;

    private double IntroLength => loopStart;

    private double LoopLength => loopEnd - loopStart;

    // DSP time when to schedule the next section
    private double nextSchedule;

    public double OffsetInSeconds { get; private set; }

    public bool IsPlaying { get; private set; }

    public AudioClip Clip => clip;

    public double LoopStart => loopStart;

    public double LoopEnd => loopEnd;

    private void SwapSources() {
      (frontSource, backSource) = (backSource, frontSource);
    }

    private void Awake() {
      frontSource = gameObject.AddComponent<AudioSource>();
      backSource = gameObject.AddComponent<AudioSource>();
      OffsetInSeconds = 0;
      IsPlaying = false;
    }

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

    private void Update() {
      if (IsPlaying) {
        if (backSource.isPlaying && !frontSource.isPlaying) {
          SwapSources();
          ScheduleLoop();
        }

        UpdateOffsetInSeconds();
      }
    }

    private void ScheduleSection(double start, double end) {
      backSource.timeSamples = (int) (((double) backSource.clip.frequency) * start);
      backSource.PlayScheduled(nextSchedule);
      nextSchedule += end - start;
      backSource.SetScheduledEndTime(nextSchedule);
    }

    private void ScheduleLoop() {
      ScheduleSection(loopStart, loopEnd);
    }

    private void OnDisable() {
      Stop();
    }

    private void UpdateOffsetInSeconds() {
      OffsetInSeconds = ((double) frontSource.timeSamples) / frontSource.clip.frequency;
    }

    public void Play() {
      nextSchedule = AudioSettings.dspTime + 0.1;

      frontSource.clip = clip;
      backSource.clip = clip;

      if (OffsetInSeconds < loopStart) {
        ScheduleSection(OffsetInSeconds, loopStart);
      } else {
        ScheduleSection(OffsetInSeconds, loopEnd);
      }

      SwapSources();
      ScheduleLoop();

      IsPlaying = true;
    }

    public void Pause() {
      IsPlaying = false;

      UpdateOffsetInSeconds();

      frontSource.Stop();
      backSource.Stop();
    }

    public void Stop() {
      OffsetInSeconds = 0;
      IsPlaying = false;

      frontSource.Stop();
      backSource.Stop();
    }
  }
}
