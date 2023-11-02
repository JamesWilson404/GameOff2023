using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance;
    [SerializeField] Tilemap gameBoard;
    [SerializeField] GameObject CardsParent;

    public Dictionary<Vector2Int, string> CardLookup;
    public Dictionary<Vector2Int, string> BloodTiles;

    public GameObject NewCardPrefab;

    Vector2Int DeckPosition;
    [SerializeField] Vector2Int[] AdjacencyLookup = new Vector2Int[]
    {
        new Vector2Int(1, 0),
        new Vector2Int(-1, 0),
        new Vector2Int(0, 1),
        new Vector2Int(0, -1)
    };


    private void Awake()
    {
        if (Instance == null)
        {
            BoardManager.Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }

        CardLookup = new Dictionary<Vector2Int, string>();
        BloodTiles = new Dictionary<Vector2Int, string>();
        FindStartingDeck();

    }

    private void FindStartingDeck()
    {
        foreach (var pos in gameBoard.cellBounds.allPositionsWithin)
        {
            Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
            if (gameBoard.HasTile(localPlace))
            {
                DeckPosition = (Vector2Int)localPlace;
                PlaceCard(DeckPosition, "Deck");
                Debug.Log("Deck Found: X: " + localPlace.x.ToString() + " Y: " + localPlace.y.ToString());
            }
        }
    }


    public void PlaceCard(Vector2Int position, string card)
    {
        if (!BloodTiles.ContainsKey(position))
        {
            //  New Blood Tile
            BloodTiles.Add(position, card);
        }
        CardLookup.Add(position, card);
    }

    public bool TryPlaceCard(Vector3 mousePos, GameObject cardToPlace)
    {
        var Coords2D = GetNearestTile(mousePos);
        if (IsPlacementValid(Coords2D))
        {
            Debug.Log("PLACABLE");
            var placementPosition = GetCellCenter(Coords2D);
            Instantiate(NewCardPrefab, placementPosition, Quaternion.identity, CardsParent.transform);
            PlaceCard(Coords2D, "New Card");
            return true;
        }
        return false;
    }

    public bool IsPlacementValid(Vector2Int placementPosition)
    {
        if (CardLookup.ContainsKey(placementPosition))
        {
            return false;
        }

        foreach (var item in AdjacencyLookup)
        {
            if (CardLookup.ContainsKey(placementPosition + item))
            {
                return true;
            }
        }
        return false;
    }

    public IEnumerator CleanUpCards()
    {
        Debug.Log("CLEAN");

        while (CardsParent.transform.childCount > 0)
        {
            Destroy(CardsParent.transform.GetChild(0).gameObject);
            AudioManager.Instance.PlaySound(SoundFX.CARD_BURN);
            yield return new WaitForSeconds(0.25f);      
        }
        
        CardLookup.Clear();
        CardLookup.Add(DeckPosition, "Deck");
    }

    public Vector3 GetCellCenter(Vector2Int cellPos)
    {
        return gameBoard.GetCellCenterWorld((Vector3Int)cellPos);
    }    

    public Vector2Int GetNearestTile(Vector3 mousePos)
    {
        var attemptedCoords = gameBoard.WorldToCell(mousePos);
        var Coords2D = (Vector2Int)attemptedCoords;
        return Coords2D;
    }
}
