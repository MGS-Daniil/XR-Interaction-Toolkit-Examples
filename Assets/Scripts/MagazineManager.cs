using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class MagazineManager : MonoBehaviour
{
    [SerializeField] private GameObject magazinePrefab;
    private XRSocketInteractor _socketInteractor;

    private void Start()
    {
        _socketInteractor = gameObject.GetComponent<XRSocketInteractor>();
    }

    public void SpawnMagazine()
    {
        _socketInteractor.startingSelectedInteractable =
            Instantiate(magazinePrefab, transform.position, transform.rotation).GetComponent<XRGrabInteractable>();
    }
}