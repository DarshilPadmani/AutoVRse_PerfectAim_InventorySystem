using UnityEngine;

public class TargetCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Ball"))
            return;

        BallHitTracker ball = collision.gameObject.GetComponent<BallHitTracker>();

        // If no tracker found, add one
        if (ball == null)
            ball = collision.gameObject.AddComponent<BallHitTracker>();

        // Check cooldown
        if (!ball.CanScoreAgain())
            return;

        ball.RegisterHit();

        // Report the hit to the ScoreManager
        if (ScoreManager.Instance != null)
        {
            Vector3 hitPos = collision.contacts.Length > 0 ? collision.contacts[0].point : transform.position;
            ScoreManager.Instance.AddScore(hitPos);
        }
    }
}

public class BallHitTracker : MonoBehaviour
{
    private float lastScoreTime = -Mathf.Infinity;
    [SerializeField] private float scoreCooldown = 3f; 

    // Returns true if enough time passed since last score
    public bool CanScoreAgain()
    {
        return Time.time - lastScoreTime >= scoreCooldown;
    }

    // Called when the ball successfully scores
    public void RegisterHit()
    {
        lastScoreTime = Time.time;
    }
}