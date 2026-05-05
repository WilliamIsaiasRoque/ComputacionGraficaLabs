using UnityEngine;

public class FondoParallax : MonoBehaviour
{
    public Transform camara;

    public float efectoParallax = 0.5f;

    private float longitud;
    private float posicionInicialX;

    void Start()
    {
        posicionInicialX = transform.position.x;
        longitud = GetComponentInChildren<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        float distancia = camara.position.x * efectoParallax;
        float movimiento = camara.position.x * (1 - efectoParallax);

        transform.position = new Vector3(posicionInicialX + distancia, transform.position.y, transform.position.z);


        if (movimiento > posicionInicialX + longitud)
        {
            posicionInicialX += longitud;
        }
        
        else if (movimiento < posicionInicialX - longitud)
        {
            posicionInicialX -= longitud;
        }
    }
}