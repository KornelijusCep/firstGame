using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class CollectStars : MonoBehaviour
{
    public GameObject endPortal;
    TMP_Text leftStarsText; 

    public int level = 0;
    private int needToCollect = 0;
    private int secretToCollect = 0;
    private int collectedStars = 0;

    void Start()
    {
        endPortal.SetActive(false); 

        leftStarsText = GameObject.Find("LeftStars").GetComponent<TMP_Text>(); 
        level = SceneManager.GetActiveScene().buildIndex;

        if(level == 1)
        {
            needToCollect = 6;
            secretToCollect = 2;
            collectedStars = 0;
        }
        if(level == 2)
        {
            collectedStars = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(needToCollect > collectedStars)
        {
            leftStarsText.text = $"Left Stars: {needToCollect-collectedStars}";
        }
        else {
            if(needToCollect+secretToCollect > collectedStars)
            {
                leftStarsText.text = $"Portal is opened, but some stars left, so you reset with R";
                endPortal.SetActive(true);
            }
            else if(needToCollect+secretToCollect == collectedStars)
            {
                leftStarsText.text = $"I think you good to go now UwU";
            }
        }
        
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Points")
        {
            Destroy(other.gameObject);
            collectedStars++;
        }
    }
}
