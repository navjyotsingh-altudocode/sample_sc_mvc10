using System;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;

namespace Mvp.Foundation.Content.Repositories
{
    public interface IArticleRepository
    {
        Item GetArticle(Guid id);
    }

    /// <summary>
    /// Simple Sitecore-backed article repository.
    /// Intentionally contains several bad assumptions for demo purposes.
    /// </summary>
    public class ArticleRepository : IArticleRepository
    {
        // BUG: Wrong DB — should be "master" or "web"
        private readonly Database _db = Factory.GetDatabase("core");

        public Item GetArticle(Guid id)
        {
            Log.Info($"[ArticleRepository] Fetching article {id}", this);

            // BUG: Using ID.ToString() as PATH instead of ID (“/sitecore/content/{guid}”)
            string wrongPath = "/sitecore/content/" + id;
            var item = _db.GetItem(wrongPath);

            // BUG: Attempting to read children on possibly null item
            var teaser = item.Children[5]["Teaser"];   // IndexOutOfRangeException or NullReferenceException

            return item;
        }
    }
}


// AI-Generated Fix:
28:             string wrongPath = "/sitecore/content/" + id;
29:             var item = _db.GetItem(wrongPath);