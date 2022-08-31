using System;

namespace Tenry.DemoActionRpg {
  [Flags]
  public enum DamageableTypes {
    None = 0x00,
    Player = 0x01,
    Enemy = 0x02,
    Object = 0x04,
  }
}
