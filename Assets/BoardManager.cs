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
    public GameObject BloodParent;

    public Dictionary<Vector2Int, UICard> CardLookup;
    public Dictionary<Vector2Int, UICard> BloodTiles;

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

        CardLookup = new Dictionary<Vector2Int, UICard>();
        BloodTiles = new Dictionary<Vector2Int, UICard>();
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
                PlaceCard(DeckPosition, null);
                Debug.Log("Deck Found: X: " + localPlace.x.ToString() + " Y: " + localPlace.y.ToString());
            }
        }
    }

    internal void SetDeckImage(Tile tile)
    {
        gameBoard.SetTile((Vector3Int)DeckPosition, tile);
        gameBoard.RefreshTile( (Vector3Int) DeckPosition) ;
    }

    public void PlaceCard(Vector2Int position, UICard card)
    {
        if (!BloodTiles.ContainsKey(position) && card != null)
        {
            //  New Blood Tile
            BloodTiles.Add(position, card);
            card.RequiresBloodTrail = true;
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
            var newCard = Instantiate(NewCardPrefab, placementPosition, Quaternion.identity, CardsParent.transform);
            var uiCard = newCard.GetComponent<UICard>();
            uiCard.Init(cardToPlace.GetComponent<UIHandCard>().CurrentCard);
            PlaceCard(Coords2D, uiCard);
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
            var card = CardsParent.transform.GetChild(0);
            DeckManager.Instance.AddToDiscard(card.GetComponent<UICard>().CurrentCard);
            Destroy(card.gameObject);
            AudioManager.Instance.PlaySound(SoundFX.CARD_BURN);
            yield return new WaitForSeconds(0.25f);      
        }
        
        CardLookup.Clear();
        CardLookup.Add(DeckPosition, null);
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
