using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : MonoBehaviour
{
    public GameObject endPoint;
    private NavMeshAgent agent;
    // Start is called before the first frame update
    private void Awake() {
        agent = GetComponent<NavMeshAgent>();
        endPoint = GameObject.Find("End");
    }
    void Start()
    {
        agent.destination = endPoint.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
