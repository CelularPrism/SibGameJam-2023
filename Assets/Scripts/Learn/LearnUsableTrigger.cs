using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class LearnUsableTrigger : MonoBehaviour
{
    [SerializeField] private GameObject learnObject;
    [SerializeField] private UseEnvironments use;

    private void FixedUpdate()
    {
        if (use != null && use.CanUse())
        {
            learnObject.SetActive(true);
        } else
        {
            learnObject.SetActive(false);
        }
    }
}
