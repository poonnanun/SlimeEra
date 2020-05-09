using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject buildUI;
    public GameObject unitUI;
    private GameObject selectedFloor;
    private GameObject selectedWall;
    private Vector3 orginalPosition;
    private int state;

    void Awake()
    {
        state = 0;
        orginalPosition = new Vector3(0, 100, 0);
    }

    void Update()
    {
        
    }

    public void SelectWall(GameObject floor){
        selectedFloor = floor;
        state = 1;
        buildUI.transform.position = selectedFloor.transform.position;
    }
    public void SelectUnit(GameObject wall){
        selectedWall = wall;
        state = 1;
        unitUI.transform.position = selectedWall.transform.position;
    }

    public void BuildWall(GameObject building){
        Vector3 newPos = new Vector3(selectedFloor.transform.position.x, selectedFloor.transform.position.y+1, selectedFloor.transform.position.z);
        GameObject tmp = Instantiate(building, newPos, selectedFloor.transform.rotation);
        buildUI.transform.position =  orginalPosition;
        state = 0;
    }
    public void BuildUnit(GameObject building){
        Vector3 newPos = new Vector3(selectedWall.transform.position.x, selectedWall.transform.position.y+1, selectedWall.transform.position.z);
        GameObject tmp = Instantiate(building, newPos, selectedWall.transform.rotation);
        unitUI.transform.position =  orginalPosition;
        selectedWall.GetComponent<WallScript>().setHasUnit(1);
        state = 0;
    }

    public void CloseUI(){
        buildUI.transform.position =  orginalPosition;
        state = 0;
    }
    public void setState(int state){
        this.state = state;
    }

    public int getState(){
        return state;
    }
}
