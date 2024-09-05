using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackCollider : MonoBehaviour
{
    [SerializeField] private Transform AttackPoint;
    [SerializeField] private float RayAtk;
    [SerializeField] LayerMask LayerEnemy;

    //Método de deburação para visualizar a área de ataque
    private void OnDrawGizmos() {
        if(this.AttackPoint != null) {
            Gizmos.DrawWireSphere(this.AttackPoint.position, this.RayAtk);
        }
    }

    private void Attack() {
        Collider2D CollEnemy = Physics2D.OverlapCircle(this.AttackPoint.position, this.RayAtk, this.LayerEnemy);
        if(CollEnemy != null) {
            // Lógica para tirar dano dos inimigos
        }
    }
}
