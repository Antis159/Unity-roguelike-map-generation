/* 
 * author : jiankaiwang
 * description : The script provides you with basic operations of first personal control.
 * platform : Unity
 * date : 2017/12
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController2 : MonoBehaviour {

    public float speed = 10.0f;
    public float sprintSpeed = 1000f;
    private float translation;
    private float straffe;

    void Start () {
        Cursor.lockState = CursorLockMode.Locked;		
        }
	void Update () {
                    
            float speedModifier = 1;
        if (Input.GetKey(KeyCode.LeftShift)) {
            speedModifier = sprintSpeed;
        } else {
            speedModifier = speed;
        }
        translation = Input.GetAxis("Vertical") * speedModifier * Time.deltaTime;
        straffe = Input.GetAxis("Horizontal") * speedModifier * Time.deltaTime;
        transform.Translate(straffe, 0, translation);

        if (Input.GetKeyDown("escape")) {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}