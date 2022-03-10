using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpawnProjectiles : MonoBehaviour
{
    [SerializeField]
    GameObject firePoint;
    [SerializeField]
    Camera cam;
    [SerializeField]
    List<GameObject> vfx = new List<GameObject>();
    [SerializeField]
    int secondsToDestroy;
    GameObject effectToSpawn;
    GameObject projectile;
    bool buttonHold = false, timerOn = false;
    float timer;

    void Start()
    {
        effectToSpawn = vfx[0];
        projectile = new GameObject();
    }

    void Update()
    {
        IncreaseTime();
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
            var projectilePSSettings = projectile.GetComponentInChildren<ParticleSystem>().main;
            projectile.transform.position = firePoint.transform.position;
            projectile.transform.localRotation = Quaternion.LookRotation(target - firePoint.transform.position);
            if(timer < 5)
            {
                projectile.GetComponentInChildren<ParticleSystem>().transform.localScale += new Vector3(timer / 1000, timer / 1000, timer / 1000);
                if (timer < 1)
                {
                    projectilePSSettings.startColor = new ParticleSystem.MinMaxGradient(new Color(255, 0, 0));
                }
                if(timer >=1 && timer < 2)
                {
                    projectilePSSettings.startColor = new ParticleSystem.MinMaxGradient(new Color(255, 220, 0));
                }
                if (timer >= 2 && timer < 3)
                {
                    projectilePSSettings.startColor = new ParticleSystem.MinMaxGradient(new Color(0, 255, 255));
                }
                if (timer >= 3 && timer < 4)
                {
                    projectilePSSettings.startColor = new ParticleSystem.MinMaxGradient(new Color(255, 255, 255));
                }
                if (timer >= 4)
                {
                    projectilePSSettings.startColor = new ParticleSystem.MinMaxGradient(new Color(120, 0, 120));
                }
            }
            else
                projectile.GetComponentInChildren<ParticleSystem>().transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        }
    }

    void Shoot()
    {
        Destroy(projectile, secondsToDestroy);
    }

    void IncreaseTime()
    {
        if (timerOn)
            timer += Time.deltaTime;
        else
            timer = 0;
    }
}
