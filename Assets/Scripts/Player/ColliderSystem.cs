using UnityEngine;

public class ColliderSystem : MonoBehaviour
{
    // Start is called before the first frame update
    private void ManagerCollision(GameObject Collision) {
        switch (Collision.tag)
        {
            case "Floor":
                // Código para fazer algo
                break;
            case "Obstacle":
                // Código para fazer algo
                break;
            case "Enemy":
                // Código para fazer algo
                break;
            case "Wall":
                // Código para fazer algo
                break;
            default:
                break;
        }   
    }

    private void OnCollisionEnter2D(Collision2D other) {
        ManagerCollision(other.gameObject);
    }
}
