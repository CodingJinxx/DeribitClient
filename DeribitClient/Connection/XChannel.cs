using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace DeribitClient
{
    public class XChannel<T>
    {
        private readonly Channel<T> _channel;
        private readonly CancellationTokenSource _tokenSource;
        private readonly ChannelReader<T> _reader;

        public XChannel()
        {
            this._channel = Channel.CreateUnbounded<T>();
            this._tokenSource = new CancellationTokenSource();
            this._reader = this._channel.Reader;
        }

        public async void Write(T obj)
        {
            await this._channel.Writer.WriteAsync(obj, this._tokenSource.Token);
        }

        public async ValueTask<T> Read()
        {
            return await this._reader.ReadAsync(this._tokenSource.Token);
        }

        public bool TryRead(out T item)
        {
            if (this._reader.TryRead(out var i))
            {
                item = i;
                return true;
            }

            item = default;
            return false;
        }

        public void CancelChannel()
        {
            this._tokenSource.Cancel();
        }
    }
}