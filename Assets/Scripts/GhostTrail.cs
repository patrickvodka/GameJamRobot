using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Serialization;

public class GhostTrail : MonoBehaviour
{
    public float cloneRegisterTime;
    public float cloneSpawnTimer;
    private GameObject ghostClone;
    private GameObject ghostStacks;
    public bool canSpawn=false;
    public bool hasSpawn=false;
    [SerializeField]private bool canRegister=false;
    private bool hasRegister=false;
    private bool startSpawn;
    private int currentPosSpawn = 0;
    private int currentPosRegister = 0;
    public List<Vector2> Pos2D_1;
    public List<Vector2> Pos2D_2;
    public List<Vector2> Pos2D_3;
    // je vais les mettre dans un scriptable object et copier le contenue a chaque mort  ici 
    private GhostState currentState;

    private enum GhostState
    {
        idle,
        Register,
        Spawning
        
    }
    private void Awake()
    {
        Pos2D_1 = new List<Vector2>();
        ghostStacks = gameObject;
        ghostClone =ghostStacks;
    }

    private void Start()
    {
        
    }

    public void FixedUpdate()
    {
        
    }

    private void Update()
    {
        if ( Input.GetKeyDown(KeyCode.A))
        {
            canRegister = false;
            canSpawn = true;
        }
        if ((Input.GetKeyDown(KeyCode.E)))
        {
            canRegister = true;
            canSpawn = false;

            Debug.Log($"{canSpawn}");
        }
        
        if (!canSpawn && canRegister)
        {
            StartCoroutine(RegisteringClone(cloneRegisterTime));
        }

        if (canSpawn && !canRegister)
        {
            if (currentPosSpawn  == Pos2D_1.Count && currentPosSpawn!=0)
            {
                Debug.Log("END");
                return;
            }

            StartCoroutine(SpawnClone(cloneSpawnTimer));
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
                Debug.Log("diff");
                Pos2D_1.Add(transform.position);
            }
            else
            {
                Debug.Log("=");
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

    private IEnumerator SpawnClone(float TimeSpawn)
    {
        canSpawn = false;
        if (currentPosSpawn == 0)
        {
            transform.position = Pos2D_1[0];
        }
        transform.position = Pos2D_1[currentPosSpawn];
        yield return new WaitForSeconds(TimeSpawn);
        currentPosSpawn++;
        canSpawn = true;

    }


    private void MultiSpawnClone()
    {
        
    }
}

    
