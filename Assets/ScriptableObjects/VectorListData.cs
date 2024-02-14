using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewVectorListData", menuName = "Vector List Data")]
public class VectorListData : ScriptableObject
{
    public List<Vector2> vectorList = new List<Vector2>();

    public void ConvertListToArray()
    {
        Vector2[] vectorArray = vectorList.ToArray();
        Debug.Log("Converted list to array:");
        foreach (Vector2 vector in vectorArray)
        {
            Debug.Log(vector);
        }
    }
}
