using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour
{
    public float speed = 4f;
    float rot;
    //Ray mouseRay;
    //RaycastHit mouseHit;
    //Vector3 mousePos;
    Animator anim;
    Vector3 move;
    CharacterController controller;
    public GameObject interactText;
    public Camera MCamera;

    void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        Movement();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name.Contains("def not a dur"))
        {
            interactText.SetActive(true);
            StartCoroutine("doorInteract",collider);
        }
    }
    private void OnTriggerExit(Collider collider)
    {
        if(collider.gameObject.name.Contains("def not a dur"))
        {
            interactText.SetActive(false);
            StopCoroutine("doorInteract");
        }
    }
    IEnumerator doorInteract(Collider door)
    {
        yield return new WaitForSeconds(0.1f);
        while(true)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Vector3 move = getDoor(door);
                controller.enabled = false;
                transform.position += move;
                controller.enabled = true;
                Camera.main.transform.position += move * 5;
                MCamera.transform.position += move * 5;
                StopCoroutine("doorInteract");
            }
            yield return null;
        }
    }
    private Vector3 getDoor(Collider door)
    {
        Vector3 rot = door.gameObject.transform.root.eulerAngles;
        string doorName = door.gameObject.name;
        int doorRot=0;
        for(int i = 1; i <= 4; i++)
        {
            if (doorName == "def not a dur" + i)
            {
                doorRot = i-1;
                break;
            }
        }
        if (rot.y == 90)
            doorRot++;
        else if (rot.y == 180)
            doorRot += 2;
        else if (rot.y == -90 || rot.y == 270)
            doorRot += 3;
        doorRot %= 4;
        if (doorRot == 0) return new Vector3(0, 0, 2);
        else if (doorRot == 1) return new Vector3(2, 0, 0);
        else if (doorRot == 2) return new Vector3(0,0,-2);
        else return new Vector3(-2, 0, 0);
    }
    //void OnControllerColliderHit(ControllerColliderHit hit)
    //{
    //    if (hit.collider.name.Contains("def not a dur"))
    //    {
    //        interactText.SetActive(true);
    //    //    Debug.Log("asd");
    //    //    Vector3 door = hit.collider.gameObject.transform.position;
    //    //    if (door.x > transform.position.x) transform.position += new Vector3(2f, 0, 0);
    //    //    else if (door.x < transform.position.x) transform.position += new Vector3(-2f, 0, 0);
    //    //    else if (door.z > transform.position.z) transform.position += new Vector3(0, 0, -2f);
    //    //    else transform.position += new Vector3(0, 0, 2f);
    //    }
    //}

    void Movement()
    {

        move = Vector3.zero;

        if (Input.GetKey(KeyCode.A))
        {
            move += new Vector3(-1, 0, 0);
            rot = -90;
        }
        if (Input.GetKey(KeyCode.D))
        {
            move += new Vector3(1, 0, 0);
            rot = 90;
        }
        if (Input.GetKey(KeyCode.W))
        {
            move += new Vector3(0, 0, 1);
            if (Input.GetKey(KeyCode.D)) rot = 45;
            else if (Input.GetKey(KeyCode.A)) rot = -45;
            else rot = 0;
        }
        if (Input.GetKey(KeyCode.S))
        {
            move += new Vector3(0, 0, -1);
            if (Input.GetKey(KeyCode.D)) rot = 135;
            else if (Input.GetKey(KeyCode.A)) rot = -135;
            else rot = 180;
        }
        if (Input.GetMouseButtonDown(0)) 
        {
            //if (Physics.Raycast(mouseRay, out mouseHit)) mousePos = mouseHit.point;
            //rot = Mathf.Atan2(Camera.main.ScreenToViewportPoint(Input.mousePosition).y - Camera.main.WorldToViewportPoint(transform.position).y, Camera.main.ScreenToViewportPoint(Input.mousePosition).x - Camera.main.WorldToViewportPoint(transform.position).x) * Mathf.Rad2Deg;
            anim.SetInteger("Condition", 2);
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)) anim.SetInteger("Condition", 1);
        else anim.SetInteger("Condition", 0);

        move = move.normalized * speed * Time.deltaTime;
        transform.eulerAngles = new Vector3(0, rot, 0);
        //transform.position += move;
        controller.Move(move);
        if (transform.position.y > 0.9f) controller.Move(new Vector3(0, -0.1f, 0));
        if (transform.position.y < 0) controller.Move(new Vector3(0, 0.2f, 0));
    }
}
