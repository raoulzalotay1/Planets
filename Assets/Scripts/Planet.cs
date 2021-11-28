using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Planet : MonoBehaviour
{
    public static List<Planet> Planets = new List<Planet>();
    private static float timeStep = 1;

    public int Mass;

    public float Radius;

    public Vector3 Velocity;

    public Color Color;

    private bool setMat;

    public float Volume()
    {
        return 4 / 3 * Mathf.PI * Mathf.Pow(Radius, 3);
    }

    void Awake()
    {
        setMat = false;
    }

    void Update()
    {
        if (Application.isPlaying && Constants.FusePlanets)
        {
            foreach (var planet in Planets)
            {
                if (planet != this)
                {
                    if (hasCollided(this, planet))
                    {
                        GameObject planetGO = Instantiate(Constants.PlanetPrefab);
                        planetGO.transform.position = Constants.MixVector3(gameObject.transform.position, planet.gameObject.transform.position);
                        Planet newPlanet = planetGO.GetComponent<Planet>();
                        newPlanet.Radius = Constants.GetNewRadius(Radius, planet.Radius);
                        newPlanet.Mass = Mass + planet.Mass;
                        newPlanet.Velocity = Constants.MixVector3(Velocity, planet.Velocity);
                        newPlanet.Color = Constants.MixColor(Color, planet.Color);
                        Destroy(gameObject);
                        Destroy(planet.gameObject);
                    }
                }
            }
        }

        if (Radius != 0)
        {
            gameObject.transform.localScale = new Vector3(Radius * 2, Radius * 2, Radius * 2);
        }

        if (Constants.DefaultPlanetMaterial != null && !setMat)
        {
            Material mat = new Material(Constants.DefaultPlanetMaterial);
            mat.color = Color;
            gameObject.GetComponent<MeshRenderer>().material = mat;
            setMat = true;
        }

        if (!Planets.Contains(this))
        {
            Planets.Add(this);
        }

        if (Application.isPlaying)
        {
            this.Velocity = GetNextStep(this.Velocity, this.gameObject.transform.position);
            
            this.gameObject.transform.position = GetNextPos(this.gameObject.transform.position, this.Velocity);
        }
    }

    private void collide(GameObject collider)
    {
        if (collider.tag == "Planet")
        {
            Planet colPlanet = collider.GetComponent<Planet>();

            GameObject planetGO = Instantiate(Constants.PlanetPrefab);
            Planet planet = planetGO.GetComponent<Planet>();
            planet.Mass = Mass + colPlanet.Mass;
            planet.Velocity = Constants.MixVector3(Velocity, colPlanet.Velocity);
            planet.Color = Constants.MixColor(Color, colPlanet.Color);
        }
    }

    void OnDestroy()
    {
        Planets.Remove(this);
    }

    private static bool hasCollided(Planet planet1, Planet planet2)
    {
        return planet1.Radius + planet2.Radius >= Vector3.Distance(planet1.gameObject.transform.position, planet2.gameObject.transform.position);
    }

    public Vector3 GetNextPos(Vector3 currentPos, Vector3 velocity)
    {
        return currentPos + velocity * timeStep;
    }

    public Vector3 GetNextStep(Vector3 velocity, Vector3 pos)
    {
        foreach (var planet2 in Planets)
        {
            if (planet2 != this)
            {
                float G = Constants.G;
                float mass1 = this.Mass;
                float mass2 = planet2.Mass;
                float distance = (planet2.gameObject.transform.position - pos).sqrMagnitude;

                Vector3 forceDirection = (planet2.gameObject.transform.position - pos).normalized;

                Vector3 force = forceDirection * G * mass1 * mass2 / distance;
                Vector3 acceleration = force / mass1;
                velocity += acceleration * timeStep;
            }
        }

        return velocity;
    }

    public Vector3 GetNextStep(Vector3 velocity, Vector3 pos, List<Vector3> planetPositions, List<Planet> Planets)
    {
        for (int i = 0; i < Planets.Count; i++)
        {
            if (Planets[i] != this)
            {
                float G = Constants.G;
                float mass1 = this.Mass;
                float mass2 = Planets[i].Mass;
                float distance = (planetPositions[i] - pos).sqrMagnitude;

                Vector3 forceDirection = (planetPositions[i] - pos).normalized;

                Vector3 force = forceDirection * G * mass1 * mass2 / distance;
                Vector3 acceleration = force / mass1;
                velocity += acceleration * timeStep;
            }
        }

        return velocity;
    }
}
