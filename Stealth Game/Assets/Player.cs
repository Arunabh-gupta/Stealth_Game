using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{   
    public float MoveSpeed = 10f;
    public float smoothMoveTime = 0.1f;
    public float turnSpeed = 8f;
    float angle;
    float smoothInputMgnitude;
    float smoothMovevelocity;
    
    Vector3 velocity;
    public Rigidbody rb;
    private void Start() {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        Vector3 inputDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        float InputMganitude = inputDir.magnitude;
        smoothInputMgnitude = Mathf.SmoothDamp(smoothInputMgnitude, InputMganitude, ref smoothMovevelocity, smoothMoveTime);
        float targetAngle = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg;
        angle = Mathf.LerpAngle(angle, targetAngle, turnSpeed * Time.deltaTime * InputMganitude);
        // transform.eulerAngles = Vector3.up * angle;
        // transform.Translate(transform.forward * MoveSpeed * Time.deltaTime * smoothInputMgnitude, Space.World);
        velocity = transform.forward * MoveSpeed * smoothInputMgnitude;
    }
    private void FixedUpdate() {
        rb.MoveRotation(Quaternion.Euler(Vector3.up * angle));
        rb.MovePosition(rb.position + velocity * Time.deltaTime);
    }
}
