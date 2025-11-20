using UnityEngine;

public class KeyItem : MonoBehaviour
{
    public float floatSpeed = 1f;
    public float floatHeight = 0.5f;
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float y = Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = startPos + new Vector3(0, y, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            MazeGameManager gm = FindFirstObjectByType<MazeGameManager>();
            if (gm != null) gm.PlayerWin();
            Destroy(gameObject);
        }
    }
}