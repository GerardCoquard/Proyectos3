using System.Collections.Generic;
using UnityEngine;

public class MirrorObject : MonoBehaviour
{
    private bool isValid;
    public int id;
    private List<WorldObject> worldObjects = new List<WorldObject>();
    private float distanceToDetect;

    private void Update()
    {
        CheckList();
    }

    private void CheckList()
    {
        if (worldObjects.Count != 0)
        {

            foreach (WorldObject item in worldObjects)
            {
                float distance = Vector3.Distance(item.transform.position, gameObject.transform.position);
                if (distance < distanceToDetect)
                {
                    isValid = true;
                }
                else
                {
                    isValid = false;
                }

            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<WorldObject>() != null)
        {
            if (other.GetComponent<WorldObject>().id == id)
            {

                worldObjects.Add(other.GetComponent<WorldObject>());
            }
        }
    }
    public void SetDistance(float d)
    {
        distanceToDetect = d;
    }
    public bool GetIsValid()
    {
        return isValid;
    }
}
