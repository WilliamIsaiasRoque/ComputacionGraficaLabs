using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    private bool estaReiniciando = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("died") && !estaReiniciando)
        {
            StartCoroutine(Reiniciar());
        }
    }

    private IEnumerator Reiniciar()
    {
        estaReiniciando = true;

        PlayerController pc = GetComponent<PlayerController>();
        if (pc != null) pc.editWorld = true;

        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}