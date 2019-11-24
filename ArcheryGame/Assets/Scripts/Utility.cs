using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility : MonoBehaviour
{
    public static Utility Instance;
    public Font font;

    void Awake()
    {
        Instance = this;
    }

    public Font GetDefaultFont()
    {
        return font;
    }
}
