//using UnityEngine;

//public class Target : MonoBehaviour
//{
//    public int points = 10;  // Points awarded for hitting the target
//    private ScoreManager scoreManager;

//    private void Start()
//    {
//        scoreManager = FindObjectOfType<ScoreManager>();
//    }

//    private void OnCollisionEnter(Collision collision)
//    {
//        // Check if the object hitting the target has the tag "Ball"
//        if (collision.gameObject.CompareTag("Ball"))
//        {
//            scoreManager.AddScore(points);  // Add points to score
//            Destroy(gameObject);  // Destroy the target after it's hit
//        }
//    }
//}
