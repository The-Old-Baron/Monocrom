using UnityEngine;

public class ColliderSystem : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] private Vector2 _groundCheckRadius = new Vector2(1, 1);

    [SerializeField] private LayerMask _whatIsWall;
    [SerializeField] private readonly float _wallCheckDistance = 0.5f;

    [SerializeField] private LayerMask _whatIsCeiling;
    [SerializeField] private readonly float _ceilingCheckDistance = 0.5f;

    [SerializeField] private LayerMask _whatIsEnemy;
    [SerializeField] private readonly float _enemyCheckDistance = 0.5f;

    public bool IsTouchingEnemy(Transform transform)
    {
        return Physics2D.Raycast(transform.position, Vector2.right, _enemyCheckDistance, _whatIsEnemy) ||
               Physics2D.Raycast(transform.position, Vector2.left, _enemyCheckDistance, _whatIsEnemy);
    }
    public bool IsGrounded(Transform transform)
    {
        return Physics2D.OverlapCircle(transform.position, _groundCheckRadius.y, _whatIsGround);
    }

    public bool IsTouchingWall(Transform transform, Vector2 direction)
    {
        return Physics2D.Raycast(transform.position, direction, _wallCheckDistance, _whatIsWall);
    }

    public bool IsTouchingCeiling(Transform transform, Vector2 direction)
    {
        return Physics2D.Raycast(transform.position, direction, _ceilingCheckDistance, _whatIsCeiling);
    }

    public bool IsTouchingCeiling(Transform transform)
    {
        return Physics2D.Raycast(transform.position, Vector2.up, _ceilingCheckDistance, _whatIsCeiling);
    }

    public bool IsTouchingWall(Transform transform)
    {
        return Physics2D.Raycast(transform.position, Vector2.right, _wallCheckDistance, _whatIsWall) ||
               Physics2D.Raycast(transform.position, Vector2.left, _wallCheckDistance, _whatIsWall);
    }
}