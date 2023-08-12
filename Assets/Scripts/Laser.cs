using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public Camera cam;
    public LineRenderer lineRenderer;
    public Transform firePoint;

    public float targetTime = 3.0f;
    private Quaternion rotation;
    // Start is called before the first frame update
    void Start()
    {
        DisableLaser();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            EnableLaser();
            
        }
        if (Input.GetButton("Fire1"))
        {
            targetTime -= Time.deltaTime; 
            UpdateLaser();
        }

        if (targetTime <= 0.0f)
        {
            DisableLaser();
        }
        if (Input.GetButtonUp("Fire1"))
        {
            DisableLaser();
        }
        // RotateToMouse();
    }

    void EnableLaser()
    {
        lineRenderer.enabled = true;
        targetTime = 3.0f;
    }

    void UpdateLaser()
    {
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        lineRenderer.SetPosition(0, firePoint.position - transform.position);
        // Console.Log(firePoint)

        lineRenderer.SetPosition(1, mousePos - transform.position);
    }

    void DisableLaser()
    {
        lineRenderer.enabled = false;
    }

    // void RotateToMouse()
    // {
    //     Vector2 direction = cam.ScreenToWorldPoint(Input.mousePosition) - transform.position;
    //     float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    //     rotation.eulerAngles = new Vector3(0,0,angle);
    //     transform.rotation = rotation;
    // }
}
