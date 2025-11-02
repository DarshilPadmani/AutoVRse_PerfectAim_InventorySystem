//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.XR.ARFoundation;
//using UnityEngine.XR.ARSubsystems;

//[System.Serializable]
//public class Basket
//{
//    public GameObject basketPrefab;
//    public List<GameObject> ScoreMaterials;
//}

//public class GenerateRandomBins : MonoBehaviour
//{
//    public ARPlaneManager planeManager;
//    public List<Basket> basketElements;

//    private bool basketExists = false;

//    void OnEnable()
//    {
//        planeManager.planesChanged += OnPlanesChanged;
//    }

//    void OnDisable()
//    {
//        planeManager.planesChanged -= OnPlanesChanged;
//    }

//    void OnPlanesChanged(ARPlanesChangedEventArgs args)
//    {
//        if (basketExists) return;

//        foreach (ARPlane plane in args.added)
//        {
//            if (plane.alignment == PlaneAlignment.HorizontalUp)
//            {
//                SpawnNewBasketOnPlane(plane);
//                break;
//            }
//        }
//    }

//    public void SpawnNewBasket()
//    {
//        foreach (ARPlane plane in planeManager.trackables)
//        {
//            if (plane.alignment == PlaneAlignment.HorizontalUp)
//            {
//                SpawnNewBasketOnPlane(plane);
//                break;
//            }
//        }
//    }

//    private void SpawnNewBasketOnPlane(ARPlane plane)
//    {
//        int randomIndex = Random.Range(0, basketElements.Count);
//        Basket selectedBasket = basketElements[randomIndex];

//        // Instantiate the basket prefab
//        GameObject basketInstance = Instantiate(selectedBasket.basketPrefab, plane.center, Quaternion.identity);

//        // Instantiate all ScoreMaterials in the basket
//        foreach (GameObject scoreMaterial in selectedBasket.ScoreMaterials)
//        {
//            // Instantiate scoreMaterial relative to the basket instance
//            Instantiate(scoreMaterial, basketInstance.transform);
//        }

//        basketExists = true;
//    }
//}
