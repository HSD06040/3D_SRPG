using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    Animator animator;


    void Awake()
    {
        animator = GetComponent<Animator>();
    }
}

public class CharacterFacialController
{
    Animator animator;

    public CharacterFacialController(Animator animator)
    {
        this.animator = animator;
    }


}
