using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace BACnet.Core
{
    public class SearchList<TKey, TResult>
    {
        /// <summary>
        /// Lock synchronizing access to the search list
        /// </summary>
        private readonly object _lock = new object();

        /// <summary>
        /// The search handler
        /// </summary>
        private ISearchHandler<TKey> _handler;

        /// <summary>
        /// The list of requested searches
        /// </summary>
        private LinkedList<SearchRequest> _requests;

        /// <summary>
        /// The search attempts that are active
        /// </summary>
        private LinkedList<SearchAttempt> _attempts;

        /// <summary>
        /// A temporary collection of search requests
        /// </summary>
        private List<SearchRequest> _tempRequests;

        /// <summary>
        /// Comparer used to compare key instances
        /// </summary>
        private IComparer<TKey> _keyComparer;

        /// <summary>
        /// The maximum number of retries
        /// for a search
        /// </summary>
        private int _maxRetries;

        /// <summary>
        /// The timeout period for a search attempt
        /// </summary>
        private TimeSpan _searchTimeout;

        /// <summary>
        /// Constructs a new search list instance
        /// </summary>
        public SearchList(ISearchHandler<TKey> handler)
        {
            _handler = handler;
            _requests = new LinkedList<SearchRequest>();
            _attempts = new LinkedList<SearchAttempt>();
            _tempRequests = new List<SearchRequest>();
            _maxRetries = 2;
            _keyComparer = Comparer<TKey>.Default;
        }

        /// <summary>
        /// Collects all of the requests for a specific key, removing
        /// them from the requests list
        /// </summary>
        /// <param name="key">The key to collect requests for</param>
        /// <returns>The array of requests</returns>
        private SearchRequest[] _collectRequests(TKey key)
        {
            SearchRequest[] ret = null;

            for(var node = _requests.First; node != null;)
            {
                if (_keyComparer.Compare(key, node.Value.Key) == 0)
                {
                    var temp = node.Next;
                    _tempRequests.Add(node.Value);
                    _requests.Remove(node);
                    node = temp;
                }
                else
                    node = node.Next;
            }

            ret = _tempRequests.Count == 0 ? null :  _tempRequests.ToArray();
            _tempRequests.Clear();
            return ret;
        }

        /// <summary>
        /// Called when a search attempt timer ticks
        /// </summary>
        private void _searchTick(object state)
        {
            SearchAttempt attempt = (SearchAttempt)state;
            TKey key;
            bool timeout = false;
            SearchRequest[] requests = null;

            lock(_lock)
            {
                attempt.attempt++;
                key = attempt.Key;
                if (attempt.attempt > _maxRetries)
                {
                    timeout = true;
                    requests = _collectRequests(attempt.Key);
                }
            }

            if (!timeout)
                _handler.DoSearch(key);
            else if(requests != null && timeout)
            {
                foreach (var request in requests)
                    request.Callback.OnTimeout(key);
            }
        }

        /// <summary>
        /// Called by the search handler whenever a result is found
        /// </summary>
        /// <param name="key">The key of the result</param>
        /// <param name="result">The result</param>
        public void ResultFound(TKey key, TResult result)
        {
            SearchRequest[] requests = null;

            lock(_lock)
            {
                requests = _collectRequests(key);
            }

            if(requests != null)
            {
                foreach (var request in requests)
                    request.Callback.OnFound(result);
            }
        }

        private struct SearchRequest
        {
            /// <summary>
            /// The key that is being searched for
            /// </summary>
            public TKey Key { get; private set; }

            /// <summary>
            /// The callback instance for the search
            /// </summary>
            public ISearchCallback<TKey, TResult> Callback { get; private set; }

            public SearchRequest(TKey key, ISearchCallback<TKey, TResult> callback) : this()
            {
                this.Key = key;
                this.Callback = callback;
            }
        }

        private class SearchAttempt : IDisposable
        {
            /// <summary>
            /// The key that is being searched for
            /// </summary>
            public TKey Key { get; private set; }

            /// <summary>
            /// The current search attempt
            /// </summary>
            public int Attempt { get { return attempt; } }

            /// <summary>
            /// The current search attempt
            /// </summary>
            internal int attempt;

            /// <summary>
            /// The timer that is being used for this search
            /// </summary>
            internal Timer timer;

            /// <summary>
            /// Constructs a new search attempt instance
            /// </summary>
            /// <param name="list">The search list</param>
            /// <param name="key">The key to search for</param>
            public SearchAttempt(SearchList<TKey, TResult> list, TKey key)
            {
                this.Key = key;
                this.attempt = 0;
                this.timer = new Timer(
                    list._searchTick,
                    this,
                    TimeSpan.Zero,
                    list._searchTimeout);
            }

            /// <summary>
            /// Disposes of all resources held by the search attempt
            /// </summary>
            public void Dispose()
            {
                if (timer != null)
                {
                    timer.Dispose();
                    timer = null;
                }
            }
        }
    }

    public interface ISearchHandler<TKey>
    {
        /// <summary>
        /// Informs the handler that a search should
        /// be performed for a specific key
        /// </summary>
        /// <param name="key">The key to search for</param>
        void DoSearch(TKey key);
    }

    public interface ISearchCallback<TKey, TResult>
    {
        /// <summary>
        /// Called when a result is found for a specific key
        /// </summary>
        /// <param name="result">The result that was found</param>
        void OnFound(TResult result);

        /// <summary>
        /// Called when a search times out
        /// </summary>
        /// <param name="key">The key of the search that timed out</param>
        void OnTimeout(TKey key);
    }
}
