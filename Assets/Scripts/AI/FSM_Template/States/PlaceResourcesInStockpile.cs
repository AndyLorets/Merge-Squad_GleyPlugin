internal class PlaceResourcesInStockpile : AIState
{
    private readonly Gatherer _gatherer;

    public PlaceResourcesInStockpile(Gatherer gatherer)
    {
        _gatherer = gatherer;
    }

    public override void Tick()
    {
        if (_gatherer.Take())
            _gatherer.StockPile.Add();
    }

    public override void OnEnter() { }

    public override void OnExit() { }
}