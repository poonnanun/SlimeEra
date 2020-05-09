using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorScript : MonoBehaviour
{
    public Material normal;
    public Material onHover;
    public Material error;
    public Material path;
    private Renderer rend;
    private GameManager gameManager;
    private bool isPath;
    void Start()
    {
        isPath = false;
        gameManager = FindObjectOfType<GameManager>();
        rend = GetComponent<Renderer>();
    }

    private void OnMouseEnter() {
        if(gameManager.GetState() == 0 && gameManager.getWallMax() == false){
            if (rend != null){
                rend.material = onHover;
            }
        }
    }

    private void OnMouseExit() {
        if (rend != null && !isPath){
            rend.material = normal;
        }else if(rend != null && isPath){
            rend.material = path;
        }
    }
    private void OnMouseDown() {
        if(gameManager.GetState() == 0 && gameManager.getWallMax() == false){
            gameManager.SelectWall(gameObject);
        }
    }
    public void ErrorPlace(){
        if (rend != null){
            rend.material = error;
        }
    }

    public void PathEnable(){
        if (rend != null){
            rend.material = path;
        }
        isPath = true;
    }
    public void PathDisable(){
        if (rend != null){
            rend.material = normal;
        }
        isPath = false;
    }
}
