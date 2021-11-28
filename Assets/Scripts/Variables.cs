using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Variables : MonoBehaviour
{
    public float G;
    public Material StandardMaterial;
    public GameObject PlanetPrefab;
    public bool FusePlanets;

    void Update()
    {
        Constants.G = G;
        Constants.DefaultPlanetMaterial = StandardMaterial;
        Constants.PlanetPrefab = PlanetPrefab;
        Constants.FusePlanets = FusePlanets;
    }
}
