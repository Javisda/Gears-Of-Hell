using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootSystem : MonoBehaviour
{

    // Bullet
    public GameObject bullet;

    // Bullet Force
    public float shootForce;

    // Gun Stats
    public float timeBetweenShooting, spread, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    int bulletsLeft, bulletsShot;

    // Action control
    public bool shooting;
    bool readyToShoot, reloading;

    // Bug fixing
    public bool allowInvoke = true;

    // Audio
    AudioSource gunAudio;

    private void Awake()
    {
        // Inicializacion del variables
        bulletsLeft = magazineSize;
        readyToShoot = true;
        gunAudio = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Recarga autom�ticamente si no quedan balas
        if (readyToShoot && shooting && !reloading && bulletsLeft <= 0) Reload();
    }

    public void Shooting() 
    {
        // Comprueba si se puede disparar
        if (readyToShoot && shooting && !reloading && (bulletsLeft) > 0) {
            bulletsShot = 0;
            Shoot();
        }
    }

    private void Shoot() 
    {
        readyToShoot = false;
        gunAudio.Play();

        // Se calcula la direcci�n y origen del disparo
        Vector3 origin = transform.position;
        Vector3 direction = transform.forward;

        // C�lculo de spread
        float x = Random.Range(-spread, spread);

        // C�lculo de la nueva direcci�n con spread
        Vector3 directionWithSpread = direction + new Vector3(x, 0, 0);

        // Instanciaci�n de la bala
        GameObject currentBullet = Instantiate(bullet, origin, Quaternion.identity);
        currentBullet.transform.forward = directionWithSpread.normalized;
        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);

        bulletsLeft--;
        bulletsShot++;

        if (allowInvoke) {
            Invoke("ResetShot", timeBetweenShooting); // Llama a la funci�n despu�s de timeBetweenShooting segundos
            allowInvoke = false;
        }

        // Repetimos la funcion de disparo dependiendo de bulletsPerTap
        if (bulletsShot < bulletsPerTap && (bulletsLeft > 0)) {
            Invoke("Shoot", timeBetweenShots);
        }
    }

    private void ResetShot() {
        readyToShoot = true;
        allowInvoke = true;
    }

    private void Reload() { // Llamar funci�n cuando jugador pulsa R
        if ((bulletsLeft < magazineSize) && !reloading) { 
            reloading = true;
            Invoke("ReloadFinished", reloadTime);
        }
    }

    private void ReloadFinished() {
        bulletsLeft = magazineSize;
        reloading = false;
    }
}
