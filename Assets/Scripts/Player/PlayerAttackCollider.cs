using UnityEngine;

public class PlayerAttackCollider : MonoBehaviour
{
    [SerializeField] private Transform AttackPointRight;
    [SerializeField] private Transform AttackPointLeft;
    [SerializeField] private float RayAtk;
    [SerializeField] LayerMask LayerEnemy;

    [SerializeField] PlayerController PlayerControls;

    //Método de depuração para visualizar a área de ataque
    private void OnDrawGizmos() {
        if(AttackPointRight != null && PlayerControls.directionMovement == DirectionMove.Right) {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(AttackPointRight.position, RayAtk);
        } 
        else
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(AttackPointLeft.position, RayAtk);
        }
    }

    private void Attack() {
        Transform AttackPoint = (PlayerControls.directionMovement == DirectionMove.Right) ? AttackPointRight : AttackPointLeft;

        Collider2D[] CollEnemy = Physics2D.OverlapCircleAll(AttackPoint.position, RayAtk, LayerEnemy);
        foreach (Collider2D ColliderEnemy in CollEnemy)
        {
            if(CollEnemy != null) {
                print($"Enemy {ColliderEnemy}"); // Lógica de dano
            }
        }
    }
}
