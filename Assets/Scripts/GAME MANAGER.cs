using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using System.Collections;

public class MazeGameManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject runPanel;
    public GameObject winPanel;
    public GameObject losePanel;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip runSound;
    public AudioClip scream;
    public AudioClip winSound;
    public AudioClip loseSound;
    public AudioSource music;

    [Header("Monster")]
    public NavMeshAgent monsterAgent;
    public Transform player;
    public float initialSpeed = 5f;
    public float chaseSpeed = 35f;
    public float startDelay = 2f;
    public float slowDuration = 2f;

    void Start()
    {
        Time.timeScale = 0f;

        if (runPanel != null) runPanel.SetActive(true);
        if (winPanel != null) winPanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        if (monsterAgent != null)
        {
            monsterAgent.isStopped = true;
            monsterAgent.speed = initialSpeed;
        }

        StartCoroutine(StartSequence());
    }

    IEnumerator StartSequence()
    {
        if (audioSource != null && runSound != null)
        {
            audioSource.PlayOneShot(runSound);
        }

        yield return new WaitForSecondsRealtime(startDelay);

        if (runPanel != null) runPanel.SetActive(false);
        Time.timeScale = 1f;

        if (music != null) music.Play();

        if (monsterAgent != null)
        {
            monsterAgent.isStopped = false;
            monsterAgent.speed = initialSpeed;
        }

        yield return new WaitForSeconds(slowDuration);

        if (monsterAgent != null)
        {
            monsterAgent.speed = chaseSpeed;
        }
    }

    public void PlayerCaught()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f;

        if (audioSource != null && scream != null)
            audioSource.PlayOneShot(scream);

        if (audioSource != null && loseSound != null)
            audioSource.PlayOneShot(loseSound);

        if (losePanel != null) losePanel.SetActive(true);

        if (monsterAgent != null)
            monsterAgent.isStopped = true;

        if (music != null)
            music.Stop();
    }

    public void PlayerWin()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f;

        if (audioSource != null && winSound != null)
            audioSource.PlayOneShot(winSound);

        if (winPanel != null) winPanel.SetActive(true);

        if (monsterAgent != null)
            monsterAgent.isStopped = true;

        if (music != null)
            music.Stop();
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}