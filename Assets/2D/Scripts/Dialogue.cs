using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    [SerializeField]
    private GameObject dialogueCanvas;
    [SerializeField]
    private GameObject InteractionCanvas;
    [SerializeField]
    private TMP_Text dialogueText; 
    /*[SerializeField] // Added serialized field for InteractionImage
    private Image InteractionImage;*/
    [SerializeField]
    [TextArea]
    private string[] dialogueWords;

    

    private bool dialogueActivated;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && dialogueActivated == true)
        {
            
            //InteractionImage.enabled = false;
            InteractionCanvas.SetActive(false);
            dialogueCanvas.SetActive(true);
            dialogueText.text = dialogueWords[0];

            
          
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            
           //InteractionImage.enabled = true; // Show interaction prompt 
           dialogueActivated = true;
           InteractionCanvas.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        dialogueActivated = false;
        dialogueCanvas.SetActive(false);
        InteractionCanvas.SetActive(false);
        //InteractionImage.enabled = false; // Hide interaction prompt
    }
}