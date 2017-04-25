using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


public class GameController : MonoBehaviour {

    public Estado estado { get; private set; }

    public GameObject obstaculo;
    public float espera;
    public float tempoDestruicao;
    public GameObject menuCamera;
    public GameObject menuPanel;
    public GameObject gameOverPanel;
    public GameObject pontosPanel;
    private List<GameObject> obstaculos;


    public static GameController instancia = null;
    private int pontos;
    public Text txtPontos;
    public Text txtMaiorPontuacao;

    private void Awake() {
        if (instancia == null) {
            instancia = this;
        }
        else if (instancia != null) {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start () {
        obstaculos = new List<GameObject>();
        estado = Estado.AguardandoComecar;
        PlayerPrefs.SetInt("HighScore", 0);
        menuCamera.SetActive(true);
        menuPanel.SetActive(true);
        gameOverPanel.SetActive(false);
        pontosPanel.SetActive(false);
    }
	

	IEnumerator GerarObstaculos() {
        while (GameController.instancia.estado == Estado.Jogando) {
            Vector3 pos = new Vector3(4.3f, Random.Range(-3f, 3f), 3.13f);
            GameObject obj = Instantiate(obstaculo, pos, Quaternion.identity) as GameObject;
            obstaculos.Add(obj);
            StartCoroutine(DestruirObstaculo(obj));
            yield return new WaitForSeconds(espera);
        }
    }

    IEnumerator DestruirObstaculo(GameObject obj) {
        yield return new WaitForSeconds(tempoDestruicao);
        if (obstaculos.Remove(obj)) {
            Destroy(obj);
        }
    }


    public void PlayerComecou() {
        estado = Estado.Jogando;
        menuCamera.SetActive(false);
        menuPanel.SetActive(false);
        pontosPanel.SetActive(true);
        atualizarPontos(0);
        StartCoroutine(GerarObstaculos());
    }

    public void PlayerMorreu() {
        estado = Estado.GameOver;
        if (pontos > PlayerPrefs.GetInt("HighScore")) {
            PlayerPrefs.SetInt("HighScore", pontos);
            txtMaiorPontuacao.text = "" + pontos;
        }
        gameOverPanel.SetActive(true);
    }

    private void atualizarPontos(int x) {
        pontos = x;
        txtPontos.text = "" + x;
    }

    public void incrementarPontos(int x) {
        atualizarPontos(pontos + x);
    }

    public void PlayerVoltou() {
        while (obstaculos.Count > 0) {
            GameObject obj = obstaculos[0];
            if (obstaculos.Remove(obj)) {
                Destroy(obj);
            }
        }
        estado = Estado.AguardandoComecar;
        menuCamera.SetActive(true);
        menuPanel.SetActive(true);
        gameOverPanel.SetActive(false);
        pontosPanel.SetActive(false);
        GameObject.Find("Nave").GetComponent<PlayerControllerFINAL>().recomecar();
    }

}
