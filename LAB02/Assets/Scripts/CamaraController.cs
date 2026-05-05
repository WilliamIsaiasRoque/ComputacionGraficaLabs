using UnityEngine;

public class CamaraController : MonoBehaviour
{
    public Transform objetivo;
    public float velocidadCamara = 0.1f;
    public Vector3 desplazamiento;

    [Header("Límites del Mapa")]
    public float minX;
    public float maxX;

    private void LateUpdate()
    {
        if (objetivo == null) return;

        Vector3 posicionDeseada = new Vector3(
            objetivo.position.x + desplazamiento.x,
            desplazamiento.y, // Altura fija independiente del salto
            objetivo.position.z + desplazamiento.z
        );

        // 2. RESTRICCIÓN HORIZONTAL
        posicionDeseada.x = Mathf.Clamp(posicionDeseada.x, minX, maxX);

        // 3. Suavizado (Lerp)
        Vector3 posicionSuavizada = Vector3.Lerp(transform.position, posicionDeseada, velocidadCamara);

        // 4. Aplicamos la posición final
        transform.position = posicionSuavizada;
    }
}