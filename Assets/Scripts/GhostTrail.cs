using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.Serialization;
using System.Linq;
using static GameManager;
//[ExecuteInEditMode]
//[]
public class GhostTrail : MonoBehaviour
{
    public VectorListData vectorListData;
    public float cloneRegisterTime;
    public float cloneSpawnTimer;
    public  GameObject Clone;
    /*[HideInInspector]*/ public bool canSpawn= false;
    /*[HideInInspector]*/public bool canRegister= false;
    private bool startSpawn;
    /*[HideInInspector]*/public bool blockWorking=false;
    private int currentSpawnIndex = 0;
    private int currentPosSpawn = 0;
    private int currentPosRegister = 0;
    private int nonNullList=0;
    private bool hasSpawnClones=false;
    //public List<List<GameObject> WeaponSlots = new List<GameObject>(4);
    public List<Vector2>[] ListParentPos2D;
   
    private GameObject[] GO_Clone;
        // peut faire un dictionnaire
    // je vais les mettre dans un scriptable object et copier le contenue a chaque mort  ici 
    public int maxNbrOfClones= 3;
    [HideInInspector]public int currentNbrClones=0;

    private enum GhostState
    {
        clone1,
        clone2,
        clone3,
        Dead
        
    }
    private void Awake()
    {
        GO_Clone = new GameObject[maxNbrOfClones];
        ListParentPos2D = new List<Vector2>[maxNbrOfClones];
        for (int i = 0; i < maxNbrOfClones; i++)
        {
            ListParentPos2D[i] = new List<Vector2>();
            
        }
        
        
    }

    private void Start()
    {
        //vectorListData.vectorList.Clear();
        //CloneSpawnNbr();
    }

    public void FixedUpdate()
    {
        
        
    }

    private void Update()
    {
        if (canSpawn && !canRegister)
        {
            if (blockWorking)
            {
                Debug.Log("ENDSpawn");
                canSpawn = false;
                
                return;
            }

            SpawnCloneManager();
        }
        
    }

    private void LateUpdate()
    {
        Debug.Log($"NOMBRE DE CLONE = {currentNbrClones}");
        if (!canSpawn && canRegister && !blockWorking)
        {
            
            StartCoroutine(RegisteringClone(cloneRegisterTime));
        }

        
    }
    public void StartRegister()
    {
        canRegister = true;
        blockWorking = false;
        canSpawn = false;
        Debug.Log("startREGISTER");
    }
    public void StartSpawning()
    {
        canRegister = false;
        CloneSpawnNbr();
        canSpawn = true;
        Debug.Log("startSPAWN");
    }
    

    private IEnumerator RegisteringClone(float TimeSpawn)
    {
        canRegister = false;
        Debug.Log("Registering");
        
            ListParentPos2D[currentNbrClones].Add(transform.position);
            if (ListParentPos2D[currentNbrClones].Count > 0)
            {
                Debug.Log($"au dessus = {ListParentPos2D[currentNbrClones]}"); 
            }
            else
            {
                Debug.Log($"en dessous = {ListParentPos2D[currentNbrClones]}"); 
            }

            yield return null;
        
        if (!canSpawn && !blockWorking)
        {
            canRegister = true;
        }
    }

    private IEnumerator MoveClone(List<Vector2> SubPos2D,int currentSpawnIndexIE,float Spawntime,int GO_enemy)
    {
        GO_Clone[GO_enemy].transform.position =  SubPos2D[currentSpawnIndexIE];
        yield return null;
        //yield return new WaitForSeconds(Spawntime);
        //yield return new WaitForSeconds(cloneSpawnTimer*Time.fixedDeltaTime);

    }

    public void CloneSpawnNbr()
    {
        currentSpawnIndex = 0;
        nonNullList = 0;
        foreach (var listChildPos in ListParentPos2D)
        {
            if (listChildPos.Count > 0 )
            {
               
                nonNullList++;
            }
           
        }
        Debug.Log($"liste de non null = {nonNullList}");
        
    }

    private void SpawnCloneManager()
    {
        if (!hasSpawnClones)
        {
            
            hasSpawnClones = true;
            for (int i = 0; i < nonNullList; i++)
            {
                Debug.Log($"nombre de clo,e ={currentNbrClones}");
                var PosSpawn = ListParentPos2D[0][0];
                GO_Clone[i] = Instantiate(Clone, new Vector3(PosSpawn.x, PosSpawn.y, transform.position.z),
                    Quaternion.identity);
            }
        }
        int allNullList = 0;
        
        for (int i = 0; i < nonNullList; i++)
        {
            if (ListParentPos2D[i] != null && currentSpawnIndex < ListParentPos2D[i].Count && ListParentPos2D[i][currentSpawnIndex] != null)
            {
                //Debug.Log($"Index out of range: currentSpawnIndex ({currentSpawnIndex}) >= ListParentPos2D[{i}].Count ({ListParentPos2D[i].Count})");
                StartCoroutine(MoveClone(ListParentPos2D[i], currentSpawnIndex,cloneSpawnTimer,i ));
            }
            else
            {
                allNullList++;
            }
            
        }
        if(allNullList == nonNullList)
        {
            Debug.Log("dqsdsfdsfgdsfdsfdsffdsffd");
            //explosion
            canSpawn = false;
            currentSpawnIndex = 0;
        }

        currentSpawnIndex++;
    }
    public void ChangeCurrentState()
    {
        
        if (currentNbrClones-2==maxNbrOfClones)
        {
            GameManager.Instance.UpdateGameState(GameState.Lose);
            Debug.Log("dead");
        }
        else
        {
            currentSpawnIndex = 0;
            //currentPosRegister = 0;
            currentNbrClones+=1;
            hasSpawnClones = false;

        }
    }
    
}