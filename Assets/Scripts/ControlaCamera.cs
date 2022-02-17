using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlaCamera : MonoBehaviour
{
    public GameObject Jogador;
    private Vector3 distCompensar; //entre jogador e câmera

    void Start() //calcular a diferença de distância entre jogador e câmera
    {
        distCompensar = transform.position - Jogador.transform.position;
    }

    void Update()
    {
        transform.position =  Jogador.transform.position + distCompensar;  
    }
}
