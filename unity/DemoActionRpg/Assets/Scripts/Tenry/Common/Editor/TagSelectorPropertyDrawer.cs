using UnityEditor;
using UnityEngine;

namespace Tenry.Common.Editor {
  [CustomPropertyDrawer(typeof(TagSelectorAttribute))]
  public class TagSelectorPropertyDrawer : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
      if (property.propertyType == SerializedPropertyType.String) {
        EditorGUI.BeginProperty(position, label, property);
        
        property.stringValue = EditorGUI.TagField(position, label, property.stringValue);
      } else {
        EditorGUI.PropertyField(position, property, label);
      }
    }
  }
}
