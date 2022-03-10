using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpawnProjectiles : MonoBehaviour
{
    public GameObject firePoint;
    public Camera cam;
    public List<GameObject> vfx = new List<GameObject>();
    public int secondsToDestroy;
    private GameObject effectToSpawn;
    private GameObject projectile;
    bool buttonHold = false, timerOn = false;
    float timer;

    void Start()
    {
        effectToSpawn = vfx[0];
        projectile = new GameObject();
    }

    void Update()
    {
        Timer();
        if (!Mouse.current.leftButton.CheckStateIsAtDefault())
        {
            buttonHold = true;
            if(timerOn == false)
            {
                projectile = Instantiate(effectToSpawn, firePoint.transform.position, firePoint.transform.rotation);
            }
            timerOn = true;
            OnHold(timer);
        }
        if (Mouse.current.leftButton.CheckStateIsAtDefault() && buttonHold == true)
        {
            Shoot();
            buttonHold = false;
            timerOn = false;
        }
    }

    void OnHold(float timer)
    {
        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        Vector3 target;

        if (Physics.Raycast(ray, out RaycastHit hit))
            target = hit.point;
        else
            target = ray.GetPoint(75);

        if (firePoint != null)
        {
            projectile.transform.position = firePoint.transform.position;
            projectile.transform.localRotation = Quaternion.LookRotation(target - firePoint.transform.position);
            if (timer < 5)
            {
                projectile.GetComponentInChildren<ParticleSystem>().transform.localScale += new Vector3(timer / 100, timer / 100, timer / 100);
                Debug.Log(projectile.GetComponentInChildren<ParticleSystem>().transform.localScale);
            }
            else
                projectile.GetComponentInChildren<ParticleSystem>().transform.localScale += new Vector3(1, 1, 1);
        }
    }

    void Shoot()
    {
        Destroy(projectile, secondsToDestroy);
    }

    void Timer()
    {
        if (timerOn)
            timer += Time.deltaTime;
        else
            timer = 0;
    }
}
