using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class CalculatePlanetOrbit : MonoBehaviour
{
    public int steps;

    public bool RenderPath;

    public GameObject Center;

    public int Skipper = 1;

    void Awake()
    {

    }

    void Update()
    {
        if (RenderPath)
        {
            int CenterIndex = 0;
            if (Center != null)
            {
                CenterIndex = Planet.Planets.FindIndex(center => center == Center.GetComponent<Planet>());
            }

            List<Vector3> planetPosList = new List<Vector3>();
            List<Vector3> planetVelList = new List<Vector3>();

            List<Planet> Planets = Planet.Planets;

            foreach (var planet in Planets)
            {
                planetPosList.Add(planet.gameObject.transform.position);
                planetVelList.Add(planet.Velocity);
            }

            Vector3 StartCenter;
            if (Center == null)
            {
                StartCenter = new Vector3(0, 0, 0);
            }
            else
            {
                StartCenter = planetPosList[CenterIndex];
            }

            Vector3[] BeforePos = new Vector3[Planets.Count];
            Vector3[] BeforePosCenter = new Vector3[Planets.Count];

            for (int j = 0; j < steps; j++)
            {
                for (int i = 0; i < Planets.Count; i++)
                {
                    Vector3 NextVelCenter = Planets[CenterIndex].GetNextStep(planetVelList[CenterIndex], planetPosList[CenterIndex], planetPosList, Planets);
                    Vector3 NextCenter = Planets[CenterIndex].GetNextPos(planetPosList[CenterIndex], NextVelCenter);
                    Vector3 NextVel = Planets[i].GetNextStep(planetVelList[i], planetPosList[i], planetPosList, Planets);
                    Vector3 Next = Planets[i].GetNextPos(planetPosList[i], NextVel);

                    if (j == 0)
                    {
                        BeforePos[i] = planetPosList[i];
                        BeforePosCenter[i] = planetVelList[CenterIndex];
                    }

                    if (Skipper == 0)
                    {
                        Skipper = 1;
                    }

                    if (j % Skipper == 0)
                    {
                        if (Center != null)
                        {
                            Debug.DrawLine(BeforePos[i] - (BeforePosCenter[i] - StartCenter), Next - (NextCenter - StartCenter), Planet.Planets[i].Color);
                        }
                        else
                        {
                            Debug.DrawLine(BeforePos[i], Next, Planet.Planets[i].Color);
                        }

                        BeforePos[i] = Next;
                        BeforePosCenter[i] = NextCenter;
                    }

                    planetPosList[i] = Next;
                    planetVelList[i] = NextVel;
                }
            }
        }
    }
}
