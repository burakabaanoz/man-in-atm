using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    [Header("Etkileþim Ayarlarý")]
    public float uzanmaMesafesi = 3f;

    [Header("Arayüz (UI)")]
    public TextMeshProUGUI elimizdekiParaText; // Sað üstteki yazý
    public Image crosshairResmi;
    public TextMeshProUGUI bakilanObjeText; // Crosshair altýndaki bilgi yazýsý

    [Header("ATM Sistem Referanslarý")]
    public AtmScreenManager ekranYoneticisi; // Hiyerarþiden AtmScreenManager'ýn olduðu objeyi sürükle

    private int elimizdekiPara = 0;
    private Camera anaKamera;
    private PlayerControls kontroller;

    [Header("Görsel Efektler")]
    public GameObject eldekiParaModeli;
    // Hafýzada tutulan durumlar
    private ParaDegeri suAnBakilanPara;
    private Outline suAnkiOutline;
    private bool paraCikisindayiz = false; // Mouse çýkýþ yuvasýnýn üzerinde mi?

    private void Awake()
    {
        kontroller = new PlayerControls();
        kontroller.Gameplay.Interact.performed += ctx => TiklamaAlgila();
    }

    private void OnEnable() { kontroller.Gameplay.Enable(); }
    private void OnDisable() { kontroller.Gameplay.Disable(); }

    void Start()
    {
        anaKamera = Camera.main;
        ArayuzuGuncelle();
        HedefdenCik();
        ElModeliniGuncelle();
    }

    void Update()
    {
        // Iþýn fýrlatarak neye baktýðýmýzý kontrol et
        Ray ray = new Ray(anaKamera.transform.position, anaKamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, uzanmaMesafesi))
        {
            // 1. Durum: Bir Para Destesine mi bakýyoruz?
            ParaDegeri tespitEdilenPara = hit.collider.GetComponent<ParaDegeri>();

            if (tespitEdilenPara != null)
            {
                // Eðer yeni bir desteye geçtiysek eskiyi temizle
                if (suAnBakilanPara != tespitEdilenPara)
                {
                    HedefdenCik();
                    suAnBakilanPara = tespitEdilenPara;

                    // UI ve Outline Geri Bildirimi
                    crosshairResmi.color = Color.red;
                    bakilanObjeText.text = "Al: " + tespitEdilenPara.deger;
                    suAnkiOutline = tespitEdilenPara.GetComponent<Outline>();
                    if (suAnkiOutline != null) suAnkiOutline.enabled = true;
                }
                return; // Para destesindeysek aþaðýyý kontrol etmeye gerek yok, çýk.
            }

            // 2. Durum: Para Çýkýþ Yuvasýna (OutputSlot) mý bakýyoruz?
            if (hit.collider.CompareTag("OutputSlot"))
            {
                if (!paraCikisindayiz) // Sadece ilk karede temizle ve kur
                {
                    HedefdenCik();
                    paraCikisindayiz = true;
                    crosshairResmi.color = Color.green; // Yuvaya bakarken yeþil olsun
                    bakilanObjeText.text = "Tamamla";
                }
                return;
            }
        }

        // 3. Durum: Hiçbir þey veya etkileþimsiz bir þey bakýlýyorsa temizle
        HedefdenCik();
    }

    private void HedefdenCik()
    {
        // Outline söndür
        if (suAnkiOutline != null)
        {
            suAnkiOutline.enabled = false;
            suAnkiOutline = null;
        }

        suAnBakilanPara = null;
        paraCikisindayiz = false; // Çýkýþ yuvasý durumunu sýfýrla

        // UI Sýfýrla
        crosshairResmi.color = Color.white;
        bakilanObjeText.text = "";
    }

    private void TiklamaAlgila()
    {
        // Eðer bir para destesindeysek para al
        if (suAnBakilanPara != null)
        {
            ParaAl(suAnBakilanPara.deger);
        }
        // Eðer çýkýþ yuvasýndaysak iþlemi tamamlamayý dene
        else if (paraCikisindayiz)
        {
            IslemiTamamla();
        }
    }

    private void ParaAl(int miktar)
    {
        elimizdekiPara += miktar;
        ArayuzuGuncelle();
        ElModeliniGuncelle();
        // Bir para aldýðýmýzda, ekran kýrmýzýysa (önceki hata) onu beyaza döndürmek iyi olabilir.
        // Ama istekte sadece doðru parayý verince beyaz olsun dediðin için buna dokunmuyoruz.
    }

    // Eþleþme kontrolünü ve cezayý uygulayan fonksiyon
    private void IslemiTamamla()
    {
        if (ekranYoneticisi == null) return;

        int gerekenMiktar = ekranYoneticisi.SuAnkiIstenenMiktar;

        // Mantýk Kontrolü: Eþleþiyor mu?
        if (elimizdekiPara == gerekenMiktar)
        {
            // BAÞARI DURUMU
            Debug.Log("<color=green>ÝÞLEM BAÞARILI!</color> Doðru miktar verildi.");

            ekranYoneticisi.YeniDegerUret(); // Ekraný sýfýrla, beyaz yap, yeni sayý ver.
        }
        else
        {
            // HATA DURUMU (Eksik veya Fazla)
            Debug.Log("<color=red>HATA!</color> Yanlýþ miktar. Elindeki: " + elimizdekiPara + " Gereken: " + gerekenMiktar);

            ekranYoneticisi.HataDurumu(); // Ekraný kýrmýzý yap (Sayý ayný kalýr).
        }

        // CEZA/SONUÇ: Elimizdeki para her iki durumda da sýfýrlanýr
        elimizdekiPara = 0;
        ArayuzuGuncelle();
        ElModeliniGuncelle();
    }

    private void ArayuzuGuncelle()
    {
        if (elimizdekiParaText != null)
        {
            elimizdekiParaText.text = "Elimdeki Para: " + elimizdekiPara;
        }
    }

    // YENÝ: Paramýzýn durumuna göre görseli açýp kapatan fonksiyon
    private void ElModeliniGuncelle()
    {
        // Eðer referans boþ býrakýldýysa hata vermesin diye kontrol ediyoruz
        if (eldekiParaModeli == null) return;

        // Eðer elimizdeki para 0'dan büyükse modeli aç (Aktif et), yoksa kapat (Pasif et)
        if (elimizdekiPara > 0)
        {
            eldekiParaModeli.SetActive(true);
        }
        else
        {
            eldekiParaModeli.SetActive(false);
        }
    }
}