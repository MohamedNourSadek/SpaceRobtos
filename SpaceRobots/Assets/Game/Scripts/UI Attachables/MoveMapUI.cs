using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMapUI : MonoBehaviour
{
    public float speed = 1f;
    public List<GameObject> movingMaps;
    public GameObject spawnPoint;
    public GameObject endPoint;


    public void FixedUpdate()
    {
        foreach(var map in movingMaps)
        {
            map.transform.localPosition += (speed * map.transform.up);

            if(speed > 0f)
            {
                if ((map.transform.position.x >= endPoint.transform.position.x)
                && (map.transform.position.y >= endPoint.transform.position.y))
                {
                    map.transform.position = spawnPoint.transform.position;
                }
            }
            else
            {
                if ((map.transform.position.x <= endPoint.transform.position.x)
                && (map.transform.position.y <= endPoint.transform.position.y))
                {
                    map.transform.position = spawnPoint.transform.position;
                }
            }
        }

    }


}
