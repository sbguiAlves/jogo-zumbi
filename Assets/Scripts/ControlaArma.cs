using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ControlaArma : MonoBehaviour
{
    public GameObject Bala;
    public GameObject CanoDaArma;
    public AudioClip SomDoTiro;

    public ControlaInterface scriptControlaInterface;

    private void Start()
    {
        scriptControlaInterface = GameObject.FindObjectOfType(typeof(ControlaInterface)) as ControlaInterface;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown("Fire1") && !EventSystem.current.IsPointerOverGameObject())
        {
            //Instancia novos objetos
            Instantiate(Bala, CanoDaArma.transform.position, CanoDaArma.transform.rotation);
            ControlaAudio.instancia.PlayOneShot(SomDoTiro);
        }

    }
}
