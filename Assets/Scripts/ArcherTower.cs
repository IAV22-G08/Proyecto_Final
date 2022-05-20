using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public enum TipoFlecha { NORMAL = 0, LIGERA = 1 , PESADA = 2}
public class ArcherTower : MonoBehaviour
{

    public TipoFlecha tipoFlecha = TipoFlecha.NORMAL;
    public GameObject proyectilNormal;
    public GameObject proyectilPesado;
    public GameObject proyectilLigero;

    public float tiempoEntreDisparo;
    public float velocidadDisparo;
    public float velocidadEnemMax;
    
    private List<GameObject> enemigosEnRango;
    private bool listoDisparo = false;

    // Start is called before the first frame update
    void Start()
    {
        enemigosEnRango = new List<GameObject>();
        Invoke("RecargarDisparo", tiempoEntreDisparo);
    }

    // Update is called once per frame
    void Update()
    {
        if (listoDisparo)
        {
            if(enemigosEnRango.Count > 0)
            {
                Disparar(enemigosEnRango[0]);
            }
        }
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
        else
        {
            enemigosEnRango.Remove(target);
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
            //Debug.Log("Sale");
        }
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




        //Vector3 displacement = targetPosition - this.transform.position;
        //float targetMoveAngle = Vector3.Angle(-displacement, targetVelocity) * Mathf.Deg2Rad;
        ////if the target is stopping or if it is impossible for the projectile to catch up with the target (Sine Formula)
        //if (targetVelocity.magnitude == 0 || targetVelocity.magnitude > projectileSpeed && Mathf.Sin(targetMoveAngle) / projectileSpeed > Mathf.Cos(targetMoveAngle) / targetVelocity.magnitude)
        //{
        //    Debug.Log("Position prediction is not feasible.");
        //    return targetPosition;
        //}
        ////also Sine Formula
        //float shootAngle = Mathf.Asin(Mathf.Sin(targetMoveAngle) * targetVelocity.magnitude / projectileSpeed);
        //return targetPosition + targetVelocity * displacement.magnitude / Mathf.Sin(Mathf.PI - targetMoveAngle - shootAngle) * Mathf.Sin(shootAngle) / targetVelocity.magnitude;


    }
}
