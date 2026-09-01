using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sign : MonoBehaviour
{
    private Animator anim;
    public GameObject signSprite;
    private bool canPress;
    private IInteractable currentInteractable;

    private void Awake()
    {
        if (signSprite != null)
        {
            anim = signSprite.GetComponent<Animator>();
            signSprite.SetActive(false);
        }
    }

    private void Update()
    {
        if (canPress && currentInteractable != null && Input.GetKeyDown(KeyCode.I))
        {
            currentInteractable.TriggerAction();
            HideSign();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable != null)
        {
            canPress = true;
            currentInteractable = interactable;
            signSprite.SetActive(true);

            if (anim != null)
            {
                anim.SetTrigger("Show");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable != null && interactable == currentInteractable)
        {
            canPress = false;
            currentInteractable = null;
            signSprite.SetActive(false);

            if (anim != null)
            {
                anim.SetTrigger("Hide");
            }
        }
    }

    private void HideSign()
    {
        canPress = false;
        currentInteractable = null;
        signSprite.SetActive(false);
        if (anim != null)
        {
            anim.SetTrigger("Hide");
        }
    }
}