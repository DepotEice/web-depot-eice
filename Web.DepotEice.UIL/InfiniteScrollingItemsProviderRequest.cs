namespace Web.DepotEice.UIL
{
    public delegate Task<IEnumerable<T>> ItemsProvider<T>(InfiniteScrollingItemsProviderRequest request);

    public sealed class InfiniteScrollingItemsProviderRequest
    {
        public int StartIndex { get; }
        public CancellationToken CancellationToken { get; }

        public InfiniteScrollingItemsProviderRequest(int startIndex, CancellationToken cancellationToken)
        {
            StartIndex = startIndex;
            CancellationToken = cancellationToken;
        }
    }
}
