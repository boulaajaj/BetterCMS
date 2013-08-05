﻿using System;
using System.Linq;

using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.MediaManager.ViewModels;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.Page;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Pages.Command.Page.GetPageProperties
{
    /// <summary>
    /// Command for getting view model with page properties
    /// </summary>
    public class GetPagePropertiesCommand : CommandBase, ICommand<Guid, EditPagePropertiesViewModel>
    {
        /// <summary>
        /// The category service
        /// </summary>
        private ICategoryService categoryService;

        /// <summary>
        /// The tag service
        /// </summary>
        private readonly ITagService tagService;

        /// <summary>
        /// The options service
        /// </summary>
        private readonly IOptionService optionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetPagePropertiesCommand" /> class.
        /// </summary>
        /// <param name="tagService">The tag service.</param>
        /// <param name="categoryService">The category service.</param>
        /// <param name="optionService">The option service.</param>
        public GetPagePropertiesCommand(ITagService tagService, ICategoryService categoryService, IOptionService optionService)
        {
            this.tagService = tagService;
            this.categoryService = categoryService;
            this.optionService = optionService;
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="id">The page id.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public EditPagePropertiesViewModel Execute(Guid id)
        {
            var model = Repository
                .AsQueryable<PageProperties>()
                .Where(p => p.Id == id)
                .Select(page =>
                    new
                    {
                        Model = new EditPagePropertiesViewModel
                          {
                              Id = page.Id,
                              Version = page.Version,
                              PageName = page.Title,
                              PageUrl = page.PageUrl,
                              PageCSS = page.CustomCss,
                              PageJavascript = page.CustomJS,
                              UseNoFollow = page.UseNoFollow,
                              UseNoIndex = page.UseNoIndex,
                              UseCanonicalUrl = page.UseCanonicalUrl,
                              IsVisibleToEveryone = page.Status == PageStatus.Published,
                              IsInSitemap = page.NodeCountInSitemap > 0,
                              IsArchived = page.IsArchived,
                              TemplateId = page.Layout.Id,
                              CategoryId = page.Category.Id,
                              Image = page.Image == null ? null :
                                  new ImageSelectorViewModel
                                          {
                                              ImageId = page.Image.Id,
                                              ImageVersion = page.Image.Version,
                                              ImageUrl = page.Image.PublicUrl,
                                              ThumbnailUrl = page.Image.PublicThumbnailUrl,
                                              ImageTooltip = page.Image.Caption
                                          },
                              SecondaryImage = page.SecondaryImage == null ? null :
                                  new ImageSelectorViewModel
                                          {
                                              ImageId = page.SecondaryImage.Id,
                                              ImageVersion = page.SecondaryImage.Version,
                                              ImageUrl = page.SecondaryImage.PublicUrl,
                                              ThumbnailUrl = page.SecondaryImage.PublicThumbnailUrl,
                                              ImageTooltip = page.SecondaryImage.Caption
                                          },
                              FeaturedImage = page.FeaturedImage == null ? null :
                                  new ImageSelectorViewModel
                                          {
                                              ImageId = page.FeaturedImage.Id,
                                              ImageVersion = page.FeaturedImage.Version,
                                              ImageUrl = page.FeaturedImage.PublicUrl,
                                              ThumbnailUrl = page.FeaturedImage.PublicThumbnailUrl,
                                              ImageTooltip = page.FeaturedImage.Caption
                                          }
                          }
                    })
                .FirstOne();

            if (model != null && model.Model != null)
            {
                model.Model.Tags = tagService.GetPageTagNames(id);
                model.Model.RedirectFromOldUrl = true;
                model.Model.Categories = categoryService.GetCategories();
                model.Model.UpdateSitemap = true;

                var fakePage = new Root.Models.Page { Layout = new Root.Models.Layout() };
                fakePage.Layout.LayoutOptions = Repository
                    .AsQueryable<LayoutOption>(lo => lo.Layout.Id == model.Model.TemplateId)
                    .ToList();
                fakePage.Options = Repository
                    .AsQueryable<PageOption>()
                    .ToList();

                optionService.SetOptionValues(model.Model, fakePage, fakePage);
            }

            return model != null ? model.Model : null;
        }
    }
}