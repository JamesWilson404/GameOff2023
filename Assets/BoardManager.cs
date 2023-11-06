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
    [SerializeField] GameObject ShopParent;
    public GameObject BloodParent;

    public Dictionary<Vector2Int, UICard> CardLookup;
    public Dictionary<Vector2Int, UICard> BloodTiles;

    public GameObject NewCardPrefab;
    public GameObject ShopCardPrefab;

    public Card debugCard;

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

    public void PlaceCard(Vector2Int position, UIGameCard card)
    {
        if (!BloodTiles.ContainsKey(position) && card != null)
        {
            //  New Blood Tile
            BloodTiles.Add(position, card);
            card.RequiresBloodTrail = true;
        }
        CardLookup.Add(position, card);
    }

    internal bool IsDeckPosition(Vector2Int coords2D)
    {
        return coords2D == DeckPosition;
    }

    public bool TryPlaceCard(Vector3 mousePos, GameObject cardToPlace)
    {
        var Coords2D = GetNearestTile(mousePos);
        if (IsPlacementValid(Coords2D))
        {
            Debug.Log("PLACABLE");
            var placementPosition = GetCellCenter(Coords2D);
            var newCard = Instantiate(NewCardPrefab, placementPosition, Quaternion.identity, CardsParent.transform);
            var uiCard = newCard.GetComponent<UIGameCard>();
            uiCard.Init(cardToPlace.GetComponent<UIHandCard>().CurrentCard);
            PlaceCard(Coords2D, uiCard);
            return true;
        }
        return false;
    }

    internal IEnumerator PresentShop()
    {

        Game.Instance.GameUI.LoadEventUI(eEventType.Shop);
        yield return new WaitForSeconds(1f);
        for (int i = -2; i < 3; i++)
        {
            if (i == 0)
            {
                i++;
            }

            var placementPosition = GetCellCenter(DeckPosition + new Vector2Int(i, 0));
            var newShopCard = Instantiate(ShopCardPrefab, placementPosition, Quaternion.identity, ShopParent.transform);
            newShopCard.GetComponent<UIEventCard>().Init(debugCard);
            yield return new WaitForSeconds(0.2f);

        }
    }

    public void FinishEvent()
    {
        StartCoroutine(CleanUpShopCards());
        Game.Instance.FinishEvent();
    }

    public IEnumerator AwardBloodTokens()
    {
        yield return new WaitForSeconds(2f);

        Game.Instance.BloodTokens += BloodTiles.Count;

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
        if (CardsParent.transform.childCount == 0)
        {
            yield return null;
        }

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

    public IEnumerator CleanUpShopCards()
    {
        Debug.Log("CLEAN");
        if (ShopParent.transform.childCount == 0)
        {
            yield return null;
        }

        while (ShopParent.transform.childCount > 0)
        {
            var card = ShopParent.transform.GetChild(0);
            Destroy(card.gameObject);
            AudioManager.Instance.PlaySound(SoundFX.CARD_BURN);
            yield return new WaitForSeconds(0.25f);
        }
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
