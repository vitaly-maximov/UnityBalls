using UnityEngine;

public static class Common
{
    public static Color GetColor(uint argb)
    {
        float a = ((argb & 0xff000000) >> 24) / 255f;
        float r = ((argb & 0x00ff0000) >> 16) / 255f;
        float g = ((argb & 0x0000ff00) >> 8) / 255f;
        float b = ((argb & 0x000000ff) >> 0) / 255f;

        return new Color(r, g, b, a);
    }
}