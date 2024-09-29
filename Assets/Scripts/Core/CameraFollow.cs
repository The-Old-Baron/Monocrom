using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    GameObject playerObject;
    // Start is called before the first frame update
    void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(playerObject == null){
            return;
        }
        transform.position = Vector3.Lerp(this.transform.position, new Vector3(playerObject.transform.position.x, playerObject.transform.position.y, -10), 1.8f * Time.fixedDeltaTime);
        
    }
}