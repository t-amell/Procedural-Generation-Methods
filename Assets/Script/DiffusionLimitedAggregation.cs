using UnityEngine;

public class DiffusionLimitedAggregation : MonoBehaviour
{
    public GameObject floorTemplate;
    public Transform floorHier;

    // Floors to place
    public int floorNum = 50;

    // Current axis bounds
    private (int, int) xCurBounds = (0,0);
    private (int, int) zCurBounds = (0,0);

    // Speed of floor activation showcase
    public float activationSpeed = .01f;

    // Optional seed var
    public int seed = 0;
    private System.Random rand;
    void Start()
    {
        // If var is set, use it for Random generator
        rand = seed != 0 ? new System.Random(seed) : new System.Random();
        InstantiateFloorPiece(Vector2.zero);

        for (int i = 0; i < floorNum; i++)
        {
            // Randomize compass direction
            int compDir = rand.Next(1, 5);
            Vector3 hitColl = RayDetermination(compDir);

            // Position of soon-to-be generated floor
            Vector2 newPos = new Vector2();

            // Instantiate floor depending right next to hit floor closest to ray origin
            switch (compDir)
            {
                case 1:
                    newPos = InstantiateFloorPiece(new Vector2(hitColl.x-2, hitColl.z));
                    break;
                case 2:
                    newPos = InstantiateFloorPiece(new Vector2(hitColl.x+2, hitColl.z));
                    break;
                case 3: 
                    newPos = InstantiateFloorPiece(new Vector2(hitColl.x, hitColl.z+2));
                    break;
                case 4:
                    newPos = InstantiateFloorPiece(new Vector2(hitColl.x, hitColl.z-2));
                    break;
                default:
                    Debug.LogError("Couldn't instantiate new floor");
                    break;
            }

            // Update bounds if new floor exceeds previous
            if(xCurBounds.Item1 > newPos.x) { xCurBounds.Item1 = (int)newPos.x; }
            if(xCurBounds.Item2 < newPos.x) { xCurBounds.Item2 = (int)newPos.x; }
            if(zCurBounds.Item1 > newPos.y) { zCurBounds.Item1 = (int)newPos.y; }
            if(zCurBounds.Item2 < newPos.y) { zCurBounds.Item2 = (int)newPos.y; }

        }
        // Deactivate floors to then activate for showcase
        StartCoroutine(Utility.ChangeFloorState(0, false));
        StartCoroutine(Utility.ChangeFloorState(activationSpeed, true));
    }

    // Place floor gameObject
    Vector2 InstantiateFloorPiece(Vector2 pos){
        GameObject obj = Instantiate(floorTemplate, new Vector3(pos.x, 0, pos.y), Quaternion.identity, floorHier);
        return pos;
    }

    Vector3 RayDetermination(int compDir){
        // Ray details
        Vector3 rayVecOr = new Vector3();
        Vector3 rayVecDir = new Vector3();
        RaycastHit hit;

        // Possible ray origin bounds (makes it impossible for the ray to miss)
        int varianceX = rand.Next(xCurBounds.Item1, xCurBounds.Item2);
        int varianceZ = rand.Next(zCurBounds.Item1, zCurBounds.Item2);

        switch(compDir){
            case 1:
                rayVecOr = new Vector3(-100, 0, varianceZ);
                rayVecDir = Vector3.right;
                break;
            case 2:
                rayVecOr = new Vector3(100, 0, varianceZ);
                rayVecDir = Vector3.left;
                break;
            case 3: 
                rayVecOr = new Vector3(varianceX, 0, 100);
                rayVecDir = Vector3.back;
                break;
            case 4:
                rayVecOr = new Vector3(varianceX, 0, -100);
                rayVecDir = Vector3.forward;
                break;
            default:
                Debug.LogError("Couldn't random a cardinal direction");
                break;
        }
        // Create ray
        Physics.Raycast(rayVecOr, transform.TransformDirection(rayVecDir), out hit, float.PositiveInfinity);
        return hit.collider.transform.position;
    }
}
