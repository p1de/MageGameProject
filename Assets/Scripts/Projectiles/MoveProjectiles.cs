using UnityEngine;

public class MoveProjectiles : MonoBehaviour
{
    public float speed;
    public float fireRate;

    void Update()
    {
        if (speed != 0)
            transform.position += transform.forward * speed * Time.deltaTime / transform.localScale.x;
        else Debug.LogWarning("Projectile has no speed");
    }
}
