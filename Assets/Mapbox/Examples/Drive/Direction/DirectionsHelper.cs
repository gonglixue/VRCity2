using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mapbox;
using Mapbox.MeshGeneration.Factories;
using Mapbox.Scripts.Utilities;

public class DirectionsHelper : MonoBehaviour
{
    public DirectionsFactory Directions;
    public List<Transform> Waypoints;

	void Start ()
	{
		// draw directions path at start
		Query ();
	}

    void Update()
    {
        Test();
    }

    public void Query()
    {
        var waypoints = new List<GeoCoordinate>();
        foreach (var wp in Waypoints)
        {
            waypoints.Add(wp.transform.GetDriveGeoPosition());
            Debug.Log("drive waypoints:" + wp.transform.GetDriveGeoPosition());
        }

        Directions.Query(waypoints);
    }

    void Test()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Debug.Log("space key down");
            Query();
        }
    }


}
