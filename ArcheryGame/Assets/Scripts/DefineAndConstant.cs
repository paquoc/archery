using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefineAndConstant : MonoBehaviour
{
    public class ColorDefine
    {
        public static Color Red = Color.red;
        public static Color Orange = new Color(242, 105, 13);
        public static Color Pink = new Color(255, 41, 126);
        public static Color Purple = new Color(170, 0, 211);
        public static Color Green = Color.green;
        public static Color Yellow = Color.yellow;
        public static Color White = Color.white;

        public static List<Color> ListAvailableColor = new List<Color>() {
            ColorDefine.Red,
            ColorDefine.Orange,
            ColorDefine.Pink,
            ColorDefine.Purple,
            ColorDefine.Green,
            ColorDefine.Yellow,
            ColorDefine.White,
        };

        void Awake()
        {
          
        }

        public static string GetColorName(Color color)
        {
            if (color == ColorDefine.Red)
                return "Red";
            if (color == ColorDefine.Green)
                return "Green";
            if (color == ColorDefine.Orange)
                return "Orange";
            if (color == ColorDefine.Yellow)
                return "Yellow";
            if (color == ColorDefine.Purple)
                return "Purple";
            if (color == ColorDefine.Pink)
                return "Pink";
            if (color == ColorDefine.White)
                return "White";
            return "";
        }

        public static Color GetRandomColor()
        {
            int randIndex = Random.Range(0, ListAvailableColor.Count);
            return ListAvailableColor[randIndex];
        }
    }
}
