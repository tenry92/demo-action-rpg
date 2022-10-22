using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Tenry.DemoActionRpg {
  public class CheatInput : MonoBehaviour {
    public enum Direction {
      Up,
      Down,
      Left,
      Right,
    }

    #region Serialized Fields
    [SerializeField]
    private InputActionMap actionMap;

    [SerializeField]
    private Direction[] code;
    
    [SerializeField]
    private UnityEvent activated;
    #endregion

    private InputAction up;
    
    private InputAction down;

    private InputAction left;

    private InputAction right;

    private int currentIndex = 0;

    private void Awake() {
      up = actionMap.FindAction("Up");
      down = actionMap.FindAction("Down");
      left = actionMap.FindAction("Left");
      right = actionMap.FindAction("Right");
    }

    private void Start() {
      up.performed += _ => Press(Direction.Up);
      down.performed += _ => Press(Direction.Down);
      left.performed += _ => Press(Direction.Left);
      right.performed += _ => Press(Direction.Right);
    }

    private void OnEnable() {
      currentIndex = 0;

      up.Enable();
      down.Enable();
      left.Enable();
      right.Enable();
    }

    private void OnDisable() {
      up.Disable();
      down.Disable();
      left.Disable();
      right.Disable();
    }

    private void Press(Direction direction) {
      if (currentIndex < code.Length) {
        var expected = code[currentIndex];

        if (direction == expected) {
          ++currentIndex;

          if (currentIndex == code.Length) {
            this.activated.Invoke();
          }
        } else {
          // reset
          currentIndex = 0;
        }
      }
    }
  }
}
