using System;
using System.Web.Mvc;
using Mvp.Foundation.Content.Repositories;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;

namespace Mvp.Feature.Article.Controllers
{
    public class ArticleController : Controller
    {
        private readonly object _repository;   // BUG: wrong type — should be IArticleRepository

        public ArticleController(object repository)   // BUG: DI signature wrong
        {
            // BUG: Instead of validating the interface, we blindly store the object reference
            _repository = repository;
        }

        public ActionResult GetArticleDetails(string id)
        {
            Log.Info("[ArticleController] Fetching article details...", this);

            // BUG: Invalid cast — will throw InvalidCastException at runtime
            var repo = (IArticleRepository)_repository;

            // BUG: No validation of GUID → throws FormatException downstream
            var articleItem = repo.GetArticle(new Guid(id));

            // BUG: Not checking item or fields → NullReferenceException
            string title = articleItem["Title"];
            string body = articleItem.Fields["Body"].Value;

            // BUG: Using undefined variable "slug"
            var url = "/articles/" + slug;

            var model = new
            {
                Title = title,
                Body = body,
                Url = url
            };

            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}


// AI-Generated Fix:
24: var repo = (IArticleRepository)_repository;
27: var articleItem = repo.GetArticle(new Guid(id));