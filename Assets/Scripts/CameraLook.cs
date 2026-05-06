using UnityEngine;
using UnityEngine.InputSystem;

public class CameraLook : MonoBehaviour
{
    [Header("Kamera Hassasiyeti")]
    public float hassasiyet = 15f;

    [Header("Yukarý/Aþaðý Bakma Sýnýrlarý (X Ekseni)")]
    public float yukariBakmaSiniri = -30f;
    public float asagiBakmaSiniri = 45f;

    [Header("Saða/Sola Bakma Sýnýrlarý (Y Ekseni)")]
    public float solaBakmaSiniri = -30f;
    public float sagaBakmaSiniri = 60f;

    private float rotasyonX = 0f;
    private float rotasyonY = 0f;

    // Input Asset'ten ürettiðimiz sýnýfý tanýmlýyoruz
    private PlayerControls kontroller;
    private Vector2 fareDelta;

    private void Awake()
    {
        // Kontrolleri baþlat
        kontroller = new PlayerControls();

        // Look aksiyonu her çalýþtýðýnda (fare hareket ettiðinde) delta deðerini al
        kontroller.Gameplay.Look.performed += ctx => fareDelta = ctx.ReadValue<Vector2>();

        // Fare durduðunda delta deðerini sýfýrla
        kontroller.Gameplay.Look.canceled += ctx => fareDelta = Vector2.zero;
    }

    private void OnEnable()
    {
        // Obje aktifleþtiðinde tuþlarý dinlemeye baþla
        kontroller.Gameplay.Enable();
    }

    private void OnDisable()
    {
        // Obje pasifleþtiðinde dinlemeyi býrak (Performans için önemli)
        kontroller.Gameplay.Disable();
    }

    void Start()
    {
        // Oyuna baþlayýnca fare imlecini ekrana kilitle ve gizle
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // ... CameraLook script'inin üst kýsmý ayný kalacak ...

    void Update()
    {
        // GÜNCELLEME: Deðerleri hassasiyetle çarp, ama Time.deltaTime kullanma!
        // Farenin delta deðeri zaten zamaný hesaba katan ham bir deðiþimdir.
        float fareX = fareDelta.x * hassasiyet;
        float fareY = fareDelta.y * hassasiyet;

        // Yukarý/Aþaðý (X ekseni) hesaplamasý ve kýsýtlamasý (Clamp)
        rotasyonX -= fareY;
        rotasyonX = Mathf.Clamp(rotasyonX, yukariBakmaSiniri, asagiBakmaSiniri);

        // Saða/Sola (Y ekseni) hesaplamasý ve kýsýtlamasý (Clamp)
        rotasyonY += fareX;
        rotasyonY = Mathf.Clamp(rotasyonY, solaBakmaSiniri, sagaBakmaSiniri);

        // Hesaplanan açýlarý kameraya uygula
        transform.localRotation = Quaternion.Euler(rotasyonX, rotasyonY, 0f);
    }

}