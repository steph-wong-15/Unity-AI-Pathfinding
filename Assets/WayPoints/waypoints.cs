using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Code referenced from Unity Waypoints Tutorial

public class waypoints : MonoBehaviour
{

   public  GameObject[] waypoint;
   public GameObject tracker;
   int currentWaypoint = 0;

    //speeds, rotation, distances
   public float agentSpeed = 5f;
   public float trackerSpeed = 10f;
   public float rotationSpeed = 5f;
   public float minDistance = 2f;
   public float maxDistanceAhead = 10f;


    // Start is called before the first frame update
    void Start()
    {

        tracker = GameObject.CreatePrimitive(PrimitiveType.Cube);
        tracker.transform.position = new Vector3(0, 5f, 0);

        DestroyImmediate(tracker.GetComponent<Collider>());

        tracker.transform.rotation = this.transform.rotation;
        tracker.transform.position = this.transform.position;
        
    }

    //tracker follows the waypoints and agent follows tracker
    void Tracker()
    {

        if (Vector3.Distance(this.transform.position, tracker.transform.position) > maxDistanceAhead)
            return;

        if (Vector3.Distance(tracker.transform.position, waypoint[currentWaypoint].transform.position) < minDistance)
            currentWaypoint++;
        if (currentWaypoint >= waypoint.Length)
            currentWaypoint = 0;

        tracker.transform.LookAt(waypoint[currentWaypoint].transform);
        tracker.transform.Translate(0, 0, (trackerSpeed) * Time.deltaTime);

    }

    // Update is called once per frame
    void Update()
    {

        Tracker();

        Quaternion lookAtWaypoint =Quaternion.LookRotation(tracker.transform.position - this.transform.position);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, lookAtWaypoint, rotationSpeed * Time.deltaTime);
        this.transform.Translate(0, 0, agentSpeed * Time.deltaTime);

    }

}
