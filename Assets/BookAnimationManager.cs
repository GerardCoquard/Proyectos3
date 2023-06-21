using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookAnimationManager : MonoBehaviour
{
    private void Start() {
        Collider[] colliders =  GetComponentsInChildren<Collider>();
        foreach (Collider item in colliders)
        {
            item.isTrigger = true;
        }
    }
    public void MoveBooks()
    {
        Collider[] colliders =  GetComponentsInChildren<Collider>();
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
        foreach (Collider item in colliders)
        {
            item.isTrigger = false;
        }
    }

    public void PlayBooksAnimation()
    {
        BookAnimationRandomizer[] books = GetComponentsInChildren<BookAnimationRandomizer>();

        foreach (BookAnimationRandomizer book in books)
        {
            book.PlayRandomAnimation();
        }
    }
}
