using UnityEngine;

public class FollowPath : MonoBehaviour {

    Transform goal;
    float speed = 5.0f;
    float accuracy = 2.0f;
    float rotSpeed = 2.0f;

    // Access to the WPManager script
    public GameObject wpManager;
    // Array of waypoints
    GameObject[] wps;
    // Current waypoint
    GameObject currentNode;
    // Starting waypoint index
    int currentWP = 0;
    // Access to the Graph script
    Graph g;

    // Use this for initialization
    void Start() {

        // Get hold of wpManager and Graph scripts
        wps = wpManager.GetComponent<WPManager>().waypoints;
        g = wpManager.GetComponent<WPManager>().graph;
        // Set the current Node
        currentNode = wps[1];
    }

    public void Go() {

        // Use the AStar method passing it currentNode and distination
        g.AStar(currentNode, wps[23]);
        // Reset index
        currentWP = 0;
    }

    public void MyGo() {

        // Use the AStar method passing it currentNode and distination
        g.myAStar(currentNode, wps[23]);
        //Debug.Log("Graph = " + g.getPathPoint(currentWP));
        // Reset index
        currentWP = 0;
    }


    // Update is called once per frame
    void LateUpdate() {

        // If we've nowhere to go then just return
        if (g.getPathLength() == 0 || currentWP == g.getPathLength())
            return;

        //the node we are closest to at this moment
        currentNode = g.getPathPoint(currentWP);

        //if we are close enough to the current waypoint move to next
        if (Vector3.Distance(
            g.getPathPoint(currentWP).transform.position,
            transform.position) < accuracy) {
            currentWP++;
        }

        //if we are not at the end of the path
        if (currentWP < g.getPathLength()) {
            goal = g.getPathPoint(currentWP).transform;
            Vector3 lookAtGoal = new Vector3(goal.position.x,
                                            this.transform.position.y,
                                            goal.position.z);
            Vector3 direction = lookAtGoal - this.transform.position;

            // Rotate towards the heading
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation,
                                                    Quaternion.LookRotation(direction),
                                                    Time.deltaTime * rotSpeed);

            // Move
            this.transform.Translate(0, 0, speed * Time.deltaTime);
        }

    }
}