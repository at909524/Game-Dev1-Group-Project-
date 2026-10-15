
using UnityEngine;

public class ChestController : MonoBehaviour
{
    [Header("Chest Settings")]
    public Transform chestLid;
    public float openAngle = -90f;
    public float openSpeed = 120f;

    [Header("Effects")]
    public Light yellowLight;

    [Header("Win Screen")]
    public GameObject winScreen;

    private Quaternion closedRotation;
    private Quaternion openRotation;

    private bool isOpening = false;
    private bool isOpen = false;

    private void Start()
    {

        closedRotation = chestLid.localRotation;


        openRotation = closedRotation *
            Quaternion.Euler(openAngle, 0f, 0f);


        if (yellowLight != null)
        {
            yellowLight.enabled = false;
        }


        if (winScreen != null)
        {
            winScreen.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (!other.CompareTag("Player") || isOpen)
        {
            return;
        }

        isOpen = true;
        isOpening = true;


        if (yellowLight != null)
        {
            yellowLight.enabled = true;
        }
    }

    private void Update()
    {
        if (!isOpening)
        {
            return;
        }


        chestLid.localRotation = Quaternion.RotateTowards(
            chestLid.localRotation,
            openRotation,
            openSpeed * Time.deltaTime
        );


        if (Quaternion.Angle(
            chestLid.localRotation, openRotation) < 0.1f)
        {
            chestLid.localRotation = openRotation;
            isOpening = false;


            if (winScreen != null)
            {
                winScreen.SetActive(true);
            }


            Time.timeScale = 0f;
        }
    }
}
