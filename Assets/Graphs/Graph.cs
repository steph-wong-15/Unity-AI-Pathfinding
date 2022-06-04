using System.Collections.Generic;
using UnityEngine;

public class Graph {

    List<Edge> edges = new List<Edge>();
    List<Node> nodes = new List<Node>();
    List<Node> pathList = new List<Node>();

    //my implementation of A*
    //used tutorial https://www.youtube.com/watch?v=mZfyt03LDH4
    //referenced source code used in above tutorial
    public void myAStar(GameObject startId, GameObject endId)
    {
        Node start = findNode(startId);
        Node end = findNode(endId);

        //make a list called open to keep unvisited nodes
        List<Node> open = new List<Node>();
        List<Node> closed = new List<Node>();

        //take the start node and add to open list
        open.Add(start);

        //initially the node is the start node
        Node current = start;

        //while there are nodes that haven't been visited
        while (open.Count > 0)
        {
            for( int counter = 1; counter < open.Count; counter++)
            {
                //calculate f cost for each node in unvisited
                open[counter].f = open[counter].g + open[counter].h;
                //calculate f cost for current node
                current.f = current.h + current.g;

                //if f cost of node in unvisited list is less than or equal to current
                if ( open[counter].f <= current.f)
                {
                    //if h cost of unvisited is less than current set current to node in unvisited
                    if(open[counter].h < current.h)
                    {
                        current = open[counter];
                    }
                }
            }

            //move current from unvisited list to visited
            open.Remove(current);
            closed.Add(current);

            //current node reached end node so return
            if (current == end)
            {
                reconstructPath(start, end);
                printPath();
                return;
            }

            foreach (Edge edge in current.edgelist)
            {
                //get cost of nodes beside current
                Node neighbour = edge.endNode;
                float updatedCostNeighbour = current.g + distance(current, neighbour);


                if (neighbour.g > updatedCostNeighbour)
                {
                    //update cost if new path has less cost
                    neighbour.g = updatedCostNeighbour;
                    //get new h cost
                    neighbour.h = distance(neighbour, end);
                    //parent of neighbour is current
                    neighbour.cameFrom = current;
                }

                //if neighbour is not in open, add to open
                if (open.Contains(neighbour) == false)
                {
                    open.Add(neighbour);
                }
            }
        }
        return;
    }

    //code used from unity waypoints tutorial
    public Graph() { }

    public void AddNode(GameObject id, bool removeRenderer = true, bool removeCollider = true, bool removeId = true) {
        Node node = new Node(id);
        nodes.Add(node);

        //remove colliders and mesh renderer
        if (removeCollider)
            GameObject.Destroy(id.GetComponent<Collider>());
        if (removeRenderer)
            GameObject.Destroy(id.GetComponent<Renderer>());
        if (removeId) {
            TextMesh[] textms = id.GetComponentsInChildren<TextMesh>() as TextMesh[];

            foreach (TextMesh tm in textms)
                GameObject.Destroy(tm.gameObject);
        }
    }

    public void AddEdge(GameObject fromNode, GameObject toNode) {
        Node from = findNode(fromNode);
        Node to = findNode(toNode);

        if (from != null && to != null) {
            Edge e = new Edge(from, to);
            edges.Add(e);
            from.edgelist.Add(e);
        }
    }

    Node findNode(GameObject id) {
        foreach (Node n in nodes) {
            if (n.getId() == id)
                return n;
        }
        return null;
    }

    public int getPathLength() {
        return pathList.Count;
    }

    public GameObject getPathPoint(int index) {
        return pathList[index].id;
    }

    public void printPath() {
        foreach (Node n in pathList) {
            Debug.Log(n.id.name);
        }
    }

    public bool AStar(GameObject startId, GameObject endId) {
        Node start = findNode(startId);
        Node end = findNode(endId);

        if (start == null || end == null) {
            return false;
        }

        List<Node> open = new List<Node>();
        List<Node> closed = new List<Node>();
        float tentative_g_score = 0;
        bool tentative_is_better;

        start.g = 0;
        start.h = distance(start, end);
        start.f = start.h;
        open.Add(start);

        while (open.Count > 0) {
            int i = lowestF(open);
            Node thisnode = open[i];
            if (thisnode.id == endId)  //path found
            {
                reconstructPath(start, end);
                printPath();
                return true;
            }

            open.RemoveAt(i);
            closed.Add(thisnode);

            Node neighbour;
            foreach (Edge e in thisnode.edgelist) {
                neighbour = e.endNode;
                neighbour.g = thisnode.g + distance(thisnode, neighbour);

                if (closed.IndexOf(neighbour) > -1)
                    continue;

                tentative_g_score = thisnode.g + distance(thisnode, neighbour);

                if (open.IndexOf(neighbour) == -1) {
                    open.Add(neighbour);
                    tentative_is_better = true;
                } else if (tentative_g_score < neighbour.g) {
                    tentative_is_better = true;
                } else
                    tentative_is_better = false;

                if (tentative_is_better) {
                    neighbour.cameFrom = thisnode;
                    neighbour.g = tentative_g_score;
                    //neighbour.h = distance(thisnode,neighbour);
                    neighbour.h = distance(thisnode, end);
                    neighbour.f = neighbour.g + neighbour.h;
                }
            }

        }

        return false;
    }

    public void reconstructPath(Node startId, Node endId) {
        pathList.Clear();
        pathList.Add(endId);

        var p = endId.cameFrom;
        while (p != startId && p != null) {
            pathList.Insert(0, p);
            p = p.cameFrom;
        }
        pathList.Insert(0, startId);
    }

    float distance(Node a, Node b) {
        float dx = a.xPos - b.xPos;
        float dy = a.yPos - b.yPos;
        float dz = a.zPos - b.zPos;
        float dist = dx * dx + dy * dy + dz * dz;
        return (dist);
    }

    int lowestF(List<Node> l) {
        float lowestf = 0;
        int count = 0;
        int iteratorCount = 0;

        for (int i = 0; i < l.Count; i++) {
            if (i == 0) {
                lowestf = l[i].f;
                iteratorCount = count;
            } else if (l[i].f <= lowestf) {
                lowestf = l[i].f;
                iteratorCount = count;
            }
            count++;
        }
        return iteratorCount;
    }

    public void debugDraw() {
        //draw edges
        for (int i = 0; i < edges.Count; i++) {
            Debug.DrawLine(edges[i].startNode.id.transform.position, edges[i].endNode.id.transform.position, Color.red);

        }
        //draw directions
        for (int i = 0; i < edges.Count; i++) {
            Vector3 to = (edges[i].startNode.id.transform.position - edges[i].endNode.id.transform.position) * 0.05f;
            Debug.DrawRay(edges[i].endNode.id.transform.position, to, Color.blue);
        }
    }
}