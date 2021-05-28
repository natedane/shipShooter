using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int maxShipCount = 16;
    public int[] cShipCount = {0,0,0};
    public int[] bShipCount = {0,0,0};
    public Transform spawns;
    public GameObject enemyPrefab;
    public GameObject bomberPrefab;
    public GameObject carrierPrefab;
    public Transform checkpoints;
    public Transform homes;

    public static GameManager instance { get; private set; }

    //private readonly Random _random = new Random();
    //GameObject[] ships;
    Dictionary<int, enemyController> ships;
    List<int>shipArray;
    int currentShips;
    int shipsAlive;
    int shipsIdle;
    float cooldown = .1f;
    float timer;
    bool ready;
    public int route;

    // Start is called before the first frame update
    void Start()
    {
      instance = this;
      Setup(maxShipCount, route,bShipCount[0],cShipCount[0]);
    }

    void Setup(int c, int r, int b, int carr){
      timer = 5f;
      currentShips = 0;
      maxShipCount = c;
      ships = new Dictionary<int, enemyController>();
      ready = false;
      shipsIdle = 0;
      route =r;
      setList();
      bShipCount = new int[]{b, 0, 0};
      cShipCount = new int[]{carr, 0, 0};
    }
    // Update is called once per frame
    void Update()
    {

        if(timer < 0){
          ready = true;
          timer = cooldown;
        }else{
          timer -= Time.deltaTime;
        }

        if(currentShips < maxShipCount && ready == true){
          sendShip();
        }
        else if(ready == true && bShipCount[1] < bShipCount[0]){
          sendBShip();
        }
        else if(ready == true && cShipCount[1] < cShipCount[0]){
          sendCShip();
        }
        else if(shipsAlive > 0 && shipsIdle > maxShipCount-2)
        {
          //Debug.Log("ships idle are: "+shipsIdle+" "+maxShipCount);
          AttackRun(maxShipCount - shipsIdle);
        }
    }

    public void ShipDestroyed(int id, int mode){
      if(mode < 2)
        shipsIdle ++;
      if(mode == 3)
        bShipCount[2]--;
      if(mode == 4)
        cShipCount[2]--;
      shipsAlive --;
      ships.Remove(id);
      shipArray.Remove(id);
      //Debug.Log("ship destroyed, left: "+shipsAlive+" "+shipsIdle);

      if(shipsAlive == 0 && bShipCount[2]<=0 && cShipCount[2]<=0){
        if(route <4)
          Setup(maxShipCount+8, route+1,0,0);
        else if(maxShipCount < 36 && route == 4)
          Setup(maxShipCount+8, 4,0,0);
        else if(maxShipCount <= 40 && route == 4)
          Setup(18, 5,1,0);
        else if(bShipCount[0]==3)
          Setup(18, 5,0,cShipCount[0]+1);
        else
          Setup(maxShipCount+3, 5,bShipCount[0]+1,0);

      }
      //ships[shipNumber] = null;
    }

    Vector2 getShipHome(int type){
      int shipRow;
      int shipPos;
      if(type ==0){
        int rows = 4;
        if(route == 5)
          rows = 3;
        int perRow = maxShipCount/rows;
        shipRow = currentShips / perRow;
        shipPos = (currentShips%perRow);
        int homePadding = (10 - perRow)/2;
        shipPos += homePadding;
     }
     else{
      shipRow = 3;
      shipPos = bShipCount[1] + (9 - bShipCount[0])/2;
      //Debug.Log("this ship is in "+shipRow+" "+shipPos+" "+type);

     }
      return homes.GetChild(shipRow).GetChild(shipPos).position;
    }

    void sendShip(){
      //Debug.Log("ship created, left: "+currentShips);
      bool arm = false;
      if(currentShips%6 == 0)
        arm = true;

      int r = getRouteSpawn(currentShips);
      Vector2 sendShips = spawns.GetChild(r).position;
      GameObject enemyObject = Instantiate(enemyPrefab, sendShips, Quaternion.identity);
      enemyController enemy = enemyObject.GetComponent<enemyController>();
      enemy.Launch(checkpoints.GetChild(r), getShipHome(0), currentShips, arm);
      ships.Add(currentShips, enemy);
      currentShips++;
      shipsAlive++;
      ready = false;
    }
    void sendBShip(){
      //Debug.Log("ship created, left: "+currentShips);
      Vector2 sendShips = spawns.GetChild(3).position;
      GameObject enemyObject = Instantiate(bomberPrefab, sendShips, Quaternion.identity);
      SentryEnemy benemy = enemyObject.GetComponent<SentryEnemy>();

      benemy.Launch(checkpoints.GetChild(4), getShipHome(1), this, bShipCount[1], true);
      bShipCount[1]++;
      bShipCount[2]++;
      ready = false;
    }
    void sendCShip(){
      Vector2 sendShips = spawns.GetChild(3).position;
      GameObject enemyObject = Instantiate(carrierPrefab, sendShips, Quaternion.identity);
      carrierEnemy cenemy = enemyObject.GetComponent<carrierEnemy>();

      cenemy.Launch(checkpoints.GetChild(4), getShipHome(1), this, cShipCount[1], true);

      cShipCount[1]++;
      cShipCount[2]++;
      ready = false;
    }

    int getRandomAlive(){
      return 0;
    }

    void AttackRun(int count){
      int perRow = maxShipCount/4;

      List<int> temp = new List<int>(shipArray);
      //Debug.Log("attack running, left: "+shipArray+" "+temp);
      for(int i = 0; i < count; i++){
        int e1 = temp[Random.Range(0, temp.Count)];
        temp.Remove(e1);
        //temp.Sort();
        //enemyController enemy = Random.value(ships);
        //Debug.Log("random is " +e1);
        enemyController enemy = ships[e1];
        if(enemy == null || enemy.getMode() != 2)
          return;
        //enemyController enemy2 = ships.Rand;
        if(e1 %perRow < perRow/2)
          enemy.Attack(checkpoints.GetChild(2));
        else
         enemy.Attack(checkpoints.GetChild(3));
        //enemy.Attack(checkpoints.GetChild(3));
        this.shipsIdle--;
      }
    }

    int getRouteSpawn(int id){
      if(route == 0){
        return route;
      }
      if(route == 1){
        return route;
      }
      if(route == 2){
        if((id %2) == 0)
          return route;
        else
          return route+1;
      }
      if(route == 4){
        if((id %2) == 0)
          return 0;
        else
          return 1;
      }
      if(route == 5){
        if((id %2) == 0)
          return 2;
        else
          return 3;
      }
      else{
        int r = id%4;
        return r;
      }
    }

    public void addIdle(int id){
      //Debug.Log("idle is " +id);
      shipsIdle ++;
    }

    void setList(){
      shipArray = new List<int>();
      for(int i =0; i < maxShipCount; i++){
        shipArray.Add(i);
      }
    }

    public void PlayerDestroyed(){

    }

}
