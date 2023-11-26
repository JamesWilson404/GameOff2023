using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public GameObject ScaleCardPrefab;

    public UIScaleCard currentHopeCard;
    public UIScaleCard currentBloodCard;

    public Card debugCard;

    public Vector2Int DeckPosition;
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

    public IEnumerator SpawnScaleCards()
    {
        yield return new WaitForSeconds(0.2f);

        var placementPosition1 = GetCellCenter(DeckPosition + new Vector2Int(-1, 0));
        var newShopCard1 = Instantiate(ScaleCardPrefab, placementPosition1, Quaternion.identity, ShopParent.transform);
        currentBloodCard = newShopCard1.GetComponent<UIScaleCard>();
        currentBloodCard.Init(eCardPolarity.Blood);
        yield return new WaitForSeconds(0.2f);
        var placementPosition2 = GetCellCenter(DeckPosition + new Vector2Int(1, 0));
        var newShopCard2 = Instantiate(ScaleCardPrefab, placementPosition2, Quaternion.identity, ShopParent.transform);
        currentHopeCard = newShopCard2.GetComponent<UIScaleCard>();
        currentHopeCard.Init(eCardPolarity.Hope);
    }

    internal void ToggleCardDisplay(bool v)
    {
        if (CardLookup.Count == 0)
        {
            return;
        }

        foreach (var item in CardLookup)
        {
            if (item.Value != null)
            {
                item.Value.ToggleVisibility(v);
            }
        }
    }

    public void AddRemovedHope(int v)
    {
        if (currentHopeCard == null)
        {
            return;
        }
        else
        {
            currentHopeCard.ChangeValue(v);
        }
    }

    internal Vector2Int GetRandomEmptyLocations(bool onBlood)
    {
        if (onBlood)
        {
            var listOfEmptyBlood = BloodTiles.Keys.ToList().FindAll(e => !CardLookup.ContainsKey(e));
            if (listOfEmptyBlood.Count > 0)
            {
                return listOfEmptyBlood[Game.Instance.rand.Next(listOfEmptyBlood.Count)];
            }
            else
            {
                return DeckPosition;
            }
        }
        return DeckPosition;
    }

    internal void PlayTrappedEffect()
    {
        StartCoroutine(TrappedDeck());
    }

    public IEnumerator TrappedDeck()
    {

        Vector2Int[] surroundingOffsets = new Vector2Int[]
{
            new Vector2Int(-1, 1), new Vector2Int(0, 1), new Vector2Int(1, 1),
            new Vector2Int(-1, 0),                         new Vector2Int(1, 0),
            new Vector2Int(-1, -1), new Vector2Int(0, -1), new Vector2Int(1, -1),
};
        var Tombstone = ((StoryData_Trapped)Game.Instance.CurrentStory).Tombstone;

        foreach (var item in surroundingOffsets)
        {
            yield return new WaitForSeconds(0.1f);
            var nextLocation = BoardManager.Instance.DeckPosition + item;
            BoardManager.Instance.PlayCard(Tombstone, nextLocation);
        }
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
        if (!BloodTiles.ContainsKey(position) && card != null &&
            !card.CurrentCard.Keywords.Contains(eCardKeyword.Painless) &&
            !card.CurrentCard.Keywords.Contains(eCardKeyword.StoryPainless))
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

    public bool TryPlaceCard(Vector3 mousePos, Card cardToPlace)
    {
        var Coords2D = GetNearestTile(mousePos);
        if (IsPlacementValid(Coords2D, cardToPlace.Keywords.Contains(eCardKeyword.Cathartic)))
        {
            var placementPosition = GetCellCenter(Coords2D);
            var newCard = Instantiate(NewCardPrefab, placementPosition, Quaternion.identity, CardsParent.transform);
            var uiCard = newCard.GetComponent<UIGameCard>();
            uiCard.Init(cardToPlace, Coords2D);
            PlaceCard(Coords2D, uiCard);
            return true;
        }
        return false;
    }

    public void PlayCard(Card cardData, Vector2Int Coords2D)
    {
        var placementPosition = GetCellCenter(Coords2D);
        var newCard = Instantiate(NewCardPrefab, placementPosition, Quaternion.identity, CardsParent.transform);
        var uiCard = newCard.GetComponent<UIGameCard>();
        uiCard.Init(cardData, Coords2D);
        PlaceCard(Coords2D, uiCard);
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
            newShopCard.GetComponent<UIEventCard>().Init(debugCard, DeckPosition + new Vector2Int(i, 0));
            yield return new WaitForSeconds(0.2f);

        }
    }

    public void FinishEvent()
    {
        StartCoroutine(CleanUpShopCards());
        Game.Instance.FinishEvent();
    }

    public void SubmitJudgement()
    {
        if (Game.Instance.TurnState != Game.eTurnState.Judgement)
        {
            return;
        }



        StartCoroutine(CleanUpShopCards());
        Game.Instance.SubmitJudgement();

    }

    public IEnumerator AwardBloodTokens()
    {
        yield return new WaitForSeconds( 1.2f + (BoardManager.Instance.CardLookup.Count + 2) * 0.2f);

        Game.Instance.GameUI.StartTrail();
    }

    public bool IsPlacementValid(Vector2Int placementPosition)
    {
        return IsPlacementValid(placementPosition, false);
    }


    public bool IsPlacementValid(Vector2Int placementPosition, bool cathartic)
    {
        if (CardLookup.ContainsKey(placementPosition))
        {
            return false;
        }

        if (cathartic)
        {
            if (BloodTiles.ContainsKey(placementPosition))
            {
                return false;
            }
        }

        bool adjecentToSomething = false;

        foreach (var item in AdjacencyLookup)
        {
            if (CardLookup.ContainsKey(placementPosition + item))
            {
                if (cathartic)
                {
                    adjecentToSomething = true;
                }
                else
                {
                    return true;
                }
            }
            else if (cathartic && BloodTiles.ContainsKey(placementPosition + item))
            {
                adjecentToSomething = true;
            }
        }


        if (cathartic && adjecentToSomething)
        {
            return true;
        }


        return false;
    }

    public IEnumerator CleanUpCards()
    {
        yield return new WaitForSeconds(1f);

        if (CardsParent.transform.childCount == 0)
        {
            yield return null;
        }

        List<UIGameCard> CardsToDestroy = new List<UIGameCard>();
        List<UIGameCard> CardsToKeep = new List<UIGameCard>();

        foreach (Transform card in CardsParent.transform)
        {
            var cardData = card.GetComponent<UIGameCard>();

            if (!cardData.CurrentCard.Keywords.Contains(eCardKeyword.Forgetful) &&
                !cardData.CurrentCard.Keywords.Contains(eCardKeyword.Power) &&
                !cardData.CurrentCard.Keywords.Contains(eCardKeyword.StoryKeep))
            {
                DeckManager.Instance.AddToDiscard(card.GetComponent<UICard>().CurrentCard);
            }

            if (!cardData.CurrentCard.Keywords.Contains(eCardKeyword.Power) &&
                !cardData.CurrentCard.Keywords.Contains(eCardKeyword.StoryKeep))
            {
                CardsToDestroy.Add(cardData);
            }
            else
            {
                if (cardData.CurrentCard.Keywords.Contains(eCardKeyword.StoryKeep))
                {
                    if (Game.Instance.EndOfRound)
                    {
                        CardsToDestroy.Add(cardData);
                    }
                    else
                    {
                        CardsToKeep.Add(cardData);

                    }
                }
                else
                {
                    CardsToKeep.Add(cardData);
                }
            }
        }

        foreach (UIGameCard card in CardsToDestroy)
        {
            Destroy(card.gameObject);
            AudioManager.Instance.PlaySound(SoundFX.CARD_BURN);
            yield return new WaitForSeconds(0.2f);
        }
        CardLookup.Clear();

        foreach (UIGameCard card in CardsToKeep)
        {
            CardLookup.Add(card.position, (UICard)card);
        }

        CardLookup.Add(DeckPosition, null);
    }

    public IEnumerator CleanUpShopCards()
    {
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
