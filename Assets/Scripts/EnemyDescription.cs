using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class EnemyDescription : MonoBehaviour
{
    public Enemy enemy;
    
    [SerializeField] TMP_Text info;

    // Start is called before the first frame update
    void Start()
    {
        info.text = "Name: " + enemy.name + "\n Health: " + enemy.health.ToString() + "\n Resistance: " + enemy.resistance.ToString() + "\n Attack: " + enemy.attack.ToString() + "\n Max Xp: " + enemy._XPdrop.ToString();
    }

}
