using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro;

public class DashController : MonoBehaviour
{

    PlayerController moveScript;

    [SerializeField] TMP_Text textClock; 

    public float dashSpeed;
    public float dashTime;

    public float dashCooldown = 5.0f;
    private float timeSinceAction = 0.0f;
    private float tempCooldown;


    // Start is called before the first frame update
    void Start()
    {
        textClock = GameObject.Find("Text (TMP)").GetComponent<TMP_Text>(); 
        //textClock = GetComponent<Text>(); 
        moveScript = GetComponent<PlayerController>();

        tempCooldown = dashCooldown;
    }

    // Update is called once per frame
    void Update()
    {   
        tempCooldown -= Time.deltaTime;
        if (timeSinceAction > tempCooldown)
        {
            if(Input.GetMouseButtonDown(1) && !moveScript.crouching && !moveScript.lay && !moveScript.swimming)
            {
                tempCooldown = dashCooldown;
                StartCoroutine(Dash());
            }
        }
        else {

        }
        if (textClock != null)
        {
            if(timeSinceAction < tempCooldown)
                textClock.text = $"Dash - {(Mathf.Round(tempCooldown * 10f) *0.1f)}";
            else 
                textClock.text = "Dash - READY";
        }
            
    }

    IEnumerator Dash()
    {
        float startTime = Time.time;

        while(Time.time < startTime + dashTime)
        {
            float directionY = 0;
            moveScript.moveDirection.y = directionY;
            moveScript.playerController.Move(moveScript.moveDirection * dashSpeed * Time.deltaTime);
            yield return null;
        }
    }



    
}
