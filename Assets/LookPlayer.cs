using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookPlayer : MonoBehaviour
{
    [SerializeField] Transform objectLookPlayer;
    [SerializeField] float speed;

    private void Update()
    {
        Vector3 direction = PlayerController.instance.transform.position - objectLookPlayer.position;
        direction.Normalize();

        Quaternion targetRot = Quaternion.LookRotation(direction);

        objectLookPlayer.rotation = Quaternion.Lerp(objectLookPlayer.rotation, targetRot, speed * Time.deltaTime);
    }
}
