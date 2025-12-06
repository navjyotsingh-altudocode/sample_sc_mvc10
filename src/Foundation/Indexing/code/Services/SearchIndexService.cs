using System;
using System.Net.Http;
using System.Threading.Tasks;
using Sitecore.Diagnostics;

namespace Mvp.Foundation.Indexing.Services
{
    public interface ISearchIndexService
    {
        string SearchArticles(string query);
    }

    /// <summary>
    /// Very simple Solr-backed search service with intentionally bad patterns.
    /// </summary>
    public class SearchIndexService : ISearchIndexService
    {
        private readonly HttpClient _client;

        public SearchIndexService(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));

            // BUG: Over-aggressive timeout
            _client.Timeout = TimeSpan.FromMilliseconds(100);
        }

        public string SearchArticles(string query)
        {
            Log.Info("[SearchIndexService] Running Solr search...", this);

            // BUG: Not encoding the query string -> fails on spaces / special chars
            var url = "/solr/articles/select?q=" + query + "&wt=json";

            // BUG: Calling .Result → blocks thread → potential deadlock in ASP.NET
            var response = _client.GetAsync(url).Result;

            // BUG: Blindly throwing generic Exception with no context object
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Solr returned bad status: " + response.StatusCode);
            }

            // BUG: Using .Result again → repeated blocking
            var content = response.Content.ReadAsStringAsync().Result;

            return content;
        }
    }
}


// AI-Generated Fix:
32:             // BUG: Not encoding the query string -> fails on spaces / special chars
33:             var url = "/solr/articles/select?q=" + query + "&wt=json";