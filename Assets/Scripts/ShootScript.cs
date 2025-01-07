using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class ShootScript : MonoBehaviour
{
    [SerializeField] private Transform shootPos;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject magazine;
    [SerializeField] private GameObject magazinePrefab;
    private bool isMagazineDropped;

    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private int maxBullets;
    [SerializeField] private int bulletSpeed;
    [SerializeField] private GameObject droppedMagazinOffset;

    [SerializeField] private AudioSource shootSource;
    [SerializeField] private AudioSource noAmmoSource;
    [SerializeField] private AudioSource reloadSource;

    public ActionBasedController controller;
    public InputActionReference buttonAction;
    public XRSocketInteractor socketInteractor;
    public Magazine magazine_;

    private void Start()
    {
        text.text = "0 / " + maxBullets;
    }

    // void OnEnable()
    // {
    //     buttonAction.action.performed += DropMagazine;
    // }
    //
    // void OnDisable()
    // {
    //     buttonAction.action.performed -= DropMagazine;
    // }

    public void Shoot()
    {
        if (magazine_?.numOfBullets > 0)
        {
            var gmBullet = Instantiate(bulletPrefab, shootPos.position, shootPos.rotation);
            var script = gmBullet.GetComponent<Bullet>();
            shootSource.Play();
            script.Fire(bulletSpeed);
            magazine_.numOfBullets -= 1;
            UpdateUI();
        }
        else
        {
            noAmmoSource.Play();
        }
    }

    public void AddMagazine(SelectEnterEventArgs args)
    {
        print("enter" + args);
        //args.interactableObject.transform.GetComponent<Collider>().enabled = false;
        magazine_ = args.interactableObject.transform.GetComponent<Magazine>();
        reloadSource.Play();
        UpdateUI();
    }

    public void RemoveMagazine(SelectExitEventArgs args)
    {
        print("exit" + args);
        //args.interactableObject.transform.GetComponent<Collider>().enabled = true;
        magazine_ = null;
        UpdateUI();
    }

    // private void DropMagazine(InputAction.CallbackContext context)
    // {
    //     if (isMagazineDropped)
    //     {
    //         return;
    //     }
    //     magazine.GetComponent<Renderer>().enabled = false;
    //     var droppedMagazine = Instantiate(magazinePrefab, droppedMagazinOffset.transform.position, transform.rotation);
    //     isMagazineDropped = true;
    //     curBullets = 0;
    //     UpdateUI();
    //     Destroy(droppedMagazine, 10);
    // }

    // public void Reload()
    // {
    //     curBullets = maxBullets;
    //     UpdateUI();
    // }

    private void UpdateUI()
    {
        if (magazine_)
        {
            text.text = magazine_.numOfBullets + " / " + maxBullets;
        }
        else
        {
            text.text = "0 / " + maxBullets;
        }
    }
}