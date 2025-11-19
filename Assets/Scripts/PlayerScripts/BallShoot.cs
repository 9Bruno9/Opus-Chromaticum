using System;
using System.Collections;
using ScriptableObjects;

using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShootingSystem : MonoBehaviour
{
    public GameObject projectileprefab;  // Prefab del proiettile
    public Projectile projectColor;
    public Transform GunEnd;          // Punto di partenza del proiettile
    public float projectileForce = 50f;  // Forza del proiettile
    public float fireRate = 0.5f;        // Tempo tra gli spari
    public AlchemyColor equippedColor;
    public GameObject Bastone;
    InputAction shootAction;
    private float nextFireTime = 0f;
    public Camera fpsCam;
    public GameObject player;
    [SerializeField] AudioSource Sound;
    Inventory inventario ;
    private InventoryUI.InventoryUIManager inventoryUIManager;
    
    private ColorStaff _colorStaff;
    private bool _attemptingToShoot;
    private WaitForSecondsRealtime _fireWait;

    private bool _canShoot;

    private void Awake()
    {
        _fireWait = new WaitForSecondsRealtime(fireRate);
    }

    private void OnEnable()
    {
        shootAction = InputSystem.actions.FindAction("Attack");
        shootAction.started += AttemptShoot;
        shootAction.canceled += _ => { _attemptingToShoot = false; };
        _canShoot = true;
    }
    
    private void Start()
    {
        _colorStaff = Bastone.GetComponent<ColorStaff>();
        inventario = player.GetComponent<Inventory>();
        
        inventoryUIManager = FindFirstObjectByType<InventoryUI.InventoryUIManager>();
        //player = GameObject.FindGameObjectWithTag("Player");

    }

    private void OnDisable()
    {
        if (!shootAction.IsUnityNull())
        {
            shootAction.started -= AttemptShoot;
        }
    }


    private void AttemptShoot(InputAction.CallbackContext context)
    {
        if(GameManager.instance.isAMenuOpen) return;
        if (!_canShoot) return;
        StartCoroutine(ShootingCoroutine());
    }

    private IEnumerator ShootingCoroutine()
    {
        _attemptingToShoot = true;
        while (_attemptingToShoot)
        {
            _canShoot = false;
            if (inventario.SubColor(_colorStaff.GiveColor(), 1))
            {
                Shoot();
            }
            yield return _fireWait;
            _canShoot = true;
        }
    }

    /*
    private void Update()
    {
        if (inventoryUIManager.IsInventoryOpen)
            return;


        if (shootAction.WasPressedThisFrame() && Time.time >= nextFireTime)
        {
            if (inventario.SubColor(_colorStaff.GiveColor(), 1))
            {
                Shoot();
                
                nextFireTime = Time.time + fireRate;
            }
        }
    }*/

    private void Shoot()
    {
        //Find the exact hit position using a raycast
        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); //Just a ray through the middle of your current view
        RaycastHit hit;

        //check if ray hits something
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(75); //Just a point far away from the player

        //Calculate direction from attackPoint to targetPoint
        Vector3 direction = targetPoint - GunEnd.position;


        //Instantiate bullet/projectile
        GameObject currentBullet = Instantiate(projectileprefab, GunEnd.position, Quaternion.identity); //store instantiated bullet in currentBullet
                                                                                                        //Rotate bullet to shoot direction

        AlchemyColor ColorEquipped = _colorStaff.GiveColor();
        Projectile proiettile = currentBullet.GetComponent<Projectile>();
        proiettile.ProjectileColor = ColorEquipped;

        // Cambia il colore del materiale
        MeshRenderer renderer = currentBullet.GetComponentInChildren<MeshRenderer>();
        if (!renderer.IsUnityNull())
        {
            // Usa una copia istanziata del materiale per non modificarlo globalmente
            Material newMat = new Material(renderer.material);
            newMat.color = proiettile.ProjectileColor.itemColor;
            renderer.material = newMat;
        }


        currentBullet.transform.forward = direction.normalized;

        //Add forces to bullet
        currentBullet.GetComponent<Rigidbody>().AddForce(direction.normalized * projectileForce, ForceMode.Impulse);
        //currentBullet.GetComponent<Rigidbody>().AddForce(fpsCam.transform.up* upwardForce, ForceMode.Impulse);

        Sound.Play();
    }


    
}
