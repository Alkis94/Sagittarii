using UnityEngine;
using System.Collections;

public class PlayerFireArrow : MonoBehaviour
{

    private Animator animator;
    private Vector3 ArrowEmitterPosition;
    public GameObject ArrowEmitter;
    private float ArrowPower;
    private delegate void VoidDelegate(float someFloat);
    private VoidDelegate FireArrow;





    void Start()
    {
        animator = GetComponent<Animator>();

        if(ItemHandler.ItemDropped["DeadBird"])
        {
            FireArrow = FireArrowWithBird;
        }
        else
        {
            FireArrow = FireArrowWithoutBird;
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            //Debug.Log("GetButtonDown");
            ArrowPower = 0;
            animator.ResetTrigger("PlayerAttackRelease");
            animator.SetTrigger("PlayerAttackHold");
            UIManager.Instance.ResetPower();
        }
        else if (Input.GetButton("Fire1"))
        {

            ArrowPower += Time.deltaTime * ItemHandler.WolfPawMultiplier;
            UIManager.Instance.PowerIncreasing();
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            //Debug.Log("GetButtonUp");
            ArrowPower = ArrowPower > 1 ? 1 : ArrowPower;
            if (ArrowPower > 0.3)
            {
                animator.SetTrigger("PlayerAttackRelease");
                FireArrow(ArrowPower);
            }
            else
            {
                animator.SetTrigger("PlayerAttackCanceled");
            }
        }

    }

    void OnEnable()
    {
        PlayerCollision.OnPlayerGotDeadBird+= PlayerGotDeadBird;
    }


    void OnDisable()
    {
        PlayerCollision.OnPlayerGotDeadBird -= PlayerGotDeadBird;
    }


    // FireArrow function to be changed with delegates

    private void FireArrowWithoutBird(float arrowPower)
    {
        ObjectFactory.Instance.CreateArrow(arrowPower, ArrowEmitter, 0);
    }

    private void FireArrowWithBird(float arrowPower)
    {
        ObjectFactory.Instance.CreateArrow(arrowPower, ArrowEmitter, 0.15f);
        ObjectFactory.Instance.CreateArrow(arrowPower, ArrowEmitter, 0);
        ObjectFactory.Instance.CreateArrow(arrowPower, ArrowEmitter, -0.15f);
    }

    private void PlayerGotDeadBird()
    {
        FireArrow = FireArrowWithBird;
    }


}