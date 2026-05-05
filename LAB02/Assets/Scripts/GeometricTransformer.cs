using UnityEngine;

public class GeometricTransformer : MonoBehaviour
{
    public static GeometricTransformer activePiece;

    [Header("Referencias")]
    public PlayerController player;
    public GameObject uiPanel;

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Vector3 initialScale;

    [Header("Límites de Transformación")]
    [Tooltip("Distancia máxima que puede alejarse (arriba/abajo/lados)")]
    public float limiteDesplazamiento = 5f;
    [Tooltip("Multiplicador máximo de escala")]
    public float maxEscalaMultiplo = 2f;
    [Tooltip("Multiplicador mínimo de escala")]
    public float minEscalaMultiplo = 0.5f;

    [Header("Habilidades")]
    public bool canRotate = false;
    public bool canTranslate = false;
    public bool canScale = false;

    private bool isEditing = false;

    void OnMouseDown()
    {
        // Solo iniciamos edición si el mundo no está ya en modo edición (evita conflictos)
        if (!player.editWorld) StartEditing();
    }

    void StartEditing()
    {
        activePiece = this;
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        initialScale = transform.localScale;

        isEditing = true;
        uiPanel.SetActive(true);
        player.ModePower();
    }

    public void StopEditing()
    {
        isEditing = false;
        uiPanel.SetActive(false);
        player.CloseModerPower();
        activePiece = null;
    }

    public void UndoChanges()
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        transform.localScale = initialScale;
        StopEditing();
    }

    void Update()
    {
        if (!isEditing) return;

        // --- 1. ROTACIÓN ---
        if (canRotate)
        {
            if (Input.GetKey(KeyCode.Q)) transform.Rotate(0, 0, 150f * Time.deltaTime);
            if (Input.GetKey(KeyCode.E)) transform.Rotate(0, 0, -150f * Time.deltaTime);
        }

        // --- 2. TRASLACIÓN LIMITADA (WASD / Flechas) ---
        if (canTranslate)
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            Vector3 movimiento = new Vector3(h, v, 0) * 5f * Time.deltaTime;
            Vector3 targetPosition = transform.position + movimiento;

            // Restricción matemática (Clamping)
            targetPosition.x = Mathf.Clamp(targetPosition.x, initialPosition.x - limiteDesplazamiento, initialPosition.x + limiteDesplazamiento);
            targetPosition.y = Mathf.Clamp(targetPosition.y, initialPosition.y - limiteDesplazamiento, initialPosition.y + limiteDesplazamiento);

            transform.position = targetPosition;
        }

        // --- 3. ESCALA LIMITADA (Rueda del mouse) ---
        if (canScale)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0)
            {
                Vector3 targetScale = transform.localScale + Vector3.one * scroll * 1f;

                float minX = initialScale.x * minEscalaMultiplo;
                float maxX = initialScale.x * maxEscalaMultiplo;
                float minY = initialScale.y * minEscalaMultiplo;
                float maxY = initialScale.y * maxEscalaMultiplo;

                targetScale.x = Mathf.Clamp(targetScale.x, minX, maxX);
                targetScale.y = Mathf.Clamp(targetScale.y, minY, maxY);
                targetScale.z = initialScale.z; 

                transform.localScale = targetScale;
            }
        }

        // --- 4. ATAJOS DE TECLADO (Confirmar / Revertir) ---
        
        // Enter para confirmar (acepta el del teclado normal y el numérico)
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            StopEditing();
        }

        // Escape para cancelar y volver al estado inicial
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UndoChanges();
        }
    }
}