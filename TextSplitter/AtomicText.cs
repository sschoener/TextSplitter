namespace TextSplitter
{
    public struct AtomicTextProvider : ITextProvider
    {
        public ITextProvider Provider { get; }
        public AtomicTextProvider(ITextProvider provider) {
            Provider = provider;
        }

        ITextPosition ITextProvider.GetStartPosition()
        {
            return new AtomicPosition(Provider.GetStartPosition());
        }

        private struct AtomicPosition : ITextPosition
        {
            private readonly ITextPosition _position;
            bool ITextPosition.IsAtEnd => false;

            public AtomicPosition(ITextPosition position) {
                _position = position;
            }

            (ITextChunk text, ITextPosition rest) ITextPosition.GetText(int maxLength)
            {
                var (chunk, next) = _position.GetText(maxLength);
                if (next.IsAtEnd)
                    return (chunk, EndPosition.Instance);
                return (EmptyChunk.Instance, this);
            }
        }
    }
}