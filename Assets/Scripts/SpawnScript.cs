using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScript : MonoBehaviour
{
    public GameObject normalMonster;
    public GameObject miniBoss;
    public GameObject boss;
    private GameManager gameManager;
    private int amount;
    private int wave;
    private int normalHp;
    private int thisWaveHp;
    private int condition;
    // Start is called before the first frame update
    void Start()
    {
        condition = 0;
        thisWaveHp =100;
        normalHp = 100;
        gameManager = FindObjectOfType<GameManager>();
    }
    
    public void Spawn(int wave){
        normalHp += 10;
        if(wave%10==0){
            int rand = Random.Range(1,3);
            if(rand == 3){
                //Double hp
                this.amount = 1;
                thisWaveHp = normalHp*2;
                condition = 3;
                InvokeRepeating("BossSpawn", 1, 1);
            }else if(rand == 2){
                //Double speed
                this.amount = 1;
                thisWaveHp = normalHp;
                condition = 2;
                InvokeRepeating("BossSpawn", 1, 1);
            }else{
                //Double boss
                this.amount = 2;
                thisWaveHp = normalHp;
                condition = 1;
                InvokeRepeating("BossSpawn", 1, 1);
            }
        }else if(wave%5==0){
            int rand = Random.Range(1,3);
            if(rand == 3){
                //Double hp
                this.amount = wave*2;
                thisWaveHp = normalHp*2;
                condition = 3;
                InvokeRepeating("MiniBossSpawn", 1, 1);
            }else if(rand == 2){
                //Double amount
                this.amount = wave*4;
                thisWaveHp = normalHp;
                condition = 2;
                InvokeRepeating("MiniBossSpawn", 1, 1);
            }else{
                //Only miniboss
                this.amount = (int)Mathf.Floor((wave/5)*2);
                thisWaveHp = normalHp;
                condition = 1;
                InvokeRepeating("MiniBossSpawn", 1, 1);
            }
        }else{
            this.amount = wave*2;
            thisWaveHp = normalHp;
            InvokeRepeating("NormalSpawn", 1, 1);
        }
        
    }

    public void NormalSpawn(){
        if (amount == 0){
            gameManager.FinishSpawn();
            CancelInvoke("NormalSpawn");
            return;
        } 
        GameObject tmp = Instantiate(normalMonster, transform.position, transform.rotation);
        tmp.GetComponent<MonsterController>().SetHp(normalHp);
        gameManager.AddMonster(tmp);
        amount--;
    }
    private void MiniBossSpawn(){
        if(amount == 0){
            GameObject mini = Instantiate(miniBoss, transform.position, transform.rotation);
            mini.GetComponent<MonsterController>().SetHp(thisWaveHp*2);
            gameManager.AddMonster(mini);
            gameManager.FinishSpawn();
            CancelInvoke("MiniBossSpawn");
            return;
        } 
        if(condition != 1){
            GameObject normal = Instantiate(normalMonster, transform.position, transform.rotation);
            normal.GetComponent<MonsterController>().SetHp(thisWaveHp);
            gameManager.AddMonster(normal);
        }else{
            GameObject normal = Instantiate(miniBoss, transform.position, transform.rotation);
            normal.GetComponent<MonsterController>().SetHp(thisWaveHp*2);
            gameManager.AddMonster(normal);
        }
        amount--;
    }
    private void BossSpawn(){
        if(amount == 0){
            gameManager.FinishSpawn();
            CancelInvoke("BossSpawn");
            return;
        } 
        GameObject bo = Instantiate(boss, transform.position, transform.rotation);
        bo.GetComponent<MonsterController>().SetHp(thisWaveHp*5);
        gameManager.AddMonster(bo);
        amount--;
    }
}
