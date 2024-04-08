using UnityEngine;

namespace D2D.Utilities
{
    /// <summary>
    /// Log with nice colors.
    /// </summary>
    public static class DLog
    {
        private const string BlueColor = "1982c4";
        private const string GrayColor = "666666";
        private const string YellowColor = "ffca3a";
        private const string OrangeColor = "ffb300";
        private const string GreenColor = "8ac926";
        private const string Red = "E22121";

        public static void PrintRed(string message) => PrintColored(message, Red);

        public static void PrintGray(string message) => PrintColored(message, GrayColor);

        public static void PrintOrange(string message) => PrintColored(message, OrangeColor);

        public static void PrintBlue(string message) => PrintColored(message, BlueColor);

        public static void PrintColored(string message, string color)
        {
            if (color.IndexOf('#') == -1)
                color = '#' + color;
            
            Debug.Log($"<color={color}>{message}</color>");
        }
    }
}