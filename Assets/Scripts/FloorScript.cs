using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorScript : MonoBehaviour
{
    public Material normal;
    public Material onHover;
    private Renderer rend;
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        rend = GetComponent<Renderer>();
    }

    private void OnMouseEnter() {
        if(gameManager.getState() == 0){
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
        if(gameManager.getState() == 0){
            gameManager.SelectWall(gameObject);
        }
    }
}
