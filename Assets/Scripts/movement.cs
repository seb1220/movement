using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    public Rigidbody rb;
    public CapsuleCollider cc;

    private float movement_force = 80f;
    private float jump_force = 5f;
    private float movement_speed = 12f;
    private float slowdown_force = 40f;
    private float stop_threashold = 0.4f;
    private int n_jump = 2;
    private int i_jump = 0;
    private float air_movement_multiplyer = 0.3f;

    private bool grounded;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // asynchrone timer for double jumb delay

        Debug.Log("Update:");
        Debug.Log(rb.velocity.x);
        Debug.Log(rb.velocity.y);
        Debug.Log(rb.velocity.z);
        if (grounded)
            i_jump = n_jump;

        if (Input.GetKey(KeyCode.W) && grounded)
        {
            rb.AddForce(new Vector3(0, 0, movement_force), ForceMode.Force);
        }
        if (Input.GetKey(KeyCode.A) && grounded)
        {
            rb.AddForce(new Vector3(-movement_force, 0, 0), ForceMode.Force);
        }
        if (Input.GetKey(KeyCode.D) && grounded)
        {
            rb.AddForce(new Vector3(movement_force, 0, 0), ForceMode.Force);
        }
        if (Input.GetKey(KeyCode.S) && grounded)
        {
            rb.AddForce(new Vector3(0, 0, -movement_force), ForceMode.Force);
        }

        if (Input.GetKey(KeyCode.Space) && i_jump > 0)
        {
            rb.AddForce(new Vector3(0, jump_force, 0), ForceMode.Impulse);
            grounded = false;
            --i_jump;
        }
        
        if ((!Input.anyKey && new Vector2(rb.velocity.x, rb.velocity.z).magnitude > 0) || new Vector2(rb.velocity.x, rb.velocity.z).magnitude > movement_speed) {
            slowdown(movement_force);
        }
    }

    private void slowdown(float movement_force)
    {
        if (rb.velocity.magnitude < 0.1)
            return;

        /*Debug.Log("Update:");
        Debug.Log(Mathf.Abs(rb.velocity.x) / rb.velocity.x);
        Debug.Log(Mathf.Abs(rb.velocity.y) / rb.velocity.y);
        Debug.Log(Mathf.Abs(rb.velocity.z) / rb.velocity.z);
        Debug.Log("-------");
        Debug.Log(rb.velocity.x);
        Debug.Log(rb.velocity.y);
        Debug.Log(rb.velocity.z);*/

        
        if (0 < Mathf.Abs(rb.velocity.x) && Mathf.Abs(rb.velocity.x) < stop_threashold)
        {
            Debug.Log("wild1");
            rb.velocity = new Vector3(0, rb.velocity.y, rb.velocity.z);
        }

        if (0 < Mathf.Abs(rb.velocity.z) && Mathf.Abs(rb.velocity.z) < stop_threashold)
        {
            Debug.Log("wild2");
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, 0);
        }

        float x_slowdown = (rb.velocity.x != 0 ? Mathf.Abs(rb.velocity.x) / rb.velocity.x : 0);
        float y_slowdown = 0;//(rb.velocity.y != 0 ? (Mathf.Abs(rb.velocity.y) / rb.velocity.y > 0 ? Mathf.Abs(rb.velocity.y) / rb.velocity.y : 0) : 0);
        float z_slowdown = (rb.velocity.z != 0 ? Mathf.Abs(rb.velocity.z) / rb.velocity.z : 0);

        float slowdown_strength;

        if (!Input.anyKey)
            slowdown_strength = slowdown_force;
        else
            slowdown_strength = movement_force;

        rb.AddForce(new Vector3(-slowdown_strength * x_slowdown, -slowdown_strength * y_slowdown, -slowdown_strength * z_slowdown), ForceMode.Force);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Ground")
            grounded = true;
    }
}
