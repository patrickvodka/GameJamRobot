using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.Serialization;
//[ExecuteInEditMode]
public class GhostTrail : MonoBehaviour
{
    public float cloneRegisterTime;
    public float cloneSpawnTimer;
    public  GameObject Clone;
    private GameObject ghostStacks;
    public bool canSpawn=false;
    public bool hasSpawn=false;
    [SerializeField]private bool canRegister=false;
    private bool hasRegister,changeState,isExploding=false;
    private bool startSpawn;
    private bool blockWorking=false;
    private int currentSpawnIndex = 0;
    private int currentPosSpawn = 0;
    private int currentPosRegister = 0;
    private int nonNullList=0;
    [SerializeField] public List<Vector2>[] ListParentPos2D;
    public List<GameObject>[] GO_Clone;
        // peut faire un dictionnaire
    // je vais les mettre dans un scriptable object et copier le contenue a chaque mort  ici 
    public int maxNbrOfClones { get; set; } = 3;
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
        ListParentPos2D = new List<Vector2>[maxNbrOfClones];
        for (int i = 0; i < maxNbrOfClones; i++)
        {
            Debug.Log($"création de sous {ListParentPos2D[i]}");
            ListParentPos2D[i] = new List<Vector2>();
        }
        Debug.Log($"main liste {ListParentPos2D}");
       // GO_Clone = new List<GameObject>[maxNbrOfClones];
        //Clone = gameObject;
    }

    private void Start()
    {
        
        CloneSpawnNbr();
    }

    public void FixedUpdate()
    {
        
        if (canSpawn && !canRegister)
        {
            if (blockWorking)
            {
                Debug.Log("END");
                canSpawn = false;
                
                return;
            }

            SpawnCloneManager();
        }
    }

    private void Update()
    {
    }

    private void LateUpdate()
    {
        if ( Input.GetKeyDown(KeyCode.A))
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
        Debug.Log("test");
        if (currentPosRegister != 0 && currentPosRegister > 0 && currentPosRegister <= ListParentPos2D[currentNbrClones].Count )
        {
            var check2dBefore = ListParentPos2D[currentNbrClones][currentPosRegister-1];
            
            var playerPos2D = new Vector2(transform.position.x, transform.position.y);
            //Debug.Log($"player = {playerPos2D} et le check = {check2d}");
            if (check2dBefore != playerPos2D)
            {
                
                ListParentPos2D[currentNbrClones].Add(transform.position);
            }
        }
        else
        {
            ListParentPos2D[currentNbrClones].Add(transform.position);
        }
        yield return new WaitForSeconds(TimeSpawn);
        currentPosRegister++;
        if (!canSpawn)
        {
            canRegister = true;
        }
    }

    private IEnumerator SpawnClone(List<Vector2> SubPos2D,int currentSpawnIndexIE)
    {
        transform.position = SubPos2D[currentSpawnIndexIE];

        yield return null;
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
        int allNullList = 0;
        
        for (int i = 0; i < nonNullList; i++)
        {
            if (ListParentPos2D[i] != null && currentSpawnIndex < ListParentPos2D[i].Count && ListParentPos2D[i][currentSpawnIndex] != null)
            {
                Debug.Log($"Index out of range: currentSpawnIndex ({currentSpawnIndex}) >= ListParentPos2D[{i}].Count ({ListParentPos2D[i].Count})");
                StartCoroutine(SpawnClone(ListParentPos2D[i], currentSpawnIndex ));
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

    
