using System.Collections;

using UnityEngine;

namespace Tenry.DemoActionRpg {
  public class Interactable : MonoBehaviour {
    public void Interact() {
      MessageBox.ShowMessage("Hi there! How are you doing?");
    }
  }
}