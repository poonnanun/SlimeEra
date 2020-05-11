using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : MonoBehaviour
{
    public GameObject endPoint;
    private GameManager gameManager;
    private NavMeshAgent agent;
    private float normalSpeed;
    private int hp;
    // Start is called before the first frame update
    private void Awake() {
        normalSpeed = 1f;
        gameManager = FindObjectOfType<GameManager>();
        agent = GetComponent<NavMeshAgent>();
        endPoint = GameObject.Find("End");
    }
    void Start()
    {
        agent.speed = normalSpeed;
        agent.destination = endPoint.transform.position;
        hp = 100;
    }

    public void SetSpeed(float speed){
        
    }
    public void SetHp(int hp){
        this.hp = hp;
    }
    public void Hit(int dmg){
        hp -= dmg;
        if(hp <= 0){
            died();
        }
    }
    public void died(){
        gameManager.MonsterDied(this.gameObject);
    }
    public void Slow(float speed, float duration){
        float tmp = normalSpeed-(speed/100);
        agent.speed = tmp;
        Invoke("ToNormalSpeed", duration);
    }
    public void ToNormalSpeed(){
        agent.speed = normalSpeed;
    }
}
