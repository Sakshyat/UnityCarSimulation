using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDetect : MonoBehaviour
{
    public GameObject agent;

    private IEnumerator OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collide"))
        {
            yield return new WaitForSeconds(0);
            agent.transform.position = new Vector3(agent.transform.position.x + 3, agent.transform.position.y, agent.transform.position.z + 3);
        }

    }


}
