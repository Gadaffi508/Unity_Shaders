using UnityEngine;

public class SpiderLeg : MonoBehaviour
{
    public Transform footTarget; // Bacak ucu hedefi (IK hedef noktası)
    public float stepHeight = 0.5f; // Adım yüksekliği
    public float stepSpeed = 5f; // Adım hızı
    public float stepThreshold = 1f; // Adım atma için mesafe eşiği
    public LayerMask groundLayer; // Zemin katmanı (Raycast için)

    private Vector3 lastPosition;
    private Vector3 targetPosition;
    private bool isStepping;

    void Start()
    {
        lastPosition = footTarget.position;
        targetPosition = lastPosition;
    }

    void Update()
    {
        // Raycast ile zemini bul
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 5f, groundLayer))
        {
            Vector3 groundPosition = hit.point; // Zeminle kesişim noktası

            Debug.DrawLine(transform.position,hit.point,Color.red);
            
            // Eğer bacak hedefi ile yeni hedef arasındaki mesafe büyükse, hedefi güncelle
            if (Vector3.Distance(lastPosition, groundPosition) > stepThreshold && !isStepping)
            {
                targetPosition = groundPosition; // Yeni hedef pozisyonu belirle
                isStepping = true;
            }
        }

        // Eğer adım atılıyorsa hedefe doğru ilerle
        if (isStepping)
        {
            MoveLegTowardsTarget();
        }
    }

    // Bacağı hedefe doğru taşımak
    void MoveLegTowardsTarget()
    {
        // Mevcut pozisyon
        Vector3 currentPosition = footTarget.position;

        // Hedefe doğru hareket ettir (Lerp ile smooth geçiş)
        Vector3 newPosition = Vector3.Lerp(currentPosition, targetPosition, Time.deltaTime * stepSpeed);

        // Y eksenini yükseltmek için parabolik bir yörünge oluştur
        float midPoint = Mathf.Clamp01(Vector3.Distance(currentPosition, targetPosition) / stepThreshold);
        newPosition.y += Mathf.Sin(midPoint * Mathf.PI) * stepHeight;

        // Hedef pozisyonuna ulaştıysa
        if (Vector3.Distance(newPosition, targetPosition) < 0.05f)
        {
            isStepping = false;
            lastPosition = targetPosition; // Adımı tamamla ve yeni son pozisyonu belirle
        }

        // Bacağı yeni pozisyona taşı
        footTarget.position = newPosition;
    }
}
