using UnityEngine;

namespace Tenry.Utils {
  public static class MoreColors {
    #region Basic Grayscale
    public static Color White => new Color(1f, 1f, 1f);
    public static Color Silver => new Color(0.75f, 0.75f, 0.75f);
    public static Color Gray => new Color(0.5f, 0.5f, 0.5f);
    public static Color Black => new Color(0f, 0f, 0f);
    #endregion

    #region Basic Bright
    public static Color Red => new Color(1f, 0f, 0f);
    public static Color Yellow => new Color(1f, 1f, 0f);
    public static Color Lime => new Color(0f, 1f, 0f);
    public static Color Aqua => new Color(0f, 1f, 1f);
    public static Color Blue => new Color(0f, 0f, 1f);
    public static Color Fuchsia => new Color(1f, 0f, 1f);
    #endregion

    #region Basic Dark
    public static Color Maroon => Red * 0.5f;
    public static Color Olive => Yellow * 0.5f;
    public static Color Green => Lime * 0.5f;
    public static Color Teal => Aqua * 0.5f;
    public static Color Navy => Blue * 0.5f;
    public static Color Purple => Fuchsia * 0.5f;
    #endregion

    #region Extended Pink
    public static Color MediumVioletRed = new Color(0.78f, 0.08f, 0.52f);
    public static Color DeepPink = new Color(1.00f, 0.08f, 0.58f);
    public static Color PaleVioletRed = new Color(0.86f, 0.44f, 0.58f);
    public static Color HotPink = new Color(1.00f, 0.41f, 0.71f);
    public static Color LightPink = new Color(1.00f, 0.71f, 0.76f);
    public static Color Pink = new Color(1.00f, 0.75f, 0.80f);
    #endregion

    #region Extended Red
    public static Color DarkRed = new Color(0.55f, 0.00f, 0.00f);
    // public static Color Red = new Color(1.00f, 0.00f, 0.00f);
    public static Color Firebrick = new Color(0.70f, 0.13f, 0.13f);
    public static Color Crimson = new Color(0.86f, 0.08f, 0.24f);
    public static Color IndianRed = new Color(0.80f, 0.36f, 0.36f);
    public static Color LightCoral = new Color(0.94f, 0.50f, 0.50f);
    public static Color Salmon = new Color(0.98f, 0.50f, 0.45f);
    public static Color DarkSalmon = new Color(0.91f, 0.59f, 0.48f);
    #endregion

    #region Extended Orange
    public static Color LightSalmon = new Color(1.00f, 0.63f, 0.48f);
    public static Color OrangeRed = new Color(1.00f, 0.27f, 0.00f);
    public static Color Tomato = new Color(1.00f, 0.39f, 0.28f);
    public static Color DarkOrange = new Color(1.00f, 0.55f, 0.00f);
    public static Color Coral = new Color(1.00f, 0.50f, 0.31f);
    public static Color Orange = new Color(1.00f, 0.65f, 0.00f);
    #endregion

    #region Extended Yellow
    public static Color DarkKhaki = new Color(0.74f, 0.72f, 0.42f);
    public static Color Gold = new Color(1.00f, 0.84f, 0.00f);
    public static Color Khaki = new Color(0.94f, 0.90f, 0.55f);
    public static Color PeachPuff = new Color(1.00f, 0.85f, 0.73f);
    // public static Color Yellow = new Color(1.00f, 1.00f, 0.00f);
    public static Color PaleGoldenrod = new Color(0.93f, 0.91f, 0.67f);
    public static Color Moccasin = new Color(1.00f, 0.89f, 0.71f);
    public static Color PapayaWhip = new Color(1.00f, 0.94f, 0.84f);
    public static Color LightGoldenrodYellow = new Color(0.98f, 0.98f, 0.82f);
    public static Color LemonChiffon = new Color(1.00f, 0.98f, 0.80f);
    public static Color LightYellow = new Color(1.00f, 1.00f, 0.88f);
    #endregion

    #region Extended Brown
    // public static Color Maroon = new Color(0.50f, 0.00f, 0.00f);
    public static Color Brown = new Color(0.65f, 0.16f, 0.16f);
    public static Color SaddleBrown = new Color(0.55f, 0.27f, 0.07f);
    public static Color Sienna = new Color(0.63f, 0.32f, 0.18f);
    public static Color Chocolate = new Color(0.82f, 0.41f, 0.12f);
    public static Color DarkGoldenrod = new Color(0.72f, 0.53f, 0.04f);
    public static Color Peru = new Color(0.80f, 0.52f, 0.25f);
    public static Color RosyBrown = new Color(0.74f, 0.56f, 0.56f);
    public static Color Goldenrod = new Color(0.85f, 0.65f, 0.13f);
    public static Color SandyBrown = new Color(0.96f, 0.64f, 0.38f);
    public static Color Tan = new Color(0.82f, 0.71f, 0.55f);
    public static Color Burlywood = new Color(0.87f, 0.72f, 0.53f);
    public static Color Wheat = new Color(0.96f, 0.87f, 0.70f);
    public static Color NavajoWhite = new Color(1.00f, 0.87f, 0.68f);
    public static Color Bisque = new Color(1.00f, 0.89f, 0.77f);
    public static Color BlanchedAlmond = new Color(1.00f, 0.92f, 0.80f);
    public static Color Cornsilk = new Color(1.00f, 0.97f, 0.86f);
    #endregion

    #region Extended Purple, Violet, Magenta
    public static Color Indigo = new Color(0.29f, 0.00f, 0.51f);
    // public static Color Purple = new Color(0.50f, 0.00f, 0.50f);
    public static Color DarkMagenta = new Color(0.55f, 0.00f, 0.55f);
    public static Color DarkViolet = new Color(0.58f, 0.00f, 0.83f);
    public static Color DarkSlateBlue = new Color(0.28f, 0.24f, 0.55f);
    public static Color BlueViolet = new Color(0.54f, 0.17f, 0.89f);
    public static Color DarkOrchid = new Color(0.60f, 0.20f, 0.80f);
    // public static Color Fuchsia = new Color(1.00f, 0.00f, 1.00f);
    public static Color Magenta = new Color(1.00f, 0.00f, 1.00f);
    public static Color SlateBlue = new Color(0.42f, 0.35f, 0.80f);
    public static Color MediumSlateBlue = new Color(0.48f, 0.41f, 0.93f);
    public static Color MediumOrchid = new Color(0.73f, 0.33f, 0.83f);
    public static Color MediumPurple = new Color(0.58f, 0.44f, 0.86f);
    public static Color Orchid = new Color(0.85f, 0.44f, 0.84f);
    public static Color Violet = new Color(0.93f, 0.51f, 0.93f);
    public static Color Plum = new Color(0.87f, 0.63f, 0.87f);
    public static Color Thistle = new Color(0.85f, 0.75f, 0.85f);
    public static Color Lavender = new Color(0.90f, 0.90f, 0.98f);
    #endregion

    #region Extended Blue
    public static Color MidnightBlue = new Color(0.10f, 0.10f, 0.44f);
    // public static Color Navy = new Color(0.00f, 0.00f, 0.50f);
    public static Color DarkBlue = new Color(0.00f, 0.00f, 0.55f);
    public static Color MediumBlue = new Color(0.00f, 0.00f, 0.80f);
    // public static Color Blue = new Color(0.00f, 0.00f, 1.00f);
    public static Color RoyalBlue = new Color(0.25f, 0.41f, 0.88f);
    public static Color SteelBlue = new Color(0.27f, 0.51f, 0.71f);
    public static Color DodgerBlue = new Color(0.12f, 0.56f, 1.00f);
    public static Color DeepSkyBlue = new Color(0.00f, 0.75f, 1.00f);
    public static Color CornflowerBlue = new Color(0.39f, 0.58f, 0.93f);
    public static Color SkyBlue = new Color(0.53f, 0.81f, 0.92f);
    public static Color LightSkyBlue = new Color(0.53f, 0.81f, 0.98f);
    public static Color LightSteelBlue = new Color(0.69f, 0.77f, 0.87f);
    public static Color LightBlue = new Color(0.68f, 0.85f, 0.90f);
    public static Color PowderBlue = new Color(0.69f, 0.88f, 0.90f);
    #endregion

    #region Extended Cyan
    // public static Color Teal = new Color(0.00f, 0.50f, 0.50f);
    public static Color DarkCyan = new Color(0.00f, 0.55f, 0.55f);
    public static Color LightSeaGreen = new Color(0.13f, 0.70f, 0.67f);
    public static Color CadetBlue = new Color(0.37f, 0.62f, 0.63f);
    public static Color DarkTurquoise = new Color(0.00f, 0.81f, 0.82f);
    public static Color MediumTurquoise = new Color(0.28f, 0.82f, 0.80f);
    public static Color Turquoise = new Color(0.25f, 0.88f, 0.82f);
    // public static Color Aqua = new Color(0.00f, 1.00f, 1.00f);
    public static Color Cyan = new Color(0.00f, 1.00f, 1.00f);
    public static Color Aquamarine = new Color(0.50f, 1.00f, 0.83f);
    public static Color PaleTurquoise = new Color(0.69f, 0.93f, 0.93f);
    public static Color LightCyan = new Color(0.88f, 1.00f, 1.00f);
    #endregion

    #region Extended Green
    public static Color DarkGreen = new Color(0.00f, 0.39f, 0.00f);
    // public static Color Green = new Color(0.00f, 0.50f, 0.00f);
    public static Color DarkOliveGreen = new Color(0.33f, 0.42f, 0.18f);
    public static Color ForestGreen = new Color(0.13f, 0.55f, 0.13f);
    public static Color SeaGreen = new Color(0.18f, 0.55f, 0.34f);
    // public static Color Olive = new Color(0.50f, 0.50f, 0.00f);
    public static Color OliveDrab = new Color(0.42f, 0.56f, 0.14f);
    public static Color MediumSeaGreen = new Color(0.24f, 0.70f, 0.44f);
    public static Color LimeGreen = new Color(0.20f, 0.80f, 0.20f);
    // public static Color Lime = new Color(0.00f, 1.00f, 0.00f);
    public static Color SpringGreen = new Color(0.00f, 1.00f, 0.50f);
    public static Color MediumSpringGreen = new Color(0.00f, 0.98f, 0.60f);
    public static Color DarkSeaGreen = new Color(0.56f, 0.74f, 0.56f);
    public static Color MediumAquamarine = new Color(0.40f, 0.80f, 0.67f);
    public static Color YellowGreen = new Color(0.60f, 0.80f, 0.20f);
    public static Color LawnGreen = new Color(0.49f, 0.99f, 0.00f);
    public static Color Chartreuse = new Color(0.50f, 1.00f, 0.00f);
    public static Color LightGreen = new Color(0.56f, 0.93f, 0.56f);
    public static Color GreenYellow = new Color(0.68f, 1.00f, 0.18f);
    public static Color PaleGreen = new Color(0.60f, 0.98f, 0.60f);
    #endregion

    #region Extended White
    public static Color MistyRose = new Color(1.00f, 0.89f, 0.88f);
    public static Color AntiqueWhite = new Color(0.98f, 0.92f, 0.84f);
    public static Color Linen = new Color(0.98f, 0.94f, 0.90f);
    public static Color Beige = new Color(0.96f, 0.96f, 0.86f);
    public static Color WhiteSmoke = new Color(0.96f, 0.96f, 0.96f);
    public static Color LavenderBlush = new Color(1.00f, 0.94f, 0.96f);
    public static Color OldLace = new Color(0.99f, 0.96f, 0.90f);
    public static Color AliceBlue = new Color(0.94f, 0.97f, 1.00f);
    public static Color Seashell = new Color(1.00f, 0.96f, 0.93f);
    public static Color GhostWhite = new Color(0.97f, 0.97f, 1.00f);
    public static Color Honeydew = new Color(0.94f, 1.00f, 0.94f);
    public static Color FloralWhite = new Color(1.00f, 0.98f, 0.94f);
    public static Color Azure = new Color(0.94f, 1.00f, 1.00f);
    public static Color MintCream = new Color(0.96f, 1.00f, 0.98f);
    public static Color Snow = new Color(1.00f, 0.98f, 0.98f);
    public static Color Ivory = new Color(1.00f, 1.00f, 0.94f);
    // public static Color White = new Color(1.00f, 1.00f, 1.00f);
    #endregion

    #region Extended Gray and Black
    // public static Color Black = new Color(0.00f, 0.00f, 0.00f);
    public static Color DarkSlateGray = new Color(0.18f, 0.31f, 0.31f);
    public static Color DimGray = new Color(0.41f, 0.41f, 0.41f);
    public static Color SlateGray = new Color(0.44f, 0.50f, 0.56f);
    // public static Color Gray = new Color(0.50f, 0.50f, 0.50f);
    public static Color LightSlateGray = new Color(0.47f, 0.53f, 0.60f);
    public static Color DarkGray = new Color(0.66f, 0.66f, 0.66f);
    // public static Color Silver = new Color(0.75f, 0.75f, 0.75f);
    public static Color LightGray = new Color(0.83f, 0.83f, 0.83f);
    public static Color Gainsboro = new Color(0.86f, 0.86f, 0.86f);
    #endregion
  }
}
