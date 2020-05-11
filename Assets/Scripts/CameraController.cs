using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera cam;
    Rigidbody rg;
    // Start is called before the first frame update
    void Start()
    {
        rg = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W)){
            rg.AddForce(Vector3.left);
        }   
        else if (Input.GetKey(KeyCode.S)){
            rg.AddForce(Vector3.right);
        }    
        else if (Input.GetKey(KeyCode.D)){
            rg.AddForce(new Vector3(0,0,1));
        }    
        else if (Input.GetKey(KeyCode.A)){
            rg.AddForce(new Vector3(0,0,-1));
        }
        else{
            rg.velocity = Vector3.zero;
            rg.angularVelocity = Vector3.zero;
        }
        cam.transform.position = this.gameObject.transform.position;
    }
}
