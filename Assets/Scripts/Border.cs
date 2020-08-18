using UnityEngine;

public class Border : MonoBehaviour
{
    [SerializeField] private Vector2 _normal;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            collision.gameObject.GetComponent<BallMovement>().BounceBall(_normal);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(_normal.x, _normal.y) * 2f);
    }
}
