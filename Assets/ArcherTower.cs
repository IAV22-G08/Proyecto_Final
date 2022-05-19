using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherTower : MonoBehaviour
{

    public GameObject proyectil;
    public float tiempoEntreDisparo;
    public float velocidadDisparo;
    
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
            GameObject proyectilDisparado = Instantiate(proyectil, this.transform);
            //hay que poner al proyectil que salga un radio de distancia  en el que si sale se destruye
            proyectilDisparado.GetComponent<Proyectil>().setRadioDestruccion(this.GetComponent<DrawRadius>().getRadio());
            proyectilDisparado.transform.position = this.transform.position;
            Vector3 dirDisparo = predictedEnemyPosition(target.transform.position, target.GetComponent<Rigidbody>().velocity, velocidadDisparo) - this.transform.position;
            proyectilDisparado.GetComponent<Rigidbody>().velocity = (dirDisparo.normalized * velocidadDisparo);
            proyectilDisparado.transform.rotation = Quaternion.LookRotation(dirDisparo);
            proyectilDisparado.transform.Rotate(new Vector3(90, 0, 0));
        }
        else
        {
            enemigosEnRango.Remove(target);
        }
        

        Invoke("RecargarDisparo", tiempoEntreDisparo);
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
            Debug.Log("Entra");
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


    private Vector3 predictedEnemyPosition(Vector3 targetPosition, Vector3 targetVelocity, float projectileSpeed)
    {

        Vector3 displacement = targetPosition - this.transform.position;
        float targetMoveAngle = Vector3.Angle(-displacement, targetVelocity) * Mathf.Deg2Rad;
        //if the target is stopping or if it is impossible for the projectile to catch up with the target (Sine Formula)
        if (targetVelocity.magnitude == 0 || targetVelocity.magnitude > projectileSpeed && Mathf.Sin(targetMoveAngle) / projectileSpeed > Mathf.Cos(targetMoveAngle) / targetVelocity.magnitude)
        {
            Debug.Log("Position prediction is not feasible.");
            return targetPosition;
        }
        //also Sine Formula
        float shootAngle = Mathf.Asin(Mathf.Sin(targetMoveAngle) * targetVelocity.magnitude / projectileSpeed);
        return targetPosition + targetVelocity * displacement.magnitude / Mathf.Sin(Mathf.PI - targetMoveAngle - shootAngle) * Mathf.Sin(shootAngle) / targetVelocity.magnitude;

        //Vector3 displacement = targetPosition - shooterPosition;
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
