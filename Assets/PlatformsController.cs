using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformsController : MonoBehaviour
{
    public MoverObject small1Platform;
    public MoveObject small1;
    public Transform small1Large;
    public Transform small1Short;
    public MoverObject small2Platform;
    public MoveObject small2;
    public Transform small2Large;
    public Transform small2Short;
    public MoveObject big;
    bool up;
    bool blocked;
    private void Start() {
        small2.Move();
    }
    public void MovePlatforms()
    {
        if(blocked) return;
        if(up) big.ResetMove();
        else big.Move();

        if(up)
        {
            small1.ResetMove();
        }
        else
        {
            if(small1Platform.HasObjects()) small1.ChangeParams(small1Large);
            else small1.ChangeParams(small1Short);
            small1.Move();
        }

        if(up)
        {
            if(small2Platform.HasObjects()) small2.ChangeParams(small2Short);
            else small2.ChangeParams(small2Large);
            small2.Move();
        }
        else
        {
            small2.ResetMove();
        }
        up=!up;
    }
    public void OnMonsterEnter()
    {
        blocked = true;
        big.ResetMove();
        small1.ChangeParams(small1Large);
        small1.Move();
        small2.ChangeParams(small2Short);
        small2.Move();
    }
    public void OnMonsterExit()
    {
        blocked = true;
        small2.ChangeParams(small2Large);
        small2.Move();
    }
}
