using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScript : MonoBehaviour
{
    public GameObject monster;
    private GameManager gameManager;
    private int amount;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    
    public void Spawn(int amount){
        this.amount = amount;
        InvokeRepeating("NormalSpawn", 1, 1);
    }

    public void NormalSpawn(){
        GameObject tmp = Instantiate(monster, transform.position, transform.rotation);
        gameManager.AddMonster(tmp);
        if (--amount == 0){
            gameManager.FinishSpawn();
            CancelInvoke("NormalSpawn");
        } 
    }
}
