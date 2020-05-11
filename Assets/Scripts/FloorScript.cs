using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorScript : MonoBehaviour
{
    public Material normal;
    public Material onHover;
    public Material error;
    public Material path;
    public GameObject invis;
    private Renderer rend;
    private GameManager gameManager;
    private bool isPath;
    private bool hasWall;
    private bool isSelected;
    private GameObject invisWall;
    void Start()
    {
        invisWall = null;
        hasWall = false;
        isPath = false;
        gameManager = FindObjectOfType<GameManager>();
        rend = GetComponent<Renderer>();
    }

    private void OnMouseEnter() {
        if(gameManager.GetState() == 0 && gameManager.GetWallMax() == false){
            if (rend != null){
                rend.material = onHover;
            }
        }
    }

    private void OnMouseExit() {
        if(!isSelected){
            if (rend != null && !isPath){
                rend.material = normal;
            }else if(rend != null && isPath){
                rend.material = path;
            }
        } 
    }
    private void OnMouseDown() {
        if(gameManager.GetState() == 0 && gameManager.GetWallMax() == false){
            gameManager.SelectFloor(gameObject);
            isSelected = true;
        }
    }
    public void ErrorPlace(){
        isSelected = false;
        if (rend != null){
            rend.material = error;
        }
        Invoke("OnMouseExit", 0.5f);
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
    public void PlaceObstruct(){
        if(!isPath && !hasWall){
            Vector3 newPos = new Vector3(transform.position.x, transform.position.y+1, transform.position.z);
            invisWall = Instantiate(invis, newPos, transform.rotation);
        }
    }

    public void DeleteObstruct(){
        if(invisWall != null){
            Destroy(invisWall);
        }
    }
    public void setHasWall(bool hasWall){
        this.hasWall = hasWall;
        isSelected = false;
        if (rend != null){
            rend.material = normal;
        }
    }

    public void SetSelect(bool isSelected){
        this.isSelected = isSelected;
        if (rend != null && !isPath){
            rend.material = normal;
        }else if(rend != null && isPath){
            rend.material = path;
        }
    }
}
