using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class MirrorPuzzleManager : MonoBehaviour
{
    MirrorObject[] mirrorObjects;

    public UnityEvent eventOnComplete;
    public float distanceToDetect = 2f;
    private bool isCompleted;
    void Start()
    {
        mirrorObjects = FindObjectsOfType<MirrorObject>();
        
        SetDistance();
    }

    void Update()
    {
        CheckPuzzleComplete();
    }
    private void SetDistance()
    {
        foreach (MirrorObject item in mirrorObjects)
        {
            item.SetDistance(distanceToDetect);
        }
    }
    private void CheckPuzzleComplete()
    {
        if (mirrorObjects.Length != 0)
        {
            foreach (MirrorObject item in mirrorObjects)
            {
                Debug.Log(item + " :" + item.GetIsValid());
                if (!item.GetIsValid()) return;
            }
            CompletePuzzle();
        }
    }

    private void CompletePuzzle()
    {
        if (!isCompleted)
        {
            Debug.Log("IN COMPLETE");
            eventOnComplete?.Invoke();
            isCompleted = true;
        }
    }
}
