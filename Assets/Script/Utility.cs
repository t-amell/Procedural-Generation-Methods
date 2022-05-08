using System.Collections;
using UnityEngine;

public class Utility
{
    // Activate floors in order of creation to showcase creation path
    public static IEnumerator ChangeFloorState(float spd, bool del){
        foreach (Transform floor in GameObject.FindWithTag("FloorHierarchy").transform)
        {
            floor.gameObject.SetActive(del);
            if(del == true){
                yield return new WaitForSeconds(spd);
            }
        }
    }
}
