using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowingHints : MonoBehaviour
{

    [SerializeField] TMP_Text tutorialHint; 
    private int hints = 0;
    private bool triggered = false;

    // Update is called once per frame
    void Update()
    {
        
        if(triggered)
        {
            hints = hints + 1; 
            Debug.Log(hints);
        }
        if(hints == 1)
            tutorialHint.text = $"Tutorial: Try move camera around with the mouse and walk with W-A-S-D keyboard keys";
        else if(hints == 2)
            tutorialHint.text = $"Tutorial: You can do single jump by pressing Space";
        else if(hints == 3)
            tutorialHint.text = $"Tutorial: You can do double jump by pressing Space two times in a row";
        else if(hints == 4)
            tutorialHint.text = $"Tutorial: You can do triple jump by pressing Space three times in a row";
        else if(hints == 5)
            tutorialHint.text = $"Tutorial: You can crouch by pressing C";
        else if(hints == 6)
            tutorialHint.text = $"Tutorial: You can lay on the ground by pressing Z";
        else if(hints == 7)
            tutorialHint.text = $"Tutorial: You will some moving platforms try to jump on in and move on!";
        else if(hints == 8)
            tutorialHint.text = $"Tutorial: Try use triple jump and add dash in the end ( Mouse 1 Button)!";
        else if(hints == 9)
            tutorialHint.text = $"Tutorial: Break that red block by jumping in the air and hold ( Mouse 0 Button)";
        else if(hints == 10)
            tutorialHint.text = $"Tutorial: Try to dive in the water with -LeftControl- and dive back with -Space-!";
        else if(hints == 11)
            tutorialHint.text = $"Tutorial: And finally you can finish the game if you ready to jump in and have all the stars!";
        triggered = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Hint")
        {
            triggered = true;
            Destroy(other.gameObject);
        }    
    }

}
