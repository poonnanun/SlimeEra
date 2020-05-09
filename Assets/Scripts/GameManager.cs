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
    public GameObject wallPrefabs;
    public GameObject positionTrigger;
    private SpawnScript spawnScript;
    private GameObject selectedFloor;
    private GameObject selectedWall;
    private Vector3 orginalPosition;
    private List<GameObject> monsters;
    private Dictionary<int, bool> floorsPos;
    private Dictionary<int, int> posParents;
    private List<GameObject> highlightedPath;
    private List<int> exploredFloors;
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
        DeclareObject();
        InitializeValue();
    }

    private void Start() {
        FindShotestPath();
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
        for(int x=0; x<9; x++){
            for(int z=0; z<9; z++){
                string tmp = x.ToString()+z.ToString();
                floorsPos.Add(int.Parse(tmp), true);
            }
        }
        print("List Count = "+floorsPos.Count);
        currencyText.text = currency.ToString();
        wallDeployText.text = string.Format("{0}/{1}", wallDeploy.ToString(), maxWallDeploy.ToString());
    }
    private void DeclareObject(){
        floorsPos = new Dictionary<int, bool>();
        posParents = new Dictionary<int, int>();
        spawnScript = FindObjectOfType<SpawnScript>();
        monsters = new List<GameObject>();
        highlightedPath = new List<GameObject>();
        exploredFloors = new List<int>();
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
           
            buildUI.transform.position =  orginalPosition;
            if(building == wallPrefabs){
                string tmp = Mathf.RoundToInt(selectedFloor.transform.position.x).ToString()+Mathf.RoundToInt(selectedFloor.transform.position.z).ToString();
                floorsPos[int.Parse(tmp)] = false;
                if(!FindShotestPath()){
                    selectedFloor.GetComponent<FloorScript>().ErrorPlace();
                    floorsPos[int.Parse(tmp)] = true;
                }else{
                    GameObject newObj = Instantiate(building, newPos, selectedFloor.transform.rotation);
                    wallDeploy++;
                    wallDeployText.text = string.Format("{0}/{1}", wallDeploy.ToString(), maxWallDeploy.ToString());
                }
            }else{
                GameObject newObj = Instantiate(building, newPos, selectedFloor.transform.rotation);
                wallDeploy++;
                wallDeployText.text = string.Format("{0}/{1}", wallDeploy.ToString(), maxWallDeploy.ToString());
            }
            state = 0;
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
    private bool FindShotestPath(){
        exploredFloors.Clear();
        posParents.Clear();
        Queue<int> queue = new Queue<int>();
        queue.Enqueue(40);
        while(queue.Count < 1000 && queue.Count > 0){
            // print("queue count = "+queue.Count);
            int currPos = queue.Dequeue();
            // print(" num = "+ currPos);
            if(currPos == 48){
                showShotestPath();
                return true;
            }else if(exploredFloors.Contains(currPos)){

            }
            else{
                List<int> tmp = WalkAblePath(currPos);
                if(tmp.Count != 0){
                    foreach(int i in tmp){
                        queue.Enqueue(i);
                    }
                }
                exploredFloors.Add(currPos);
            }
        }
        return false;
    }
    private void showShotestPath(){
        foreach(GameObject d in highlightedPath){
            d.GetComponent<FloorScript>().PathDisable();
        }
        highlightedPath.Clear();
        List<int> path = new List<int>();
        int finishPos = 48;
        int curr = finishPos;
        int count = 0;
        while(curr != 40){
            path.Add(curr);
            curr = posParents[curr];
            count++;
            if(count > 1000){
                break;
            }
        }
        foreach(int i in path){
            float z = (float)Mathf.Floor(i%10);
            float x = (float)Mathf.Floor(i/10);
            // print("X="+ x + ", Z=" +z);
            positionTrigger.transform.position = new Vector3(x, -0.5f, z);
            Collider[] hitColliders = Physics.OverlapSphere(positionTrigger.transform.position, 0.25f);
            int c = 0;
            while (c < hitColliders.Length)
            {
                highlightedPath.Add(hitColliders[c].gameObject);
                c++;
            }
        }
        positionTrigger.transform.position = new Vector3(0f, 150f, 0f);
        foreach(GameObject d in highlightedPath){
            d.GetComponent<FloorScript>().PathEnable();
        }
    }
    public void addPath(GameObject path){
        print(1);
        highlightedPath.Add(path);
    }
    private List<int> WalkAblePath(int currPosition){
        List<int> tmp = new List<int>();
        if(!exploredFloors.Contains(currPosition+1)){
            if(currPosition+1 == 48){
                if(!posParents.ContainsKey(48))
                    posParents.Add(48,currPosition);
            }
            if(floorsPos.ContainsKey(currPosition+1)){
                if(floorsPos[currPosition+1] == true){
                    tmp.Add(currPosition+1);
                }
            }
        }else{
            if(!posParents.ContainsKey(currPosition)){
                posParents.Add(currPosition,currPosition+1);
            }
        }
        if(!exploredFloors.Contains(currPosition-10)){
            if(currPosition-10 == 48){
                if(!posParents.ContainsKey(48))
                    posParents.Add(48,currPosition);
            }
            if(floorsPos.ContainsKey(currPosition-10)){
                if(floorsPos[currPosition-10] == true){
                    tmp.Add(currPosition-10);
                }
            }
        }else{
            if(!posParents.ContainsKey(currPosition)){
                posParents.Add(currPosition,currPosition-10);
            }
                
        }
        if(!exploredFloors.Contains(currPosition+10)){
            if(currPosition+10 == 48){
                if(!posParents.ContainsKey(48))
                    posParents.Add(48,currPosition);
            }
            if(floorsPos.ContainsKey(currPosition+10)){
                if(floorsPos[currPosition+10] == true){
                    tmp.Add(currPosition+10);
                }
            }
        }else{
            if(!posParents.ContainsKey(currPosition)){
                posParents.Add(currPosition,currPosition+10);
            }
                
        }
        if(!exploredFloors.Contains(currPosition-1)){
            if(currPosition-1 == 48){
                if(!posParents.ContainsKey(48))
                    posParents.Add(48,currPosition);
            }
            if(floorsPos.ContainsKey(currPosition-1)){
                if(floorsPos[currPosition-1] == true){
                tmp.Add(currPosition-1);
                }
            }
        }else{
            if(!posParents.ContainsKey(currPosition)){
                posParents.Add(currPosition,currPosition-1);
            }
        }
        
        return tmp;
    }
}
