using UnityEngine;

public class MoveProjectiles : MonoBehaviour
{
    [SerializeField]
    float speed;

    void Update()
    {
        if (speed != 0)
            transform.position += transform.forward * speed * Time.deltaTime / transform.GetChild(0).localScale.x;
        else Debug.LogWarning("Projectile has no speed");
    }
}
