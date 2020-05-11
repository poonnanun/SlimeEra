using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallScript : MonoBehaviour
{
    public Material normal;
    public Material onHover;
    public Material error;
    private Renderer rend;
    private GameManager gameManager;
    private bool isSelected;
    private int hasUnit;
    // Start is called before the first frame update
    void Start()
    {
        hasUnit = 0;
        gameManager = FindObjectOfType<GameManager>();
        rend = GetComponent<Renderer>();
    }

    private void OnMouseEnter() {
        if(gameManager.GetState() == 0 && hasUnit == 0){
            if (rend != null){
                rend.material = onHover;
            }
        }
    }

    private void OnMouseExit() {
        if(!isSelected){
            if (rend != null){
                rend.material = normal;
            }
        }
    }
    private void OnMouseDown() {
            if(gameManager.GetState() == 0 && hasUnit == 0){
                isSelected = true;
                gameManager.SelectWall(gameObject);
            }
    }
    public void ErrorPlace(){
        isSelected = false;
        if (rend != null){
            rend.material = error;
        }
        Invoke("OnMouseExit", 0.5f);
    }

    public void setHasUnit(int hasUnit){
        this.hasUnit = hasUnit;
        isSelected = false;
        if (rend != null){
            rend.material = normal;
        }
    }
    public void SetSelect(bool isSelected){
        this.isSelected = isSelected;
        if (rend != null){
            rend.material = normal;
        }
    }
}
