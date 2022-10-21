using UnityEngine;
using UnityEngine.Audio;

namespace Tenry.Common {
  public class Interloop : MonoBehaviour {
    #region Serialized Fields
    [SerializeField]
    private InterloopTrack track;

    [SerializeField]
    private AudioMixerGroup output;
    #endregion

    // the source that is currently playing
    private AudioSource frontSource;

    // the source that will play next
    private AudioSource backSource;

    // DSP time when to schedule the next section
    private double nextSchedule;

    public double OffsetInSeconds { get; private set; }

    public bool IsPlaying { get; private set; }

    public InterloopTrack Track => track;

    private void SwapSources() {
      (frontSource, backSource) = (backSource, frontSource);
    }

    private void Awake() {
      frontSource = gameObject.AddComponent<AudioSource>();
      backSource = gameObject.AddComponent<AudioSource>();
      OffsetInSeconds = 0;
      IsPlaying = false;

      frontSource.outputAudioMixerGroup = output;
      backSource.outputAudioMixerGroup = output;
    }

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
      ScheduleSection(track.LoopStart, track.LoopEnd);
    }

    private void OnDisable() {
      Stop();
    }

    private void UpdateOffsetInSeconds() {
      OffsetInSeconds = ((double) frontSource.timeSamples) / frontSource.clip.frequency;
    }

    public void Play() {
      nextSchedule = AudioSettings.dspTime + 0.1;

      frontSource.clip = track.Clip;
      backSource.clip = track.Clip;

      if (OffsetInSeconds < track.LoopStart) {
        ScheduleSection(OffsetInSeconds, track.LoopStart);
      } else {
        ScheduleSection(OffsetInSeconds, track.LoopEnd);
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
