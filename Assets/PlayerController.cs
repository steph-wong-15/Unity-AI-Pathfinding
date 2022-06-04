using UnityEngine;
using UnityEngine.AI;


public class PlayerController : MonoBehaviour
{

    public Camera cam;
    public NavMeshAgent agent;
    public Vector3 endPoint;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);
            }
        }

        // referenced /https://stackoverflow.com/questions/45914775/i-used-the-c-sharp-stopwatch-class-in-unity-but-it-returns-only-000000

       // agent.SetDestination(endPoint);


    }
}