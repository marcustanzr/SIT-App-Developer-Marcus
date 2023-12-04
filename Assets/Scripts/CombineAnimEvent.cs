using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineAnimEvent : MonoBehaviour
{
    public HandController handController;
    public void CombineLeft()
    {
        handController.Combine(0);
    }
    public void CombineRight()
    {
        handController.Combine(1);
    }
}
