//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using TMPro;

//public class EventGenerator : MonoBehaviour
//{
//    public string tags = "Ball";
//    private int score = 0;

//    [SerializeField]
//    public TextMeshProUGUI scoreText;

//    void Start()
//    {
//        scoreText.SetText("AR GAME");
//    }

//    private void OnTriggerEnter(Collider other)
//    {
//        if (other.gameObject.CompareTag(tags))
//        {
//            if (tags == "Ball")
//            {
//                Debug.Log("Game Object Detected");
//                UpdateScoreText();
//                other.GetComponent<Ball>().Score();
//            }
//        }
//    }

//    public void UpdateScoreText()
//    {
//        score += 5;
//        scoreText.SetText("Score: " + score);
//        Debug.Log("Score updated: " + score);
//    }
//}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EventGenerator : MonoBehaviour
{
    public string tags = "Ball";
    private int score = 0;

    [SerializeField]
    public TextMeshProUGUI scoreText;

    [SerializeField]
    private AudioClip basketClip;

    [SerializeField]
    private AudioClip noBasketClip;

    private AudioSource audioSource;

    void Start()
    {
        scoreText.SetText("AR GAME");
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(tags))
        {
            if (tags == "Ball")
            {
                Debug.Log("Game Object Detected");
                UpdateScoreText();
            }
        }
    }

    public void UpdateScoreText()
    {
        score += 5;
        scoreText.SetText("Score: " + score);
        Debug.Log("Score updated: " + score);

        PlayAudioClip(basketClip);
    }

    private void PlayAudioClip(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("AudioClip is missing!");
        }
    }
}
