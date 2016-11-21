using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

    public float Speed = 10f;

    Vector3 movement;
    //animator anim;
    Rigidbody playerRigidbody;
    int FloorMask;
    float camRayLength = 100f;

    void Awake()
    {
        FloorMask = LayerMask.GetMask("Floor");
        playerRigidbody = GetComponent<Rigidbody>();

    }

    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        move(h, v);
        turning();


    }

    void move(float h, float v)
    {
        if (h == 0 && v == 0)
        {
            movement.Set(0f, 0f, 0f);
        }
        else
        {
            movement.Set(h, 0f, v);
            movement = movement.normalized * Speed * Time.deltaTime;
        }
        playerRigidbody.MovePosition(transform.position + movement);

    }

    void turning()
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit floorHit;

        if(Physics.Raycast(camRay , out floorHit, camRayLength , FloorMask))
        {
            Vector3 playerToMouse = floorHit.point - transform.position;
            playerToMouse.y = 0f;

            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
            playerRigidbody.MoveRotation(newRotation);
        }
    }
}
