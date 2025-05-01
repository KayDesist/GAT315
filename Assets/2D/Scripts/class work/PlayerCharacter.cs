using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    [SerializeField] AnimationEventRouter animationEventRouter;

    [SerializeField] GameObject meleeWeapon;



    private void Awake()

    {

        animationEventRouter.AddListener("MeleeAttack", OnMeleeAttack);
        

    }



    void OnMeleeAttack(AnimationEvent animationEvent)

    {

        meleeWeapon.SetActive((animationEvent.intParameter == 1));

    }

  
}