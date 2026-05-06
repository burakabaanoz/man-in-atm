using UnityEngine;
using TMPro; // Ekranda TextMeshPro kullandýðýmýz için þart

public class AtmScreenManager : MonoBehaviour
{
    [Header("UI Referanslarý")]
    public TextMeshProUGUI istenenParaText; // ATM ekranýndaki yazý (Hiyerarþiden sürükle)

    [Header("Rastgele Sayý Ayarlarý")]
    public int minPara = 10;
    public int maxPara = 100;

    // Diðer kodlarýn bu deðeri okuyabilmesi ama deðiþtirememesi için (Properties)
    public int SuAnkiIstenenMiktar { get; private set; }

    void Start()
    {
        // UI referansý baðlanmamýþsa hata verme, kodu durdur
        if (istenenParaText == null)
        {
            Debug.LogError("Lütfen ATM Ekranýndaki TextMeshPro'yu AtmScreenManager'a baðlayýn!");
            enabled = false;
            return;
        }

        // Oyun baþlarken ilk deðeri üret
        YeniDegerUret();
    }

    // 5'in katý olacak þekilde rastgele bir deðer üretir ve ekrana yazar
    public void YeniDegerUret()
    {
        // Örn: 10 ile 100 arasý 5'in katý: (10/5=2, 100/5=20). 2-20 arasý rastgele tam sayý seçip 5 ile çarp.
        int minFaktor = minPara / 5;
        int maxFaktor = (maxPara / 5) + 1; // Random.Range max'ý dahil etmez, +1 yapýyoruz.

        SuAnkiIstenenMiktar = Random.Range(minFaktor, maxFaktor) * 5;

        // Yazýyý beyaz yap ve güncelle
        istenenParaText.color = Color.white;
        istenenParaText.text = "Ýstenen: " + SuAnkiIstenenMiktar;
    }

    // Hata yapýldýðýnda ekraný kýrmýzýya çevirir (Ýstenen miktar ayný kalýr)
    public void HataDurumu()
    {
        istenenParaText.color = Color.red;
    }
}