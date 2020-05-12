using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class GameManager : MonoBehaviour
{
    public GameObject buildUI;
    public GameObject unitUI;
    public GameObject unitInfo;
    public GameObject gunnerInfo;
    public GameObject minerInfo;
    public GameObject trapInfo;
    public GameObject slowInfo;
    public GameObject upgradePath;
    public Image phase;
    public Sprite sun;
    public Sprite moon;
    public GameObject skip;
    public Text lifeText;
    public Text waveText;
    public Text monsterLeftText;
    public Text currencyText;
    public Text wallDeployText;
    public TMP_Text descriptionText;
    public GameObject description;
    public GameObject gameOverPanel;
    public GameObject wallPrefabs;
    public GameObject gunnerPrefabs;
    public GameObject minerPrefabs;
    public GameObject positionTrigger;
    public GameObject end;
    public List<GameObject> floors;
    public TMP_Text highestKillText;
    public TMP_Text currentKillText;
    public TMP_Text highestWaveText;
    public TMP_Text currentWaveText;
    private SpawnScript spawnScript;
    private GameObject selectedFloor;
    private GameObject selectedWall;
    private GameObject selectedUnit;
    private Vector3 orginalPosition;
    private List<GameObject> monsters;
    private List<GameObject> towers;
    private Dictionary<int, bool> floorsPos;
    private Dictionary<int, int> posParents;
    private List<GameObject> highlightedPath;
    private List<Upgrade> upgradesT1;
    private List<Upgrade> upgradesT2;
    private List<Upgrade> upgradesT3;
    private List<Upgrade> upgradesGunner;
    private List<Upgrade> upgradesMiner;
    private List<Upgrade> upgradesSlow;
    private List<Upgrade> upgradesTrap;
    private List<int> exploredFloors;
    private int state;
    private int wave;
    private int life;
    private int currency;
    private int wallDeploy;
    private int maxWallDeploy;
    private int isWaveRunning;
    private bool wallMax;
    private int gunnerCost;
    private int minerCost;
    private int trapCost;
    private int wallCost;
    private int monsterBounty;
    private int kill;
    public Color day;
    public Color night;
    public Color dayLight;
    public Color moonLight;
    public Camera cam;
    public Light dLight;
    private bool dusking;
    private bool dawning;
    public float duration;
    private float t = 0;
    void Awake()
    {
        DeclareObjects();
        InitializeValues();
        AddUpgrades();
    }

    private void Start() {
        FindShotestPath();
        cam.backgroundColor = day;
        dLight.color = dayLight;
    }
    void FixedUpdate()
    {
        if(isWaveRunning == 1){
            if(monsters.Count == 0){
                isWaveRunning = 0;
                FinishWave();
            }
        }
        if(dawning){
            t += Time.deltaTime / duration;
            cam.backgroundColor = Color.Lerp(night, day, t);
            dLight.color = Color.Lerp(moonLight, dayLight, t);
        }if(dusking){
            t += Time.deltaTime / duration;
            cam.backgroundColor = Color.Lerp(day, night, t);
            dLight.color = Color.Lerp(dayLight, moonLight, t);
        }
    }
    private void InitializeValues(){
        life = 10; // need to implement this later4
        wave = 1; // need to implement this later
        kill = 0;
        wallMax = false;
        currency = 200;
        wallDeploy = 0;
        maxWallDeploy = 20;
        isWaveRunning = 0;
        state = 0;
        gunnerCost = 25;
        minerCost = 50;
        trapCost = 25;
        wallCost = 10;
        monsterBounty = 2;
        dusking = false;
        dawning = false;
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
        currencyText.text = currency.ToString();
        wallDeployText.text = string.Format("{0}/{1}", wallDeploy.ToString(), maxWallDeploy.ToString()); 
    }
    private void DeclareObjects(){
        floorsPos = new Dictionary<int, bool>();
        posParents = new Dictionary<int, int>();
        spawnScript = FindObjectOfType<SpawnScript>();
        monsters = new List<GameObject>();
        towers = new List<GameObject>();
        highlightedPath = new List<GameObject>();
        exploredFloors = new List<int>();
        upgradesT1 = new List<Upgrade>();
        upgradesT2 = new List<Upgrade>();
        upgradesT3 = new List<Upgrade>();
        upgradesGunner = new List<Upgrade>();
        upgradesMiner = new List<Upgrade>();
        upgradesTrap = new List<Upgrade>();
        upgradesSlow = new List<Upgrade>();
    }
    private void AddUpgrades(){
        List<Upgrade> tmp = new List<Upgrade>();
        Upgrade powerUp = new PowerUp();
        tmp.Add(powerUp);
        Upgrade speedUp = new SpeedUp();
        tmp.Add(speedUp);
        Upgrade powerUp1 = new PowerUp1();
        tmp.Add(powerUp1);
        Upgrade speedUp1 = new SpeedUp1();
        tmp.Add(speedUp1);
        Upgrade expUp1 = new ExpUp1();
        tmp.Add(expUp1);
        Upgrade expUp2 = new ExpUp2();
        tmp.Add(expUp2);

        //Sort each tier
        foreach(Upgrade u in tmp){
            if(u.GetTag() == "Anyone"){
                if(u.GetRarity() == 1){
                    upgradesT1.Add(u);
                }else if(u.GetRarity() == 2){
                    upgradesT2.Add(u);
                }else if(u.GetRarity() == 3){
                    upgradesT3.Add(u);
                }
            }else if(u.GetTag() == "Gunner"){
                upgradesGunner.Add(u);
            }else if(u.GetTag() == "Miner"){
                upgradesMiner.Add(u);
            }else if(u.GetTag() == "Trap"){
                upgradesTrap.Add(u);
            }else if(u.GetTag() == "Slow"){
                upgradesSlow.Add(u);
            }
        }
    }
    public List<Upgrade> GetUpgradeT1(){
        return upgradesT1;
    }
    public List<Upgrade> GetUpgradeT2(){
        return upgradesT2;
    }
    public List<Upgrade> GetUpgradeT3(){
        return upgradesT3;
    }
    public List<Upgrade> GetUpgradeGunner(){
        return upgradesGunner;
    }
    public List<Upgrade> GetUpgradeMiner(){
        return upgradesMiner;
    }
    public List<Upgrade> GetUpgradeSlow(){
        return upgradesSlow;
    }
    public List<Upgrade> GetUpgradeTrap(){
        return upgradesTrap;
    }
    public int GetWave(){
        return wave;
    }
    public void SelectFloor(GameObject floor){
        selectedFloor = floor;
        state = 1;
        buildUI.SetActive(true);
    }
    public void SelectWall(GameObject wall){
        selectedWall = wall;
        state = 1;
        unitUI.SetActive(true);
    }
    public void SelectUnit(GameObject unit){
        gunnerInfo.SetActive(true);
        gunnerInfo.SetActive(false);
        minerInfo.SetActive(false);
        slowInfo.SetActive(false);
        trapInfo.SetActive(false);
        selectedUnit = unit;
        if(state == 0){
            state = 1;
        }
        unitInfo.SetActive(true);
        if(unit.tag == "Gunner"){
            gunnerInfo.SetActive(true);
            unit.GetComponent<TurretController>().SetInfo(unitInfo);
        }else if(unit.tag == "Miner"){
            minerInfo.SetActive(true);
            unit.GetComponent<MinerController>().SetInfo(unitInfo);
        }else if(unit.tag == "Slow"){
            slowInfo.SetActive(true);
            unit.GetComponent<TrapController>().SetInfo(unitInfo);
        }else if(unit.tag == "Trap"){
            trapInfo.SetActive(true);
            unit.GetComponent<TrapController>().SetInfo(unitInfo);
        }
    }
    public void BuildWall(GameObject building){
        if(wallMax == false){
            if(building == wallPrefabs){
                string tmp = Mathf.RoundToInt(selectedFloor.transform.position.x).ToString()+Mathf.RoundToInt(selectedFloor.transform.position.z).ToString();
                floorsPos[int.Parse(tmp)] = false;
                if(!FindShotestPath()){
                    selectedFloor.GetComponent<FloorScript>().ErrorPlace();
                    floorsPos[int.Parse(tmp)] = true;
                }else{
                    if(currency >= wallCost){
                        UseCurrency(wallCost);
                        Vector3 newPos = new Vector3(selectedFloor.transform.position.x, selectedFloor.transform.position.y+1f, selectedFloor.transform.position.z);
                        GameObject newObj = Instantiate(building, newPos, selectedFloor.transform.rotation);
                        wallDeploy++;
                        selectedFloor.GetComponent<FloorScript>().setHasWall(true);
                        wallDeployText.text = string.Format("{0}/{1}", wallDeploy.ToString(), maxWallDeploy.ToString());
                    }else{
                        floorsPos[int.Parse(tmp)] = true;
                        selectedFloor.GetComponent<FloorScript>().ErrorPlace();
                    }
                }
            }else{
                if(currency >= trapCost){
                    UseCurrency(trapCost);
                    Vector3 newPos = new Vector3(selectedFloor.transform.position.x, selectedFloor.transform.position.y+0.5f, selectedFloor.transform.position.z);
                    GameObject newObj = Instantiate(building, newPos, selectedFloor.transform.rotation);
                    towers.Add(newObj);
                    selectedFloor.GetComponent<FloorScript>().setHasWall(true);
                    wallDeploy++;
                    wallDeployText.text = string.Format("{0}/{1}", wallDeploy.ToString(), maxWallDeploy.ToString());
                }else{
                    selectedFloor.GetComponent<FloorScript>().ErrorPlace();
                }
            }
            buildUI.SetActive(false);
            FindShotestPath();
            state = 0;
            if(wallDeploy == maxWallDeploy){
                wallMax = true;
            }
        }
    }
    public void BuildUnit(GameObject building){
        Vector3 newPos = new Vector3(selectedWall.transform.position.x, selectedWall.transform.position.y+1, selectedWall.transform.position.z);
        int cost = 0;
        if(building == minerPrefabs){
            cost = minerCost;
        }else if(building == gunnerPrefabs){
            cost = gunnerCost;
        }else{
            return;
        }
        if(currency >= cost){
            UseCurrency(cost);
            GameObject tmp = Instantiate(building, newPos, selectedWall.transform.rotation);
            towers.Add(tmp);
            selectedWall.GetComponent<WallScript>().setHasUnit(1);
        }else{
            selectedWall.GetComponent<WallScript>().ErrorPlace();
        }
        unitUI.SetActive(false);
        state = 0;
    }
    public void AddMonster(GameObject monster){
        monsters.Add(monster);
        monsterLeftText.text = monsters.Count.ToString();
    }
    public void BuyLvUp(){
        int cost = 0;
        if(selectedUnit.tag == "Gunner"){
            cost = selectedUnit.GetComponent<TurretController>().GetExpLeft();
        }else if(selectedUnit.tag == "Miner"){
            cost = selectedUnit.GetComponent<MinerController>().GetExpLeft();
        }else if(selectedUnit.tag == "Slow"){
            cost = selectedUnit.GetComponent<TrapController>().GetExpLeft();
        }else if(selectedUnit.tag == "Trap"){
            cost = selectedUnit.GetComponent<TrapController>().GetExpLeft();
        }else{
            return;
        }
        if(cost == 0){
            return;
        }
        if(currency >= cost){
            UseCurrency(cost);
            if(selectedUnit.tag == "Gunner"){
                selectedUnit.GetComponent<TurretController>().BuyLv();
            }else if(selectedUnit.tag == "Miner"){
                selectedUnit.GetComponent<MinerController>().BuyLv();
            }else if(selectedUnit.tag == "Slow"){
                selectedUnit.GetComponent<TrapController>().BuyLv();
            }else if(selectedUnit.tag == "Trap"){
                selectedUnit.GetComponent<TrapController>().BuyLv();
            }
            CloseUI();
        }
    }
    public void UpgradeUnit(int number){
        if(selectedUnit.tag == "Gunner"){
            selectedUnit.GetComponent<TurretController>().UpgradeSkill(number);
        }else if(selectedUnit.tag == "Miner"){
            selectedUnit.GetComponent<MinerController>().UpgradeSkill(number);
        }else if(selectedUnit.tag == "Slow"){
            selectedUnit.GetComponent<TrapController>().UpgradeSkill(number);
        }else if(selectedUnit.tag == "Trap"){
            selectedUnit.GetComponent<TrapController>().UpgradeSkill(number);
        }else{
            return;
        }
        CloseUI();
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
        foreach(GameObject t in towers){
            if(t.tag == "Gunner"){
                t.GetComponent<TurretController>().GainExp(10);
            }else if(t.tag == "Miner"){
                t.GetComponent<MinerController>().GainExp(10);
            }else if(t.tag == "Slow"){
                t.GetComponent<TrapController>().GainExp(10);
            }else if(t.tag == "Trap"){
                t.GetComponent<TrapController>().GainExp(10);
            }
        }
        AddCurrency(monsterBounty);
        kill++;
        monsterLeftText.text = monsters.Count.ToString();
        Destroy(monster);
    }    
    public void Describe(int number){
        string desc = "";
        if(selectedUnit.tag == "Gunner"){
            desc = selectedUnit.GetComponent<TurretController>().GetDescription(number);
        }else if(selectedUnit.tag == "Miner"){
            desc = selectedUnit.GetComponent<MinerController>().GetDescription(number);
        }else if(selectedUnit.tag == "Slow"){
            desc = selectedUnit.GetComponent<TrapController>().GetDescription(number);
        }else if(selectedUnit.tag == "Trap"){
            desc = selectedUnit.GetComponent<TrapController>().GetDescription(number);
        }
        if(desc == ""){
        }else{
            descriptionText.text = desc;
            description.SetActive(true);
        }
    }
    public List<Upgrade> Get3RandomUpgrade(GameObject tower){
        List<Upgrade> ups = new List<Upgrade>();
        int z = 0;
        while(z<3){
            int luckyNum = Random.Range(11, 100);//TODO
            Upgrade tmp = null;
            if(luckyNum > 50){
                tmp = upgradesT3[Random.Range(0,upgradesT3.Count)];
            }else if(luckyNum > 25){
                tmp = upgradesT2[Random.Range(0,upgradesT2.Count)];
            }else if(luckyNum > 10){
                tmp = upgradesT1[Random.Range(0,upgradesT1.Count)];
            }else{
                if(tower.tag == "Gunner"){
                    tmp = upgradesGunner[Random.Range(0,upgradesGunner.Count)];
                }else if(tower.tag == "Miner"){
                   tmp = upgradesMiner[Random.Range(0,upgradesMiner.Count)];
                }else if(tower.tag == "Slow"){
                    tmp = upgradesSlow[Random.Range(0,upgradesSlow.Count)];
                }else if(tower.tag == "Trap"){
                    tmp = upgradesTrap[Random.Range(0,upgradesTrap.Count)];
                }
            }
            ups.Add(tmp);
            z++;
        }
        return ups;
    }
    public GameObject GetUpgradePath(){
        return upgradePath;
    }
    public void CloseUI(){
        buildUI.SetActive(false);
        unitUI.SetActive(false);
        if(selectedFloor != null){
            selectedFloor.GetComponent<FloorScript>().SetSelect(false);
        }
        if(selectedWall != null){
            selectedWall.GetComponent<WallScript>().SetSelect(false);
        }
        unitInfo.SetActive(false);
        gunnerInfo.SetActive(false);
        minerInfo.SetActive(false);
        slowInfo.SetActive(false);
        trapInfo.SetActive(false);
        description.SetActive(false);
        upgradePath.SetActive(false);
        if(state == 1){
            state = 0;
        }
    }
    public void SetState(int state){
        this.state = state;
    }
    public int GetState(){
        return state;
    }
    public void SetMonsterBounty(int monsterBounty){
        this.monsterBounty = monsterBounty;
    }
    public bool GetWallMax(){
        return wallMax;
    }
    public GameObject GetEnd(){
        return end;
    }
    public void AddCurrency(int curr){
        this.currency += curr;
        this.currencyText.text = currency.ToString();
    }
    public void UseCurrency(int curr){
        this.currency -= curr;
        this.currencyText.text = currency.ToString();
    }
    public void Spawn(){
        spawnScript.Spawn(wave);
    }
    public void FinishWave(){
        wave += 1;
        waveText.text = wave.ToString();
        Day();
    }
    public void Day(){
        foreach(GameObject i in floors){
            i.GetComponent<FloorScript>().DeleteObstruct();
        }
        state = 0;
        phase.sprite = sun;
        dawning = true;
        Invoke("StopTransition",duration);
        skip.SetActive(true);
    }
    public void Night(){
        foreach(GameObject i in floors){
            i.GetComponent<FloorScript>().PlaceObstruct();
        }
        state = 2;
        phase.sprite = moon;
        dusking = true;
        Invoke("StopTransition",duration);
        skip.SetActive(false);
        Spawn();
    }
    public void GameOver(){
        GetHighScore();
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
        path.Add(40);
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
    public void GetHighScore(){
        if(PlayerPrefs.HasKey("BestKill")){
            if(PlayerPrefs.GetInt("BestKill") < kill){
                PlayerPrefs.SetInt("BestKill",kill);
            }
        }else{
            PlayerPrefs.SetInt("BestKill",kill);
        }
        if(PlayerPrefs.HasKey("BestWave")){
            if(PlayerPrefs.GetInt("BestWave") < wave){
                PlayerPrefs.SetInt("BestWave",wave);
            }
        }else{
            PlayerPrefs.SetInt("BestWave",wave);
        }
        highestKillText.text = string.Format("Highest kills: {0}", PlayerPrefs.GetInt("BestKill"));
        currentKillText.text = string.Format("Current kills: {0}", kill);
        highestWaveText.text = string.Format("Highest Waves: {0}", PlayerPrefs.GetInt("BestWave"));
        currentWaveText.text = string.Format("Current Waves: {0}", wave);
    }  
    public void StopTransition(){
        dusking = false;
        dawning = false;
        t = 0;
    }
}
