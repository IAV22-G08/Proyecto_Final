using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public enum TipoFlecha { NORMAL = 0, LIGERA = 1 , PESADA = 2}
public enum FILTRADO_ATAQUE { PRIMERO = 0, FUERTE = 1 , ULTIMO = 2}

public class Tower : MonoBehaviour
{

    public TipoFlecha tipoFlecha = TipoFlecha.NORMAL;
    public FILTRADO_ATAQUE aQuienAtacar = FILTRADO_ATAQUE.PRIMERO;
    public GameObject proyectilNormal;
    public GameObject proyectilPesado;
    public GameObject proyectilLigero;

    public float tiempoEntreDisparo;
    public float velocidadDisparo;
    public float velocidadEnemMax;
    
    private List<GameObject> enemigosEnRango;
    private bool listoDisparo = false;

    GameObject enemigoAtacado = null;
    GameObject cannon = null;
    // Start is called before the first frame update
    void Start()
    {
        enemigosEnRango = new List<GameObject>();
        Invoke("RecargarDisparo", tiempoEntreDisparo);
        if(transform.childCount > 0) cannon = transform.GetChild(0).gameObject;
    }
    
    // Update is called once per frame
    void Update()
    {

        if(enemigoAtacado != null && enemigosEnRango.Count > 0)
            cannon.transform.LookAt(new Vector3(enemigoAtacado.transform.position.x, transform.position.y, enemigoAtacado.transform.position.z));

        if (listoDisparo)
        {
            if(enemigosEnRango.Count > 0)
            {
                switch (aQuienAtacar)
                {
                    case FILTRADO_ATAQUE.PRIMERO:
                        Disparar(enemigosEnRango[0]);
                        enemigoAtacado = enemigosEnRango[0];
                        break;
                    case FILTRADO_ATAQUE.ULTIMO:
                        Disparar(enemigosEnRango[enemigosEnRango.Count - 1]);
                        enemigoAtacado = enemigosEnRango[enemigosEnRango.Count - 1];
                        break;
                    case FILTRADO_ATAQUE.FUERTE:
                        GameObject enemigoaAtacar = seleccionarFuerte();
                        if (enemigoaAtacar)
                        {
                            Disparar(enemigoaAtacar);
                            enemigoAtacado = enemigoaAtacar;
                        }
                        break;

                }
            }
        }
    }

    //Devuelve el más fuerte, si encuentra un enemigo de clase fuerte lo devuelve directamente
    //Si no, se guarda el indice del primer normal y rápido que encuentra y devuelve el normal o el rápido como útlima opción
    private GameObject seleccionarFuerte()
    {
        //La versión 1 es más eficiente ya que no recorre la lista entera pero queda menos claro lo que hace asiqeu utilizamos al segundaa
        #region V1
        //int x = 0;
        //int primerNormal = -1;
        //int primerRapido = -1;
        //while (enemigosEnRango.Count > 0 && x < enemigosEnRango.Count)
        //{

        //    if (enemigosEnRango[x] != null)
        //    {
        //        Enemigo enem = enemigosEnRango[x].GetComponent<Enemigo>();
        //        if (enem && enem.tipoEnem == TipoEnemigo.FUERTE)
        //        {
        //            return enemigosEnRango[x];
        //        }
        //        else if (enem && primerNormal == -1 && enem.tipoEnem == TipoEnemigo.DEFAULT)
        //        {
        //            primerNormal = x;
        //        }
        //        else if (enem && primerRapido == -1 && enem.tipoEnem == TipoEnemigo.RAPIDO)
        //        {
        //            primerRapido = x;
        //        }
        //    }
        //    x++;


        //}


        //GameObject enemigoADevovler = (primerNormal != -1) ? enemigosEnRango[primerNormal] : enemigosEnRango[primerRapido];
        //if (enemigoADevovler == null && enemigosEnRango.Count > 0)
        //{
        //    enemigoADevovler = enemigosEnRango[0];
        //}
        //else
        //{
        //    enemigoADevovler = null;
        //}
        //return enemigoADevovler;

        #endregion

        //Es esta version tenemos una lista de 3 de tamaño (uno para cada tipo de enemigo), en ella nos guardamos el indice en enemigosEnRango que ocupa el primer enemigo de ese tipo que encontremos,
        //si no hay enemigos de ese tipo hay un -1
        List<int> indicesFuertes = new List<int>(3);
        for (int x = 0; x < 3; x++)
        {
            indicesFuertes.Add(-1);
        }
        for (int x = 0; x < enemigosEnRango.Count; x++)
        {
            Enemigo enem = enemigosEnRango[x].GetComponent<Enemigo>();
            if (enem && indicesFuertes[2] == -1 && enem.tipoEnem == TipoEnemigo.FUERTE)
            {
                indicesFuertes[2] = x;
            }
            else if (enem && indicesFuertes[1] == -1 && enem.tipoEnem == TipoEnemigo.DEFAULT)
            {
                indicesFuertes[1] = x;
            }
            else if (enem && indicesFuertes[0] == -1 && enem.tipoEnem == TipoEnemigo.RAPIDO)
            {
                indicesFuertes[0] = x;
            }
        }

        //Se devuelve el primer enemigo con indice distinto de -1, se va de  indicesFuertes.Count - 1 a 0 y aque se va de más fuerte a menos
        for (int x = indicesFuertes.Count - 1; x >= 0; x--)
        {
            if (indicesFuertes[x] > -1)
                return enemigosEnRango[indicesFuertes[x]];
        }

        //si no ha podido encontrar nada
        return null;
       

    }
    private void Disparar(GameObject target)
    {
       
        if(target != null)
        {
            listoDisparo = false;


            GameObject proyectilADisparar = proyectilNormal;
            float velAUsar = velocidadDisparo;
            float velPredecir = velocidadDisparo;
            float timepoRecargaActual = tiempoEntreDisparo;

            if (tipoFlecha == TipoFlecha.PESADA)
            {
                proyectilADisparar = proyectilPesado;
                velAUsar *= 0.5f;
                velPredecir *= 0.5f;
                timepoRecargaActual *= 1.5f;
            }
            else if(tipoFlecha == TipoFlecha.LIGERA)
            {
                proyectilADisparar = proyectilLigero;
                timepoRecargaActual *= 0.8f;


            }


            Invoke("RecargarDisparo", timepoRecargaActual);
            //para saber cuando tiene que volver a disparar

            //velAUsar va a ser la velocidad a la que va a salir el proyectil
            //velPredecir es la velocidad con la que se ahcen los cálculos para disparar
            //por eso en predictedEnemyPosition se usa velPredecir, ya que esta velocidad cambia dependiendo de si ele nemigo es veloz o no, ya que al ser veloz es más dificil predecirlo
            //y es por eso tmb que en la velocidad del rigidBody del proyectil se le pasa velAUsar ya que es la real que vamos a usar y a la que va a slir

            GameObject proyectilDisparado = Instantiate(proyectilADisparar, this.transform);
            //hay que poner al proyectil que salga un radio de distancia  en el que si sale se destruye
            proyectilDisparado.GetComponent<Proyectil>().setRadioDestruccion(this.GetComponent<DrawRadius>().getRadio());
            proyectilDisparado.transform.position = this.transform.position;


            //la flecha ligera predice perfecto
            if(tipoFlecha != TipoFlecha.LIGERA && target.GetComponent<Enemigo>().tipoEnem == TipoEnemigo.RAPIDO)
            {
                //esto es para que si un enemigo va muy rápido no se prediga tan bien la posición, así hay que colocar bien las torres
                velPredecir = velPredecir * 1.75f;
            }

            //Calcular la posición en la que estará el objeto
            Vector3 posPredEnemigo = predictedEnemyPosition(target.transform.position, target.GetComponent<Rigidbody>().velocity, velPredecir);
            Vector3 dirDisparo = posPredEnemigo - this.transform.position;
            //Debug.Log("Pos enem al disparar" + target.transform.position);
            //Debug.Log("Pos pred enem al disparar" + posPredEnemigo);

            //Una vez obtendida la dirección a la que tienen que ir os proyectiles hay que obtener la velocidad de las componentes del vector velocidad
            proyectilDisparado.GetComponent<Rigidbody>().velocity = velocidadFinal(dirDisparo, velAUsar);
            //proyectilDisparado.GetComponent<Rigidbody>().velocity = (dirDisparo.normalized * velocidadDisparo);
            proyectilDisparado.transform.rotation = Quaternion.LookRotation(dirDisparo);
            proyectilDisparado.transform.Rotate(new Vector3(90, 0, 0));
        }
       
        

    }


    Vector3 velocidadFinal(Vector3 dirDestino, float velAUsar)
    {
        float magnitud = Mathf.Sqrt(Mathf.Pow(dirDestino.x, 2) + Mathf.Pow(dirDestino.y, 2) + Mathf.Pow(dirDestino.z, 2));
        float a = velAUsar / magnitud;
        return new Vector3(a * dirDestino.x, a * dirDestino.y, a * dirDestino.z);
    }
    private void RecargarDisparo()
    {
        listoDisparo = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<EnemyMovement>() != null)
        {
            enemigosEnRango.Add(other.gameObject);
            //Debug.Log("Entra");
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<EnemyMovement>() != null)
        {
            enemigosEnRango.Remove(other.gameObject);
            Debug.Log("Sale");
        }
    }

    public void removeEnemyFromList(GameObject gO )
    {
        enemigosEnRango.Remove(gO);
    }

    private Vector3 predictedEnemyPosition(Vector3 targetPosition, Vector3 targetVelocity, float projectileSpeed)
    {
        Vector3 displacement = targetPosition - this.transform.position;
        float targetMoveAngle = Vector3.Angle(-displacement, targetVelocity) * Mathf.Deg2Rad;
        //if the target is stopping or if it is impossible for the projectile to catch up with the target (Sine Formula)
        if (targetVelocity.magnitude == 0 || targetVelocity.magnitude > projectileSpeed && Mathf.Sin(targetMoveAngle) / projectileSpeed > Mathf.Cos(targetMoveAngle) / targetVelocity.magnitude)
        {
            Debug.Log("Position prediction is not feasible.");
            Debug.Log("magnitud = " + targetVelocity.magnitude);
            return targetPosition;
        }
        //also Sine Formula
        float shootAngle = Mathf.Asin(Mathf.Sin(targetMoveAngle) * targetVelocity.magnitude / projectileSpeed);
        return targetPosition + targetVelocity * displacement.magnitude / Mathf.Sin(Mathf.PI - targetMoveAngle - shootAngle) * Mathf.Sin(shootAngle) / targetVelocity.magnitude;




    }
}
