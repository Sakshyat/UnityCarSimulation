using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collide : MonoBehaviour
{
    public GameObject agentOne, agentTwo;
    public float stopSpeed;
    // Start is called before the first frame update
    void Start()
    {
        stopSpeed = GetComponent<PathfindingTester>().speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, agentOne.transform.position) < 3 || Vector3.Distance(transform.position, agentTwo.transform.position) < 3)
        {
            GetComponent<Transform>().position += new Vector3(3, 0, 3);
            if (GetComponent<PathfindingTester>().speed < agentOne.GetComponent<PathfindingTester>().speed || GetComponent<PathfindingTester>().speed < agentTwo.GetComponent<PathfindingTester>().speed)
            {
                GetComponent<PathfindingTester>().speed = 0;

                //Debug.Log(GetComponent<PathfindingTester>().collectingSpeed);
            }
            GetComponent<PathfindingTester>().speed = 0;
            //Debug.Log(GetComponent<PathfindingTester>().speed);
        }
        else { GetComponent<PathfindingTester>().speed = stopSpeed; }
    }
   
}
