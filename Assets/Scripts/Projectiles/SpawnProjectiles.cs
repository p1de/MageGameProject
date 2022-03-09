using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpawnProjectiles : MonoBehaviour
{
    public GameObject firePoint;
    public Camera cam;
    public List<GameObject> vfx = new List<GameObject>();
    private GameObject effectToSpawn;
    private Quaternion rotation;

    void Start()
    {
        effectToSpawn = vfx[0];
    }

    void Update()
    {
        if (!Mouse.current.leftButton.CheckStateIsAtDefault())
            Shoot();
    }

    void Shoot()
    {
        GameObject vfx;
        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        Vector3 target;

        if (Physics.Raycast(ray, out RaycastHit hit))
            target = hit.point;
        else
            target = ray.GetPoint(75);

        if (firePoint != null)
        {
            vfx = Instantiate(effectToSpawn, firePoint.transform.position, Quaternion.identity);
            vfx.transform.forward = target - firePoint.transform.position;
        } 
        else
            Debug.LogWarning("No FirePoint");
    }
}
