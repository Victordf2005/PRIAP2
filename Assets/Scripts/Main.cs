using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Main : MonoBehaviour {

    public RawImage imagen;

    public ListaCamaras listaCamaras;

    // Start is called before the first frame update
    void Start()     {
        StartCoroutine(GetRequest("https://servizos.meteogalicia.gal/mgrss/observacion/jsonCamaras.action"));
    }

    // Update is called once per frame
    void Update() {
        
    }

    IEnumerator GetRequest(string uri) {

        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri)) {

            yield return webRequest.SendWebRequest();

            switch (webRequest.result) {
                case UnityWebRequest.Result.Success:
                    VerImagen(webRequest.downloadHandler.text);                    
                    break;
                case UnityWebRequest.Result.ConnectionError:
                    Debug.Log("Error de conexión");
                    break;
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.Log("Error procesando datos");
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.Log("Error de protocolo");
                    break;
            }

        }

    }

    private void VerImagen(string jsonText) {
        // Extraer Json
        listaCamaras = JsonUtility.FromJson<ListaCamaras>(jsonText);

        if (listaCamaras.listaCamaras.Count > 0) {

        int imagenEscogida = Random.Range(0, listaCamaras.listaCamaras.Count);

        StartCoroutine(LoadImage(listaCamaras.listaCamaras[imagenEscogida].imaxeCamara));

        }
    }

    private IEnumerator LoadImage(string uri) {

        WWW www= new WWW(uri);
        yield return www;

        //Renderer renderer = GetComponent<Renderer>();
        imagen.texture = www.texture;
        
    }

}