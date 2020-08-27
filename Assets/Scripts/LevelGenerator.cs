using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject[] treeTypes;
    public GameObject[] grassTypes;

    public GameObject monster;
    MeshCollider col;
    public GameObject terrain;
    [SerializeField] float monsterAppears;

    public int NumOfTrees;
    public int NumOfGrass;
    public int NumOfMonsters;
    // Start is called before the first frame update
    void Start()
    {
        col = terrain.GetComponent<MeshCollider>();
        GeneratePlants(treeTypes, NumOfTrees);
        GeneratePlants(grassTypes, NumOfGrass);
        StartCoroutine(MonsterWillAppear(NumOfMonsters));
    }


    IEnumerator MonsterWillAppear(int amount)
    {
        int count = 0;
        while (count < amount)
        {
            Vector3 randomPoint = GetRandomPoint();
            Instantiate(monster, randomPoint, Quaternion.identity);
            count++;
            yield return new WaitForSeconds(monsterAppears);
        }
    }

    private void GeneratePlants(GameObject[] trees, int amount)
    {
       for(int i = 0; i < amount; i++)
        {
            Vector3 randomPoint = GetRandomPoint();
            GameObject spwn = trees[UnityEngine.Random.Range(0, trees.Length-1)];
            Instantiate(spwn,randomPoint,Quaternion.identity);
        }
    }

    //private void GenerateGrass(GameObject[] grass, int amount)
    //{
    //    for (int i = 0; i < amount; i++)
    //    {
    //        Vector3 randomPoint = GetRandomPoint();
    //        GameObject spwn = grass[UnityEngine.Random.Range(0, grass.Length - 1)];
    //        Instantiate(spwn, randomPoint, Quaternion.identity);
    //    }
    //}

    Vector3 GetRandomPoint()
    {
        int xRandom = 0;
        int yRandom = 0;

        xRandom = (int)UnityEngine.Random.Range(col.bounds.min.x, col.bounds.max.x);
        yRandom = (int)UnityEngine.Random.Range(col.bounds.min.z, col.bounds.max.z);

        return new Vector3(xRandom, 0, yRandom);
    }
}
