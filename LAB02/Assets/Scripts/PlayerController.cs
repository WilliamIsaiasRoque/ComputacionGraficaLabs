using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float velocidad = 6f;
    public float fuerzaSalto = 10f;
    public float longitudRaycast = 1.5f; 
    public LayerMask capaSuelo;

    [HideInInspector] public bool editWorld = false;

    private bool enSuelo;
    private Rigidbody2D rb;
    public Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        // Si estamos editando, bloqueamos el proceso de inputs
        if (editWorld)
        {
            // Mantenemos velocidad horizontal en 0 pero respetamos la vertical por si acaso
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            return;
        }

        // 1. Entrada de movimiento
        float mov = Input.GetAxis("Horizontal");

        // 2. MOVIMIENTO FÍSICO
        rb.linearVelocity = new Vector2(mov * velocidad, rb.linearVelocity.y);

        // 3. ANIMATOR
        animator.SetFloat("movement", Mathf.Abs(mov * velocidad));

        // 4. ESCALA (0.8f fijo)
        if (mov < 0) transform.localScale = new Vector3(-0.8f, 0.8f, 1f);
        if (mov > 0) transform.localScale = new Vector3(0.8f, 0.8f, 1f);

        // 5. LÓGICA DE SALTO
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, longitudRaycast, capaSuelo);
        enSuelo = hit.collider != null;

        if (enSuelo && Input.GetKeyDown(KeyCode.Space))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
            rb.AddForce(new Vector2(0f, fuerzaSalto), ForceMode2D.Impulse);
        }

        animator.SetBool("ensuelo", enSuelo);
    }

    public void ModePower()
    {
        editWorld = true;

        // FRENADO FÍSICO TOTAL
        rb.linearVelocity = Vector2.zero;

        // RESET DE ANIMATOR: Forzamos el estado de reposo antes de activar el trigger
        animator.SetFloat("movement", 0f);
        animator.SetBool("ensuelo", true);

        animator.SetTrigger("poder");
    }

    public void CloseModerPower()
    {
        editWorld = false;
        animator.SetTrigger("afk");
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * longitudRaycast);
    }
}