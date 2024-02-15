using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.Serialization;
using System.Linq;
//[ExecuteInEditMode]
public class GhostTrail : MonoBehaviour
{
    public VectorListData vectorListData;
    public float cloneRegisterTime;
    public float cloneSpawnTimer;
    public  GameObject Clone;
    [SerializeField]private bool canSpawn { set; get; } = false;
    [SerializeField]private bool canRegister{ set; get; } = false;
    private bool hasRegister,changeState,isExploding=false;
    private bool startSpawn;
    private bool blockWorking=false;
    private int currentSpawnIndex = 0;
    private int currentPosSpawn = 0;
    private int currentPosRegister = 0;
    private int nonNullList=0;
    private bool hasSpawnClones=false;
    [SerializeField] public List<Vector2>[] ListParentPos2D;
    private GameObject[] GO_Clone;
        // peut faire un dictionnaire
    // je vais les mettre dans un scriptable object et copier le contenue a chaque mort  ici 
    public int maxNbrOfClones= 3;
    private int currentNbrClones=0;

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
        vectorListData.vectorList.Clear();
        CloneSpawnNbr();
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
        if ( Input.GetKeyDown(KeyCode.K))
        {
            vectorListData.vectorList = ListParentPos2D[0];
            vectorListData.ConvertListToArray();
            
        } 
    }

    private void LateUpdate()
    {
        if ( Input.GetKeyDown(KeyCode.Q))
        {
            canRegister = false;
            canSpawn = true;
            Debug.Log("SecondStart");
        }
        if ((Input.GetKeyDown(KeyCode.E)))
        {
            canRegister = true;
            canSpawn = false;
            Debug.Log("start");
            
        }
        if ( Input.GetKeyDown(KeyCode.F))
        {
            ChangeCurrentState();
            Debug.Log("State++");
        }
        
        if (!canSpawn && canRegister && !blockWorking)
        {
            StartCoroutine(RegisteringClone(cloneRegisterTime));
        }

        
    }


    private IEnumerator RegisteringClone(float TimeSpawn)
    {
        canRegister = false;
        Debug.Log("Registering");
        if (currentPosRegister != 0 && currentPosRegister > 0 && currentPosRegister <= ListParentPos2D[currentNbrClones].Count )
        {
            var check2dBefore = ListParentPos2D[currentNbrClones][currentPosRegister-1];
            
            var playerPos2D = new Vector2(transform.position.x, transform.position.y);
           
            if (check2dBefore != playerPos2D)
            {
                
                ListParentPos2D[currentNbrClones].Add(transform.position);
            }
        }
        else
        {
            ListParentPos2D[currentNbrClones].Add(transform.position);
        }

        yield return null;
        //yield return new WaitForSeconds(TimeSpawn);
        currentPosRegister++;
        if (!canSpawn)
        {
            canRegister = true;
        }
    }

    private IEnumerator SpawnClone(List<Vector2> SubPos2D,int currentSpawnIndexIE,float Spawntime,int GO_enemy)
    {
        GO_Clone[GO_enemy].transform.position =  SubPos2D[currentSpawnIndexIE];
        yield return null;
        //yield return new WaitForSeconds(Spawntime);
        //yield return new WaitForSeconds(cloneSpawnTimer*Time.fixedDeltaTime);

    }

    private void CloneSpawnNbr()
    {
        currentSpawnIndex = 0;
        nonNullList = 0;
        foreach (var listChildPos in ListParentPos2D)
        {
            if (listChildPos != null || listChildPos.Count > 0)
            {
                nonNullList++;
            }
            
        }
        
    }

    private void SpawnCloneManager()
    {
        if (!hasSpawnClones)
        {
            hasSpawnClones = true;
            for (int i = 0; i < nonNullList; i++)
            {
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
                StartCoroutine(SpawnClone(ListParentPos2D[i], currentSpawnIndex,cloneSpawnTimer,i ));
            }
            else
            {
                allNullList++;
            }
            
        }
        if(allNullList == nonNullList)
        {
            blockWorking = true;
            canSpawn = false;
            currentSpawnIndex = 0;
        }

        currentSpawnIndex++;
    }
    public void ChangeCurrentState()
    {
        
        if (currentNbrClones+1==maxNbrOfClones)
        {
            Debug.Log("dead");
        }
        else
        {
            currentSpawnIndex = 0;
            currentPosRegister = 0;
            currentNbrClones+=1;
            blockWorking = false;
            canRegister = true;
            

        }
    }
    
}

    
