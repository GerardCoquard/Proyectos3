using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookPlayer : MonoBehaviour
{
    [SerializeField] Transform objectLookPlayer;
    [SerializeField] Transform target;
    [SerializeField] float speed;
    [SerializeField] float minRot;
    [SerializeField] float maxRot;

    private void Update()
    {
        Vector3 direction = target == null ? PlayerController.instance.transform.position : target.position - objectLookPlayer.position;
        direction.Normalize();

        Quaternion targetRot = Quaternion.LookRotation(direction);
       

        Vector3 currentEulerAngles = objectLookPlayer.rotation.eulerAngles;

        currentEulerAngles.y = Mathf.Clamp(currentEulerAngles.y, minRot, maxRot);

        objectLookPlayer.rotation = Quaternion.Euler(currentEulerAngles);

        objectLookPlayer.rotation = Quaternion.Lerp(objectLookPlayer.rotation, targetRot, speed * Time.deltaTime);
    }
}
