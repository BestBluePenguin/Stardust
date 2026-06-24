using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private float duration = 0.4f;
    [SerializeField] private float maxScale = 2.5f;

    [SerializeField] private Color startColor = new Color(1f, 0.6f, 0.2f, 1f);
    [SerializeField] private AudioClip explosionClip;

    private MeshRenderer mr;
    private Material matInstance;
    private AudioSource audioSource;
    private float timer;

    private static readonly int ColorID = Shader.PropertyToID("_Color");
    private static readonly int EmissionID = Shader.PropertyToID("_EmissionColor");

    private void Awake()
    {
        mr = GetComponent<MeshRenderer>();

        // important: instance material so we don't modify shared material
        matInstance = mr.material;
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        PlaySound();
        SetColor(startColor);
    }

    private void Update()
    {
        timer += Time.deltaTime;
        float t = timer / duration;

        // grow
        float scale = Mathf.Lerp(0.2f, maxScale, t);
        transform.localScale = Vector3.one * scale;

        // fade alpha
        Color c = startColor;
        c.a = Mathf.Lerp(1f, 0f, t);

        SetColor(c);

        if (t >= 1f)
            Destroy(gameObject);
    }

    private void PlaySound()
    {
        if (audioSource != null && explosionClip != null)
        {
            audioSource.PlayOneShot(explosionClip);
        }
    }
    private void SetColor(Color c)
    {
        matInstance.SetColor(ColorID, c);
        matInstance.SetColor(EmissionID, c * 2f); // boost glow
    }
}