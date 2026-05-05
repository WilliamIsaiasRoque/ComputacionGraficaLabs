using UnityEngine;

public class UIManager : MonoBehaviour
{
    // Función para el botón del CHECK
    public void BtnConfirmar()
    {
        if (GeometricTransformer.activePiece != null)
        {
            GeometricTransformer.activePiece.StopEditing();
        }
    }

    // Función para el botón de la X
    public void BtnCancelar()
    {
        if (GeometricTransformer.activePiece != null)
        {
            // Restaurar sus valores iniciales
            GeometricTransformer.activePiece.UndoChanges();
        }
    }
}