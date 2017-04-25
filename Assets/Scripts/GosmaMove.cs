﻿using System.Collections;
using UnityEngine;

public class GosmaMove : MonoBehaviour {

    public float velocidadeh;
    public float velocidadev;
    public float min;
    public float max;
    public float espera;

    private GameObject player;
    private bool pontuou = false;

    private void Awake() {
        player = GameObject.Find("Nave");
    }

    IEnumerator Move(float destino) {
        while (Mathf.Abs(destino - transform.position.y) > 0.2f) {
            Vector3 direcaov = (destino == max) ? Vector3.up : Vector3.down;
            Vector3 velocidadeVetorial = direcaov * velocidadeh;
            transform.position = transform.position + velocidadeVetorial * Time.deltaTime;
            if(GameController.instancia.estado == Estado.Jogando) {
                if (!pontuou && transform.position.x < player.gameObject.transform.position.x) {
                    pontuou = true;
                    GameController.instancia.incrementarPontos(1);
                }
            }
            yield return null;

        }


        yield return new WaitForSeconds(espera);

        destino = (destino == max) ? min : max;
        StartCoroutine(Move(destino));
    }

    void Update() {

        Vector3 direcaoh = Vector3.left * velocidadev;
        transform.position = transform.position + direcaoh * Time.deltaTime;
        if (!pontuou && GameController.instancia.estado == Estado.Jogando) {
            if (transform.position.x < player.transform.position.x) {
                GameController.instancia.incrementarPontos(1);
                pontuou = true;
            }
        }
    }
}

