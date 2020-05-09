using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallScript : MonoBehaviour
{
    public Material normal;
    public Material onHover;
    private Renderer rend;
    private GameManager gameManager;
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
        if (rend != null){
            rend.material = normal;
        }
    }
    private void OnMouseDown() {
        if(gameManager.GetState() == 0 && hasUnit == 0){
            gameManager.SelectUnit(gameObject);
        }
    }
    public void setHasUnit(int hasUnit){
        this.hasUnit = hasUnit;
    }
}
