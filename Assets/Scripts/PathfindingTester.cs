using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PathfindingTester : MonoBehaviour
{
    // The A* manager.
    private AStarManager AStarManager = new AStarManager();
    // Array of possible waypoints.
    List<GameObject> Waypoints = new List<GameObject>();
    // Array of waypoint map connections. Represents a path.
    List<Connection> ConnectionArray = new List<Connection>();
    // The start and end target point.
    public GameObject start;
    public GameObject goal1;
    public GameObject goal2;
    public GameObject goal3;
    // Debug line offset.
    Vector3 OffSet = new Vector3(0, 0.3f, 0);
    private int count;
    public int agentCount = 0;
    public Text parcelCount;
    public Text agent;
    public Text agentSpeed;
    public int pick = 0;
    private bool reverse = false;
    public float speed = 20;
    private int stop = 0;
    public Text distanceTravelled;
    public Text timeTaken;
    private float distance;
    private float time;
    Vector3 lastPosition;
    // Start is called before the first frame update
    void Start()
    {
        parcelCount.text = "Parcels: ";
        agent.text = "Car: " + agentCount;
        agentSpeed.text = "Speed: ";

        lastPosition = transform.position;

        if (start == null || goal1 == null || goal2 == null || goal3 == null)
        {
            Debug.Log("No start or end waypoints.");
            return;
        }
        // Find all the waypoints in the level.
        GameObject[] GameObjectsWithWaypointTag;
        GameObjectsWithWaypointTag = GameObject.FindGameObjectsWithTag("Waypoint");
        foreach (GameObject waypoint in GameObjectsWithWaypointTag)
        {
            WaypointCON tmpWaypointCon = waypoint.GetComponent<WaypointCON>();
            if (tmpWaypointCon)
            {
                Waypoints.Add(waypoint);
            }
        }
        // Go through the waypoints and create connections.
        foreach (GameObject waypoint in Waypoints)
        {
            WaypointCON tmpWaypointCon = waypoint.GetComponent<WaypointCON>();
            // Loop through a waypoints connections.
            foreach (GameObject WaypointConNode in tmpWaypointCon.Connections)
            {
                Connection aConnection = new Connection();
                aConnection.SetFromNode(waypoint);
                aConnection.SetToNode(WaypointConNode);
                AStarManager.AddConnection(aConnection);
            }
        }
        if (Vector3.Distance(start.transform.position, goal1.transform.position) < Vector3.Distance(start.transform.position, goal2.transform.position)
            || Vector3.Distance(start.transform.position, goal1.transform.position) < Vector3.Distance(start.transform.position, goal3.transform.position))
        {
            // Run A Star...
            ConnectionArray = AStarManager.PathfindAStar(start, goal1);
        }
        else if (Vector3.Distance(start.transform.position, goal2.transform.position) < Vector3.Distance(start.transform.position, goal1.transform.position)
            || Vector3.Distance(start.transform.position, goal2.transform.position) < Vector3.Distance(start.transform.position, goal3.transform.position))
        {
            // Run A Star...
            ConnectionArray = AStarManager.PathfindAStar(start, goal2);
        }
        else
        {
            // Run A Star...
            ConnectionArray = AStarManager.PathfindAStar(start, goal3);
        }

    }
    // Draws debug objects in the editor and during editor play (if option set).
    void OnDrawGizmos()
    {
        // Draw path.
        foreach (Connection aConnection in ConnectionArray)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine((aConnection.GetFromNode().transform.position + OffSet),
            (aConnection.GetToNode().transform.position + OffSet));
        }
    }



    void FixedUpdate()
    {
        if (speed - pick * 2 <= 2)
        {

            speed = 2;
            Debug.Log(speed);

        }
        else
        {

            if (pick > 0)
            {

                speed = speed - pick * 2;
                Debug.Log(speed);
            }


        }
        // Calculate distance value between character and checkpoint
        distance += Vector3.Distance(transform.position, lastPosition);
        lastPosition = transform.position;
        distanceTravelled.text = "Distance: " + distance.ToString("F1") + "KM";
        time = distance / speed;
        timeTaken.text = "Time " + time.ToString();

        // if block: to prevent index out of bound
        if (count < ConnectionArray.Count)
        {
            if (!reverse)
            {
                // this will run while going from source to destination
                // if block: to detect if cube reached its destination
                if (transform.position != ConnectionArray[count].GetToNode().transform.position)
                {
                    transform.LookAt(ConnectionArray[count].GetToNode().transform);
                    //move forward to the position
                    transform.position = Vector3.MoveTowards(transform.position, ConnectionArray[count].GetToNode().transform.position, Time.deltaTime * speed);
                    agentSpeed.text = "Speed: " + speed + "km/h";
                    parcelCount.text = "Parcels: " + pick;
                    if (speed != 2)
                    {
                        speed = 20;
                        Debug.Log(speed);
                    }
                }
                else
                {
                    // if cube us reached to its destination then increment count by 1
                    count++;
                    if (speed != 2)
                    {
                        speed = 20;
                        Debug.Log(speed);
                    }
                }
            }
            if (count == ConnectionArray.Count)
            {
                if (stop < 3)
                {
                    stop += 1;
                    count = 0;
                    if (stop == 1)
                    {
                        // Run A Star...
                        ConnectionArray = AStarManager.PathfindAStar(goal1, goal2);
                        pick = pick - 1;
                    }
                    if (stop == 2)
                    {
                        // Run A Star...
                        ConnectionArray = AStarManager.PathfindAStar(goal2, goal3);
                        pick = pick -1;
                    }
                    if (stop == 3)
                    {
                        // Run A Star...
                        ConnectionArray = AStarManager.PathfindAStar(goal3, start);
                        speed = 0;
                        pick = 0;
                    }
                }
            }
        }
        if (count < ConnectionArray.Count)
        {
            if (!reverse)
            {
                if (transform.position != ConnectionArray[count].GetToNode().transform.position)
                {
                    transform.LookAt(ConnectionArray[count].GetToNode().transform);
                    transform.position = Vector3.MoveTowards(transform.position, ConnectionArray[count].GetToNode().transform.position, Time.deltaTime * speed);
                    //reset the speed 
                    if (speed != 2)
                    {
                        speed = 20;
                    }

                }
                else
                {
                    count++;

                    //to reset the speed
                    if (speed != 2)
                    {
                        speed = 20;
                    }
                }
            }
        }
        if (count == ConnectionArray.Count)
        {
            if (stop < 3)
            {
                stop += 1;
                count = 0;

                if (stop == 1)
                {
                    // Run A Star...
                    ConnectionArray = AStarManager.PathfindAStar(goal1, goal2);
                    pick = pick - 1; 
                }
                if (stop == 2)
                {
                    // Run A Star...
                    ConnectionArray = AStarManager.PathfindAStar(goal2, goal3);
                    pick = pick - 1;
                }
                if (stop == 3)
                {
                    // Run A Star...
                    ConnectionArray = AStarManager.PathfindAStar(goal3, start);
                    speed = 20;
                    pick = 0;
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pickup"))
        {
            other.gameObject.transform.localScale = new Vector3(2 ,2, 2);
        }
    }
}

