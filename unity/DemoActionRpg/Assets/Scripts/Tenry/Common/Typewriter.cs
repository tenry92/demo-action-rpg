using System.Text;

using UnityEngine;
using UnityEngine.Events;
using TMPro;

namespace Tenry.Common {
  [RequireComponent(typeof(RectTransform))]
  [RequireComponent(typeof(TMP_Text))]
  public class Typewriter : MonoBehaviour {
    #region Serialized Fields
    [SerializeField]
    private float charsPerSecond = 20f;

    [SerializeField]
    private UnityEvent PageFinished;

    [SerializeField]
    private UnityEvent TextFinished;
    #endregion

    private TMP_Text text;

    private TMP_TextInfo textInfo;

    private string textToType;

    // visual char index (excluding rich tags)
    private int currentCharIndex = 0;

    // raw string index (including rich tags)
    private int currentTextIndex = 0;

    private int currentLineIndex = 0;

    private int currentPageIndex = 0;

    private float fraction = 0f;

    private bool TextAvailable => this.currentTextIndex < this.textToType.Length;

    private float speedMultiplier = 1f;

    private void Awake() {
      this.text = this.GetComponent<TMP_Text>();
    }

    private void Start() {
      this.textToType = this.text.text;
      this.Restart();
    }

    private void Update() {
      if (this.speedMultiplier <= 0f) {
        return;
      }

      this.fraction += this.charsPerSecond * Time.deltaTime * this.speedMultiplier;

      while (this.fraction >= 1f) {
        this.fraction -= 1f;
        this.TypeNextChar();
      }
    }

    private void TypeNextChar() {
      if (this.TextAvailable) {
        if (this.GetNextCharPage() > this.currentPageIndex) {
          this.enabled = false;
          this.PageFinished?.Invoke();
          return;
        }

        this.text.text += this.RetriveNextChars();
        this.text.pageToDisplay = this.currentPageIndex + 1;
      } else {
        this.enabled = false;
        this.TextFinished?.Invoke();
      }
    }

    private int GetNextCharPage() {
      var charInfo = this.textInfo.characterInfo[this.currentCharIndex];

      return charInfo.pageNumber;
    }

    private string RetriveNextChars() {
      var sb = new StringBuilder();

      var charInfo = this.textInfo.characterInfo[this.currentCharIndex];

      if (charInfo.pageNumber > this.currentPageIndex) {
        this.currentPageIndex = ++this.text.pageToDisplay;
      }

      if (charInfo.lineNumber > this.currentLineIndex) {
        sb.Append("\n");
        ++this.currentLineIndex;
      }

      var substringLength = 1 + charInfo.index - this.currentTextIndex;
      var substring = this.textToType.Substring(this.currentTextIndex, substringLength);

      if (substring != "\n") {
        sb.Append(substring);
      }

      ++this.currentCharIndex;
      this.currentTextIndex = charInfo.index + 1;

      return sb.ToString();
    }

    public void Restart() {
      this.textInfo = this.text.GetTextInfo(this.textToType);
      this.text.text = "";

      this.currentCharIndex = 0;
      this.currentTextIndex = 0;
      this.currentLineIndex = 0;
      this.currentPageIndex = 0;
      this.fraction = 0f;
    }

    public void Continue() {
      this.enabled = true;
      this.fraction = 0f;

      if (this.GetNextCharPage() > this.currentPageIndex) {
        this.currentPageIndex = this.GetNextCharPage();
      }
    }

    public void SetSpeedMultiplier(float multiplier) {
      this.speedMultiplier = Mathf.Clamp(multiplier, 0f, 100f);
    }
  }
}
