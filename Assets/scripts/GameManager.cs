using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int maxShipCount = 20;
    public Transform spawns;
    public GameObject enemyPrefab;
    public GameObject bomberPrefab;
    public Transform checkpoints;
    public Transform homes;

    //GameObject[] ships;
    Dictionary<int, GameObject> ships;
    int currentShips;
    int shipsAlive;
    int shipsIdle;
    float cooldown = .25f;
    float timer;
    bool ready;

    // Start is called before the first frame update
    void Start()
    {
      currentShips = 0;
      shipsAlive = 0;
      ships = new Dictionary<int, GameObject>();
      timer = 5;
      ready = false;
      shipsIdle = 0;
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
        if(shipsAlive > 0 && shipsIdle == maxShipCount)
        {
          AttackRun();
        }
    }

    public void ShipDestroyed(int id, int mode){
      if(mode < 2)
        shipsIdle ++;
      shipsAlive --;
      ships.Remove(id);
      Debug.Log("ship destroyed, left: "+shipsAlive);

      if(ships.Count == 0){
        timer = 5f;
        currentShips = 0;
        maxShipCount += 4;
        ships = new Dictionary<int, GameObject>();
        ready = false;
        shipsIdle = 0;
      }
      //ships[shipNumber] = null;
    }

    Vector2 getShipHome(){
      int perRow = maxShipCount/4;
      int shipRow = 3-currentShips / perRow;
      int shipPos = (currentShips%perRow);
      int homePadding = (10 - perRow)/2;
      //Debug.Log("this ship is in "+shipRow+" "+shipPos+homePadding);
      return homes.GetChild(shipRow).GetChild(shipPos+homePadding).position;
    }

    void sendShip(){
      Vector2 sendShips = spawns.GetChild(0).position;
      GameObject enemyObject = Instantiate(enemyPrefab, sendShips, Quaternion.identity);
      enemyController enemy = enemyObject.GetComponent<enemyController>();
      enemy.Launch(checkpoints, getShipHome(), this, currentShips);
      ships.Add(currentShips, enemyObject);
      currentShips++;
      shipsAlive++;
      ready = false;
    }

    int getRandomAlive(){
      return 0;
    }

    void AttackRun(){}

}
