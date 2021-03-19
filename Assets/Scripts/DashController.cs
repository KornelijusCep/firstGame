using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro;

public class DashController : MonoBehaviour
{

    PlayerController moveScript;
    public GameObject player;
    [SerializeField] TMP_Text textClock; 

    public float dashSpeed;
    public float dashTime;

    public float dashCooldown = 5.0f;
    private float timeSinceAction = 0.0f;
    private float tempCooldown;
    private float x;

    // Start is called before the first frame update
    void Start()
    {
        textClock = GameObject.Find("DashTime").GetComponent<TMP_Text>(); 
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
            player.transform.rotation = Quaternion.Euler(45f, moveScript.angle, 0f);
            moveScript.moveDirection.y = directionY;
            moveScript.playerController.Move(moveScript.moveDirection * dashSpeed * Time.deltaTime);
            yield return null;
        }
        player.transform.rotation = Quaternion.Euler(0f, moveScript.angle, 0f);
    }



    
}
