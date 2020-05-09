using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScript : MonoBehaviour
{
    public GameObject monster;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnMouseDown() {
        GameObject tmp = Instantiate(monster, transform.position, transform.rotation);
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
