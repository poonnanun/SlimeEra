using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject buildUI;
    public GameObject unitUI;
    public Image phase;
    public Sprite sun;
    public Sprite moon;
    public GameObject skip;
    public Text lifeText;
    public Text waveText;
    public Text monsterLeftText;
    public Text currencyText;
    public Text wallDeployText;
    public GameObject gameOverPanel;
    private SpawnScript spawnScript;
    private GameObject selectedFloor;
    private GameObject selectedWall;
    private Vector3 orginalPosition;
    private List<GameObject> monsters;
    private int state;
    private int wave;
    private int life;
    private int currency;
    private int wallDeploy;
    private int maxWallDeploy;
    private int isWaveRunning;
    private bool wallMax;

    void Awake()
    {
        InitializeValue();
        DeclareObject();
    }
    void FixedUpdate()
    {
        if(isWaveRunning == 1){
            if(monsters.Count == 0){
                isWaveRunning = 0;
                FinishWave();
            }
        }
    }
    private void InitializeValue(){
        life = 10; // need to implement this later4
        wave = 1; // need to implement this later
        wallMax = false;
        currency = 0;
        wallDeploy = 0;
        maxWallDeploy = 10;
        isWaveRunning = 0;
        state = 0;
        orginalPosition = new Vector3(0, 100, 0);
        lifeText.text = life.ToString();
        waveText.text = wave.ToString();
        monsterLeftText.text = "0";
        currencyText.text = currency.ToString();
        wallDeployText.text = string.Format("{0}/{1}", wallDeploy.ToString(), maxWallDeploy.ToString());
    }
    private void DeclareObject(){
        spawnScript = FindObjectOfType<SpawnScript>();
        monsters = new List<GameObject>();
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
        if(wallMax == false){
            Vector3 newPos = new Vector3(selectedFloor.transform.position.x, selectedFloor.transform.position.y+1, selectedFloor.transform.position.z);
            GameObject tmp = Instantiate(building, newPos, selectedFloor.transform.rotation);
            buildUI.transform.position =  orginalPosition;
            state = 0;
            wallDeploy++;
            wallDeployText.text = string.Format("{0}/{1}", wallDeploy.ToString(), maxWallDeploy.ToString());
            if(wallDeploy == maxWallDeploy){
                wallMax = true;
            }
        }
        
    }
    public void BuildUnit(GameObject building){
        Vector3 newPos = new Vector3(selectedWall.transform.position.x, selectedWall.transform.position.y+1, selectedWall.transform.position.z);
        GameObject tmp = Instantiate(building, newPos, selectedWall.transform.rotation);
        unitUI.transform.position =  orginalPosition;
        selectedWall.GetComponent<WallScript>().setHasUnit(1);
        state = 0;
    }
    public void AddMonster(GameObject monster){
        monsters.Add(monster);
        monsterLeftText.text = monsters.Count.ToString();
    }
    public void FinishSpawn(){
        isWaveRunning = 1;
    }
    public List<GameObject> GetMonsters(){
        return monsters;
    }
    public void MonsterLeak(GameObject monster){
        monsters.Remove(monster);
        life -= 1;
        if(life == 0){
            GameOver();
        }
        lifeText.text = life.ToString();
        monsterLeftText.text = monsters.Count.ToString();
        Destroy(monster);
    }
    public void MonsterDied(GameObject monster){
        monsters.Remove(monster);
        monsterLeftText.text = monsters.Count.ToString();
        Destroy(monster);
    }    
    public void CloseUI(){
        buildUI.transform.position =  orginalPosition;
        unitUI.transform.position =  orginalPosition;
        state = 0;
    }
    public void SetState(int state){
        this.state = state;
    }
    public int GetState(){
        return state;
    }
    public bool getWallMax(){
        return wallMax;
    }
    public void Spawn(){
        spawnScript.Spawn(wave*2);
    }
    public void FinishWave(){
        wave += 1;
        waveText.text = wave.ToString();
        Day();
    }
    public void Day(){
        state = 0;
        phase.sprite = sun;
        skip.SetActive(true);
    }
    public void Night(){
        state = 1;
        phase.sprite = moon;
        skip.SetActive(false);
        Spawn();
    }
    public void GameOver(){
        gameOverPanel.SetActive(true);
    }
    public void MainMenu(){
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}
