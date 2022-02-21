using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private void Update()
    {
        /*  - Camera.main.transform.forward; -> olhar para a posição Z oposta a câmera
            + Camera.main.transform.forward; -> olhar para a mesma posição Z da câmera

            LookAt() -> função usada para objeto atual olhar para outro objeto.
            transform.position - -> olhar a partir da posição atual
            
            Este elemento permite que o Canvas (ou qualquer outro elemento de UI
            ou gameobject) olhe para a câmera
        */

        transform.LookAt(transform.position + Camera.main.transform.forward);
         
    }
}
