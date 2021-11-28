using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants
{
    public static float G;
    public static Material DefaultPlanetMaterial;
    public static GameObject PlanetPrefab;
    public static bool FusePlanets;

    public static Color MixColor(Color color1, Color color2)
    {
        Color result = new Color();
        result.r = (color1.r + color2.r) / 2;
        result.g = (color1.g + color2.g) / 2;
        result.b = (color1.b + color2.b) / 2;
        return result;
    }

    public static Vector3 MixVector3(Vector3 vec1, Vector3 vec2)
    {
        return new Vector3((vec1.x + vec2.x) / 2, (vec1.y + vec2.y) / 2,(vec1.z + vec2.z) / 2);
    }

    public static float GetNewRadius(float radius1, float radius2)
    {
        float noname = 4 / 3 * Mathf.PI;

        float volume1 = noname * Mathf.Pow(radius1, 3);
        float volume2 = noname * Mathf.Pow(radius2, 3);

        float newVolume = volume1 + volume2;

        float result = Mathf.Pow(newVolume / noname, 1f / 3f);
        return result;
    }
}
