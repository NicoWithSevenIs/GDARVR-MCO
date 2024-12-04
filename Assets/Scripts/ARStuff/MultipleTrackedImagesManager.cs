using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class MultipleTrackedImagesManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] characterPrefabs;

    private ARTrackedImageManager _arTrackedImageManager;

    private Dictionary<string, GameObject> _arObjectCharacters;

    // Start is called before the first frame update
    void Start()
    {


        _arTrackedImageManager = GetComponent<ARTrackedImageManager>();
        _arObjectCharacters = new Dictionary<string, GameObject>();

        _arTrackedImageManager.trackedImagesChanged += OnTrackedImageChanged;

        foreach (GameObject prefab in characterPrefabs)
        {
            GameObject newARCharacterObject = Instantiate(prefab);
            newARCharacterObject.transform.position = Vector3.zero;
            newARCharacterObject.transform.rotation = Quaternion.Euler(0, 180, 0);
           // newARCharacterObject.transform.localScale = 
            newARCharacterObject.name = prefab.name;
            newARCharacterObject.gameObject.SetActive(false);
            _arObjectCharacters.Add(newARCharacterObject.name, newARCharacterObject);
        }
    }

    private void OnDestroy()
    {
        _arTrackedImageManager.trackedImagesChanged -= OnTrackedImageChanged;
    }


    public void OnTrackedImageChanged(ARTrackedImagesChangedEventArgs eventArgs) 
    {
        foreach(ARTrackedImage trackedImage in eventArgs.added)
        {
            UpdateTrackedImage(trackedImage);
        }

        foreach(ARTrackedImage trackedImage in eventArgs.updated)
        {
            UpdateTrackedImage(trackedImage);            
            Texture2D test = trackedImage.GetComponent<Texture2D>();

        }

        foreach(ARTrackedImage trackedImage in eventArgs.removed)
        {
            _arObjectCharacters[trackedImage.referenceImage.name].gameObject.SetActive(false);
        }
    }

    private void UpdateTrackedImage(ARTrackedImage trackedImage)
    {
        if(trackedImage.trackingState is UnityEngine.XR.ARSubsystems.TrackingState.Limited
           or UnityEngine.XR.ARSubsystems.TrackingState.None)
        {
            _arObjectCharacters[trackedImage.referenceImage.name].gameObject.SetActive(false);
            return;
        }

        if(characterPrefabs != null)
        {
            _arObjectCharacters[trackedImage.referenceImage.name].gameObject.SetActive(true);
            _arObjectCharacters[trackedImage.referenceImage.name].transform.position = trackedImage.transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
