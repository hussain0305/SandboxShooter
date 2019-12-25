using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<Spawnable> offensiveSpawnables;
    public List<Spawnable> defensiveSpawnables;
    public List<Spawnable> decorativeSpawnables;

    void Start()
    {
        SetupUniversalCollisionsRules();
    }

    public void SpawnSpawnable(SpawnableType type, string id, Vector3 location, Quaternion rotation, EPlayerController owner)
    {
        Spawnable receivedBlueprint = GetBlueprint(type, id);
        GameObject spawnedSpawnable = Instantiate(receivedBlueprint.prefab, location, rotation);
        spawnedSpawnable.GetComponent<SpawnableGO>().displayName = receivedBlueprint.displayName;
        spawnedSpawnable.GetComponentInChildren<SpawnableHealth>().InitiateSystems(receivedBlueprint.health);

        if (spawnedSpawnable.GetComponent<DefensiveBase>())
        {
            spawnedSpawnable.GetComponent<DefensiveBase>().SetOwner(owner);
        }
    }

    public Spawnable GetBlueprint(SpawnableType type, string id)
    {
        switch (type)
        {
            case SpawnableType.Offence:
                foreach(Spawnable curr in offensiveSpawnables)
                {
                    if (curr.name == id)
                    {
                        return curr;
                    }
                }
                break;

            case SpawnableType.Defence:
                foreach (Spawnable curr in defensiveSpawnables)
                {
                    if (curr.name == id)
                    {
                        return curr;
                    }
                }
                break;

            case SpawnableType.Decoration:
                foreach (Spawnable curr in decorativeSpawnables)
                {
                    if (curr.name == id)
                    {
                        return curr;
                    }
                }
                break;
        }
        return new Spawnable();
    }

    public bool ObjectExistsAtLocation(Vector3 loc)
    {
        if(Physics.CheckSphere(loc, 1, (1 << LayerMask.NameToLayer("SpawnableGO"))))
        {
            return true;
        }
        return false;
    }

    public void SetupUniversalCollisionsRules()
    {
        //Ignore collision between bounds and projectiles
        Physics.IgnoreLayerCollision(10, 11);
        
        //Ignore collision between bounds and orbitals
        Physics.IgnoreLayerCollision(10, 12);
    }
}
