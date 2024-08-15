using DG.Tweening;
using QAudioManager;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CannonControllerTutorial : MonoBehaviour
{
    [Header("Cannon Configuration")]
    public GameObject cannonballPrefab;
    public Transform firePoint;
    public LineRenderer lineRenderer;
    public int lineSegmentCount = 20;
    public float launchForce = 10f;
    private bool isAiming = false;
    public GameObject dragPanel;

    [Header("Sensitivity Settings")]
    public int UPDownTouchSensitivity = 5;
    public int MoveTouchSensitivity = 5;
    public Slider sensiBar;
    public TextMeshProUGUI sliderValue;

    [Header("Turret Configuration")]
    public Transform Beral;
    public Transform Turret;
    private float XRotation = 0f;
    private float YRotation = 0f;
    public float MinMove = -45f;
    public float MaxMove = 45f;

    [Header("Health & UI")]
    public Slider HealthBar;
    public int cannonBallCount;
    public TextMeshProUGUI cannonBallCountText;
    public GameObject healthIcon;

    private GameController gameController;
    private ObjectPooling objPooled;

    private void Start()
    {
        InitializeSensitivity();
        UpdateSensitivity();

        lineRenderer.enabled = false;

        gameController = FindAnyObjectByType<GameController>();
        objPooled = FindAnyObjectByType<ObjectPooling>();
        CannonBallCountUpdate();
    }

    void Update()
    {
        if (!gameController.gameIsOn) return;

        if (Input.touchCount <= 0) return;

        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Moved)
        {
            HandleTouchMove(touch);
        }

        if (touch.phase == TouchPhase.Ended && isAiming)
        {
            isAiming = false;
            lineRenderer.enabled = false;
        }

        dragPanel.SetActive(!isAiming);
        SensiSlider();
    }

    private void HandleTouchMove(Touch touch)
    {
        float TouchX = touch.deltaPosition.x * MoveTouchSensitivity * Time.deltaTime;
        float TouchY = touch.deltaPosition.y * UPDownTouchSensitivity * Time.deltaTime;

        if (IsTouchOverUI(touch) && IsTouchOverDragPanel(touch))
        {
            YRotation -= TouchX;
            YRotation = Mathf.Clamp(YRotation, MinMove, MaxMove);
            transform.rotation = Quaternion.Euler(0f, YRotation, 0f);

            Quaternion TarRot = Quaternion.Euler(0f, 0f, 0f);
            Turret.localRotation = Quaternion.Slerp(Turret.localRotation, TarRot, 100f * Time.deltaTime);
            Beral.localRotation = Quaternion.Slerp(Turret.localRotation, TarRot, 100f * Time.deltaTime);
            isAiming = false;
        }
        else
        {
            XRotation -= TouchY;
            XRotation = Mathf.Clamp(XRotation, -40f, 40f);
            Beral.localRotation = Quaternion.Euler(XRotation, 0f, 0f);
            YRotation -= TouchX;
            YRotation = Mathf.Clamp(YRotation, -80f, 80f);
            Turret.localRotation = Quaternion.Euler(0f, YRotation * -1, 0f);
            isAiming = true;
            ShowTrajectory();
        }
    }

    private bool IsTouchOverUI(Touch touch)
    {
        return EventSystem.current.IsPointerOverGameObject(touch.fingerId);
    }

    private bool IsTouchOverDragPanel(Touch touch)
    {
        RectTransform dragPanelRect = dragPanel.GetComponent<RectTransform>();
        Vector2 localPoint;
        return RectTransformUtility.ScreenPointToLocalPointInRectangle(dragPanelRect, touch.position, null, out localPoint) && dragPanelRect.rect.Contains(localPoint);
    }

    void ShowTrajectory()
    {
        Vector3 velocity = firePoint.forward * launchForce;
        lineRenderer.positionCount = lineSegmentCount;
        lineRenderer.enabled = true;

        for (int i = 0; i < lineSegmentCount; i++)
        {
            float t = i / (float)lineSegmentCount;
            Vector3 point = firePoint.position + t * velocity;
            point.y += Physics.gravity.y * 0.5f * t * t;
            lineRenderer.SetPosition(i, point);
        }
    }

    void FireCannonball()
    {
        if (cannonBallCount <= 0) return;

        GameObject cannonball = Instantiate(cannonballPrefab, firePoint.position, firePoint.rotation);
        cannonBallCount--;
        CannonBallCountUpdate();

        Rigidbody rb = cannonball.GetComponent<Rigidbody>();
        rb.velocity = firePoint.forward * launchForce;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("EnemyBullet")) return;

        Debug.Log("Hit on Player");
        HealthBar.DOValue(HealthBar.value - 0.2f, 1f); // Smoothly decrease the value over 1 second
        HealthIconMovement();
        Destroy(other.gameObject);
    }

    public void InitializeSensitivity()
    {
        if (PlayerPrefs.HasKey("Sensi"))
        {
            UPDownTouchSensitivity = PlayerPrefs.GetInt("Sensi");
            MoveTouchSensitivity = PlayerPrefs.GetInt("Sensi");
        }
        else
        {
            PlayerPrefs.SetInt("Sensi", UPDownTouchSensitivity);
        }
    }

    public void UpdateSensitivity()
    {
        // Update sensitivity slider and UI
        sensiBar.value = UPDownTouchSensitivity;
        sliderValue.text = sensiBar.value.ToString();
    }

    public void SensiSlider()
    {
        if (sensiBar.value == UPDownTouchSensitivity) return;

        UPDownTouchSensitivity = (int)sensiBar.value;
        PlayerPrefs.SetInt("Sensi", UPDownTouchSensitivity);
        sliderValue.text = sensiBar.value.ToString();
        MoveTouchSensitivity = UPDownTouchSensitivity;
    }

    public void CannonBallCountUpdate()
    {
        cannonBallCountText.text = cannonBallCount.ToString();
    }

    public void HealthIconMovement()
    {
        healthIcon.transform.DOShakePosition(1f, new Vector3(1, 1, 0), 10, 180, false, true);
    }
}
