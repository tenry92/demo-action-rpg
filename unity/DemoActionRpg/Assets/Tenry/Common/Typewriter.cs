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

    private bool TextAvailable => currentTextIndex < textToType.Length;

    private float speedMultiplier = 1f;

    private void Awake() {
      text = GetComponent<TMP_Text>();
    }

    private void Start() {
      textToType = text.text;
      Restart();
    }

    private void Update() {
      if (speedMultiplier <= 0f) {
        return;
      }

      fraction += charsPerSecond * Time.deltaTime * speedMultiplier;

      while (fraction >= 1f) {
        fraction -= 1f;
        TypeNextChar();
      }
    }

    private void TypeNextChar() {
      if (TextAvailable) {
        if (GetNextCharPage() > currentPageIndex) {
          enabled = false;
          PageFinished?.Invoke();
          return;
        }

        text.text += RetriveNextChars();
        text.pageToDisplay = currentPageIndex + 1;
      } else {
        enabled = false;
        TextFinished?.Invoke();
      }
    }

    private int GetNextCharPage() {
      var charInfo = textInfo.characterInfo[currentCharIndex];

      return charInfo.pageNumber;
    }

    private string RetriveNextChars() {
      var sb = new StringBuilder();

      var charInfo = textInfo.characterInfo[currentCharIndex];

      if (charInfo.pageNumber > currentPageIndex) {
        currentPageIndex = ++text.pageToDisplay;
      }

      if (charInfo.lineNumber > currentLineIndex) {
        sb.Append("\n");
        ++currentLineIndex;
      }

      var substringLength = 1 + charInfo.index - currentTextIndex;
      var substring = textToType.Substring(currentTextIndex, substringLength);

      if (substring != "\n") {
        sb.Append(substring);
      }

      ++currentCharIndex;
      currentTextIndex = charInfo.index + 1;

      return sb.ToString();
    }

    public void Restart() {
      textInfo = text.GetTextInfo(textToType);
      text.text = "";

      currentCharIndex = 0;
      currentTextIndex = 0;
      currentLineIndex = 0;
      currentPageIndex = 0;
      fraction = 0f;
    }

    public void Continue() {
      enabled = true;
      fraction = 0f;

      if (GetNextCharPage() > currentPageIndex) {
        currentPageIndex = GetNextCharPage();
      }
    }

    public void SetSpeedMultiplier(float multiplier) {
      speedMultiplier = Mathf.Clamp(multiplier, 0f, 100f);
    }
  }
}
