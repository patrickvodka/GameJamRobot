using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.Serialization;
using System.Linq;
using static GameManager;

public class GhostTrail : MonoBehaviour
{
    public VectorListData vectorListData;
    public float cloneRegisterTime;
    public float cloneSpawnTimer;
    public GameObject Clone;
    public bool canSpawn = false;
    public bool canRegister = false;
    public bool blockWorking = false;
    public int maxNbrOfClones = 3;
    [HideInInspector] public int currentNbrClones = 0;

    private List<Vector2>[] ListParentPos2D;
    private GameObject[] GO_Clone;
    private int currentSpawnIndex = 0;
    private int nonNullList = 0;
    private bool hasSpawnClones = false;

    private void Awake()
    {
        GO_Clone = new GameObject[maxNbrOfClones];
        ListParentPos2D = new List<Vector2>[maxNbrOfClones];
        for (int i = 0; i < maxNbrOfClones; i++)
        {
            ListParentPos2D[i] = new List<Vector2>();
        }
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

    private void SpawnCloneManager()
    {
        if (!hasSpawnClones)
        {
            hasSpawnClones = true;
            for (int i = 0; i < nonNullList; i++)
            {
                var PosSpawn = ListParentPos2D[0][0];
                GO_Clone[i] = Instantiate(Clone, new Vector3(PosSpawn.x, PosSpawn.y, transform.position.z), Quaternion.identity);
            }
        }

        int allNullList = 0;
        for (int i = 0; i < nonNullList; i++)
        {
            if (ListParentPos2D[i] != null && currentSpawnIndex < ListParentPos2D[i].Count && ListParentPos2D[i][currentSpawnIndex] != null)
            {
                StartCoroutine(MoveClone(ListParentPos2D[i], currentSpawnIndex, cloneSpawnTimer, i));
            }
            else
            {
                allNullList++;
            }
        }

        if (allNullList == nonNullList)
        {
            Debug.Log("dqsdsfdsfgdsfdsfdsffdsffd");
            canSpawn = false;
            currentSpawnIndex = 0;
        }

        currentSpawnIndex++;
    }

    private IEnumerator MoveClone(List<Vector2> SubPos2D, int currentSpawnIndexIE, float Spawntime, int GO_enemy)
    {
        GO_Clone[GO_enemy].transform.position = SubPos2D[currentSpawnIndexIE];
        yield return null;
    }

    public void CloneSpawnNbr()
    {
        currentSpawnIndex = 0;
        nonNullList = 0;
        foreach (var listChildPos in ListParentPos2D)
        {
            if (listChildPos.Count > 0)
            {
                nonNullList++;
            }
        }
        Debug.Log($"liste de non null = {nonNullList}");
    }

    public void ChangeCurrentState()
    {
        if (currentNbrClones - 2 == maxNbrOfClones)
        {
            GameManager.Instance.UpdateGameState(GameState.Lose);
            Debug.Log("dead");
        }
        else
        {
            currentSpawnIndex = 0;
            currentNbrClones += 1;
            hasSpawnClones = false;
        }
    }
}