using System.Collections;
using UnityEngine;
using TMPro;

public class PlayerWeapon : MonoBehaviour
{
    [Header("Weapon Settings")]
    public float damage = 25f;
    public float range = 100f;
    public float fireRate = 0.15f;

    [Header("Ammo Settings")]
    public int magazineSize = 12;
    public int currentAmmo = 12;
    public int reserveAmmo = 36;
    public float reloadTime = 1.5f;

    [Header("Feedback")]
    public GameObject muzzleFlash;
    public GameObject hitImpactPrefab;
    public Transform weaponVisual;
    public float weaponKickAmount = 0.05f;
    public float weaponKickReturnSpeed = 12f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip shootSound;
    public AudioClip reloadSound;

    [Header("References")]
    public Camera playerCamera;
    public TMP_Text ammoText;
    public GameObject reloadText;

    private Vector3 weaponStartLocalPosition;
    private bool isReloading = false;
    private float nextFireTime = 0f;

    private void Awake()
    {
        if (weaponVisual != null)
        {
            weaponStartLocalPosition = weaponVisual.localPosition;
        }

        currentAmmo = magazineSize;
        UpdateAmmoUI();

        if (reloadText != null)
        {
            reloadText.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartReload();
        }

        if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime)
        {
            TryShoot();
        }

        RecoverWeaponKick();
    }

    private void TryShoot()
    {
        if (isReloading)
        {
            Debug.Log("Cannot shoot while reloading.");
            return;
        }

        if (currentAmmo <= 0)
        {
            Debug.Log("No ammo in magazine. Press R to reload.");
            return;
        }

        currentAmmo--;
        nextFireTime = Time.time + fireRate;

        UpdateAmmoUI();

        Debug.Log("Ammo: " + currentAmmo + " / " + reserveAmmo);

        Shoot();
    }

    private void Shoot()
    {
        ShowMuzzleFlash();
        ApplyWeaponKick();
        PlaySound(shootSound);

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, range))
        {
            Debug.Log("Shot hit: " + hitInfo.collider.name);

            SpawnHitImpact(hitInfo);

            TargetHealth targetHealth = hitInfo.collider.GetComponent<TargetHealth>();

            if (targetHealth != null)
            {
                targetHealth.TakeDamage(damage);
            }
        }
        else
        {
            Debug.Log("Shot missed");
        }
    }

    private void StartReload()
    {
        if (isReloading)
        {
            return;
        }

        if (currentAmmo == magazineSize)
        {
            Debug.Log("Magazine already full.");
            return;
        }

        if (reserveAmmo <= 0)
        {
            Debug.Log("No reserve ammo left.");
            return;
        }

        StartCoroutine(ReloadRoutine());
    }

    private IEnumerator ReloadRoutine()
    {
        isReloading = true;
        Debug.Log("Reloading...");
        PlaySound(reloadSound);

        if (reloadText != null)
        {
            reloadText.SetActive(true);
        }

        yield return new WaitForSeconds(reloadTime);

        int ammoNeeded = magazineSize - currentAmmo;
        int ammoToLoad = Mathf.Min(ammoNeeded, reserveAmmo);

        currentAmmo += ammoToLoad;
        reserveAmmo -= ammoToLoad;

        isReloading = false;

        UpdateAmmoUI();

        if (reloadText != null)
        {
            reloadText.SetActive(false);
        }

        Debug.Log("Reload complete. Ammo: " + currentAmmo + " / " + reserveAmmo);
    }

    private void ShowMuzzleFlash()
    {
        if (muzzleFlash == null)
        {
            return;
        }

        muzzleFlash.SetActive(true);
        StartCoroutine(HideMuzzleFlashAfterDelay());
    }

    private IEnumerator HideMuzzleFlashAfterDelay()
    {
        yield return new WaitForSeconds(0.12f);

        if (muzzleFlash != null)
        {
            muzzleFlash.SetActive(false);
        }
    }

    private void SpawnHitImpact(RaycastHit hitInfo)
    {
        if (hitImpactPrefab == null)
        {
            return;
        }

        GameObject impact = Instantiate(
            hitImpactPrefab,
            hitInfo.point,
            Quaternion.LookRotation(hitInfo.normal)
        );

        Destroy(impact, 0.5f);
    }

    private void ApplyWeaponKick()
    {
        if (weaponVisual == null)
        {
            return;
        }

        weaponVisual.localPosition = weaponStartLocalPosition + new Vector3(0f, 0f, -weaponKickAmount);
    }

    private void RecoverWeaponKick()
    {
        if (weaponVisual == null)
        {
            return;
        }

        weaponVisual.localPosition = Vector3.Lerp(
            weaponVisual.localPosition,
            weaponStartLocalPosition,
            weaponKickReturnSpeed * Time.deltaTime
        );
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    private void UpdateAmmoUI()
    {
        if (ammoText != null)
        {
            ammoText.text = currentAmmo + " / " + reserveAmmo;
        }
    }
}