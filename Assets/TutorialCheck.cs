using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialCheck : MonoBehaviour
{
    public GameObject GoalExplanation;
    public GameObject MovementExplanation;
    public GameObject SpacebarExplanation;
    public GameObject LaunchExplanation;

    public Player player;


    public TMP_Text MovementText;
    public TMP_Text SpacebarText;
    public TMP_Text LaunchText; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
           GoalExplanation.gameObject.SetActive(false);
           MovementExplanation.gameObject.SetActive(true);
           SpacebarExplanation.gameObject.SetActive(true);
           LaunchExplanation.gameObject.SetActive(true);

           player.controllable = true;
        }

         if (player.controllable && (Input.GetKeyDown(KeyCode.W) == true||Input.GetKeyDown(KeyCode.A) == true ||Input.GetKeyDown(KeyCode.S) == true ||Input.GetKeyDown(KeyCode.D) == true) )
         {
            MovementText.color = Color.green;
         }
           
         if (player.controllable && (Input.GetKeyDown(KeyCode.Space) == true ))
         {
            SpacebarText.color = Color.green;
         }
           
         if (player.controllable && (Input.GetKeyDown(KeyCode.LeftShift) == true ))
         {
            LaunchText.color = Color.green;
         }
           
         if ( MovementText.color == Color.green && SpacebarText.color == Color.green && LaunchText.color == Color.green )
         {
             StartCoroutine(TutorialOff());
           

         }     

    }

    IEnumerator TutorialOff(){
        yield return new WaitForSeconds(3); //waits 3 seconds
     Destroy(gameObject); //this will work after 3 seconds.
     MovementExplanation.gameObject.SetActive(false);
           SpacebarExplanation.gameObject.SetActive(false);
           LaunchExplanation.gameObject.SetActive(false);
     
 }
}
