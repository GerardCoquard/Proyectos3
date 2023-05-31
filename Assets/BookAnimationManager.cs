using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookAnimationManager : MonoBehaviour
{
    public void MoveBooks()
    {
        MoveObject[] movers =  GetComponentsInChildren<MoveObject>();
        RotateObject[] rotators =  GetComponentsInChildren<RotateObject>();
        foreach (MoveObject item in movers)
        {
            item.Move();
        }
        foreach (RotateObject item in rotators)
        {
            item.Rotate();
        }
    }
}
