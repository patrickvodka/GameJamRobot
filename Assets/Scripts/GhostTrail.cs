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
    private bool hasRegister,changeState=false;
    private bool startSpawn;
    private bool blockWorking=false;
    private int currentStateNbr = 0;
    private int currentPosSpawn = 0;
    private int currentPosRegister = 0;
    public List<Vector2> Pos2D_1;
    public List<Vector2> Pos2D_2;
    public List<Vector2> Pos2D_3;
    // je vais les mettre dans un scriptable object et copier le contenue a chaque mort  ici 
    private int maxNbrOfClones { get; set; }
    private int currentNbrClones;

    private enum GhostState
    {
        clone1,
        clone2,
        clone3,
        Dead
        
    }
    private void Awake()
    {
        Pos2D_1 = new List<Vector2>();
        Pos2D_2 = new List<Vector2>();
        Pos2D_3 = new List<Vector2>();
        Clone = gameObject;
    }

    private void Start()
    {
        maxNbrOfClones = 3;
        currentNbrClones = 1;
    }

    public void FixedUpdate()
    {
        
        if (canSpawn && !canRegister)
        {
            if (currentPosSpawn  == Pos2D_1.Count && currentPosSpawn!=0)
            {
                Debug.Log("END");
                canSpawn = false;
                return;
            }

            StartCoroutine(SpawnClone());
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
        
        if (!canSpawn && canRegister)
        {
            StartCoroutine(RegisteringClone(cloneRegisterTime));
        }

        
    }

    private IEnumerator RegisteringClone(float TimeSpawn)
    {
        canRegister = false;
        if (currentPosRegister != 0)
        {
            var check2d = Pos2D_1[Pos2D_1.Count - 1];
            var playerPos2D = new Vector2(transform.position.x, transform.position.y);
            //Debug.Log($"player = {playerPos2D} et le check = {check2d}");
            if (check2d != playerPos2D)
            {
                
                Pos2D_1.Add(transform.position);
            }
            else
            {
                
            }
        }
        else
        {
            Pos2D_1.Add(transform.position);
        }
        yield return new WaitForSeconds(TimeSpawn);
        currentPosRegister++;
        if (!canSpawn)
        {
            canRegister = true;
        }
    }

    private IEnumerator SpawnClone()
    {
        canSpawn = false;
        if (currentPosSpawn == 0)
        {
           
            transform.position = Pos2D_1[0];
        }
        transform.position = Pos2D_1[currentPosSpawn];
        yield return new WaitForSeconds(0);
        currentPosSpawn++;
        canSpawn = true;

    }


    public void ChangeCurrentState(int maxClone=3)
    {
        
        if (currentNbrClones==maxClone+1)
        {
            Debug.Log("dead");
        }
        else
        {
            currentNbrClones+=1;
            
        }
    }
    
}

    
