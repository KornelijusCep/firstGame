using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashController : MonoBehaviour
{

    PlayerController moveScript;

    public float dashSpeed;
    public float dashTime;

    public float dashCooldown = 5.0f;
    float timeSinceAction = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        moveScript = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceAction += Time.deltaTime;
        if (timeSinceAction > dashCooldown)
        {
            if(Input.GetMouseButtonDown(1))
            {
                timeSinceAction = 0;
                StartCoroutine(Dash());
            }
        }
    }

    IEnumerator Dash()
    {
        float startTime = Time.time;

        while(Time.time < startTime + dashTime)
        {
            float directionY = 0;
            moveScript.moveDirection.y = directionY;
            moveScript.controller.Move(moveScript.moveDirection * dashSpeed * Time.deltaTime);
            yield return null;
        }
    }

    
}
