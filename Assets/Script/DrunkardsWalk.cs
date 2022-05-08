using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class DrunkardsWalk : MonoBehaviour
{
    public GameObject floorTemplate;
    public Transform floorHier;

    // Starting position
    private Vector2 currentPos;
    // Seperate Actors to perform set actions
    public int actors = 5;
    // Actor individual steps
    public int actorSteps = 15;

    // Speed of floor activation showcase
    public float activationSpeed = .01f;

    // Optional seed var
    public int seed = 0;

    // Start is called before the first frame update
    void Start()
    {
        // If var is set, use it for Random generator
        var rand = seed != 0 ? new System.Random(seed) : new System.Random();
        // List of all previous positions
        List<Vector2> posList = new List<Vector2>();
        posList.Add(InstantiateFloorPiece(currentPos));

        for(int i = 0; i < actors; i++){
            // Actor is placed at any available previous floor
            currentPos = posList[rand.Next(0, posList.Count)];
            Debug.Log("Starting: " + currentPos);
            for(int j = 0; j < actorSteps; j++){
                // Move in any 1 coordinate direction
                Vector2 nextPos = MoveCoordinate(rand.Next(1, 5), currentPos);
                // If floor is there, don't create another
                if(!Physics.CheckSphere(nextPos, .25f)){
                    Debug.Log("Creating: " + nextPos);
                    InstantiateFloorPiece(nextPos);
                    currentPos = nextPos;
                    posList.Add(currentPos);
                } else {
                    Debug.Log("Skipping: " + nextPos);
                }
            }
        }
        StartCoroutine(Utility.ChangeFloorState(activationSpeed, true));
    }

    // Place floor gameObject
    Vector2 InstantiateFloorPiece(Vector2 pos){
        GameObject obj = Instantiate(floorTemplate, new Vector3(pos.x, 0, pos.y), Quaternion.identity, floorHier);
        obj.SetActive(false);
        return pos;
    }

    // Choose a direction to place a floor (Likely be better optimized/cleaner, but this works)
    Vector2 MoveCoordinate(int direction, Vector2 pos){
        switch(direction){
            case 1:
                pos.x += 2;
                break;
            case 2:
                pos.x -= 2;
                break;
            case 3: 
                pos.y += 2;
                break;
            case 4:
                pos.y -= 2;
                break;
            default:
                Debug.LogError("Couldn't random a cardinal direction");
                break;
        }
        return pos;
    }
}
