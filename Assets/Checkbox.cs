using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Checkbox : MonoBehaviour
{
   private bool isChecked = false;
    public GameObject Box;
    public void Toggle()
    {
        isChecked = !isChecked;

        if(isChecked)
        {
            //TODO: Fazer aniam��o de check
        }
        else
        {
            //TODO: Fazer aniam��o de uncheck
        }
    }
}
