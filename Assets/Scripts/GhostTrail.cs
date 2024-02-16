 using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.Serialization;
using System.Linq;
using Unity.Mathematics;
using static GameManager;
//[ExecuteInEditMode]
//[]
public class GhostTrail : MonoBehaviour
{
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
    private bool[] didParticule = new bool[3];
    private GameObject[] GO_Clone;
        // peut faire un dictionnaire
    // je vais les mettre dans un scriptable object et copier le contenue a chaque mort  ici 
    public int maxNbrOfClones= 3;
    [HideInInspector]public int currentNbrClones=0;
    public ParticleSystem _particleSystem;

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
        
        if (canSpawn && currentNbrClones >0)
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
        
        CloneSpawnNbr();
        canSpawn = true;
        Debug.Log("startSPAWN");
    }
    

    private IEnumerator RegisteringClone(float TimeSpawn)
    {
        if (currentNbrClones == maxNbrOfClones)
        {
            yield break;
        }
        Debug.Log("Registering");
        Debug.Log($"ListParentPos2D[{{currentNbrClones}}] et {{ListParentPos2D[currentNbrClones].Count}}");
            ListParentPos2D[currentNbrClones].Add(transform.position);

            yield return null;
            
        /*if (!canSpawn && !blockWorking)
        {
            Debug.Log("CONTINUE");
            canRegister = true;
        }*/
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
            for (int i = 0; i < currentNbrClones; i++)
            {
               // Debug.Log($"non null list  ={nonNullList}");
                var PosSpawn = ListParentPos2D[0][0];
                GO_Clone[i] = Instantiate(Clone, new Vector3(PosSpawn.x, PosSpawn.y, transform.position.z),
                    Quaternion.identity);
            }
        }
        int allNullList = 0;
        
        for (int i = 0; i < currentNbrClones; i++)
        {
            if (ListParentPos2D[i] != null && currentSpawnIndex < ListParentPos2D[i].Count) //&& ListParentPos2D[i][currentSpawnIndex] != null)
            {
                //Debug.Log($"Index out of range: currentSpawnIndex ({currentSpawnIndex}) >= ListParentPos2D[{i}].Count ({ListParentPos2D[i].Count})");
                StartCoroutine(MoveClone(ListParentPos2D[i], currentSpawnIndex,cloneSpawnTimer,i ));
            }
            else
            {
                if (didParticule[i] == false)
                {
                    didParticule[i] = true;
                    var cloneDeath = GO_Clone[i];
                    var ParticuleSpawned =
                        Instantiate(_particleSystem, cloneDeath.transform.position, quaternion.identity);
                    cloneDeath.GetComponent<Bomba>().Explode();
                    ParticuleSpawned.Play();
                    cloneDeath.SetActive(false);
                    allNullList++;
                }
            }
            
        }
        if(allNullList == currentNbrClones)
        {
            
            canSpawn = false;
            currentSpawnIndex = 0;
            return;
        }

        currentSpawnIndex++;
    }
    public void ChangeCurrentState()
    {
        
        if (currentNbrClones==maxNbrOfClones)
        {
            GameManager.Instance.UpdateGameState(GameState.Lose);
            LevelManager.RestartLevel();
        }
        else
        {
            for (int j = 0; j < didParticule.Length; j++)
            {
                didParticule[j] = false;
            }
            currentSpawnIndex = 0;
            //currentPosRegister = 0;
            currentNbrClones+=1;
            hasSpawnClones = false;
            

        }
    }
    
} 