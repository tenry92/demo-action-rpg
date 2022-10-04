using UnityEngine;
using TMPro;

using Tenry.Common;

namespace Tenry.DemoActionRpg {
  public class MessageBox : MonoBehaviour {
    #region Serialized Fields
    [SerializeField]
    private TMP_Text text;
    #endregion

    public static void ShowMessage(string message) {
      var messageBox = GameObject.FindObjectOfType<MessageBox>(true);

      if (messageBox == null) {
        Debug.LogWarning("No object of MessageBox found");
        return;
      }

      messageBox.Show(message);
    }

    public void Show(string message) {
      this.text.text = message;

      var canvasGroup = this.GetComponent<CanvasGroup>();

      if (canvasGroup != null) {
        canvasGroup.alpha = 1f;
      }
      
      var typewriter = this.text.GetComponent<Typewriter>();

      if (typewriter != null) {
        typewriter.enabled = true;
      }

      this.gameObject.SetActive(true);
    }
  }
}
