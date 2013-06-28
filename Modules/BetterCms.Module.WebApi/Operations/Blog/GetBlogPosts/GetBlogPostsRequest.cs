﻿using System.Runtime.Serialization;

using BetterCms.Module.Api.Operations.Enums;

namespace BetterCms.Module.Api.Operations.Blog.GetBlogPosts
{
    [DataContract]
    public class GetBlogPostsRequest : RequestBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetBlogPostsRequest" /> class.
        /// </summary>
        public GetBlogPostsRequest()
        {
            FilterByTagsConnector = FilterConnector.And;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to include unpublished blog posts.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include unpublished blog posts; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Order = 10, Name = "includeUnpublished")]
        public bool IncludeUnpublished { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include archived blog posts.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to include archived blog posts; otherwise, <c>false</c>.
        /// </value>
        [DataMember(Order = 20, Name = "includeArchived")]
        public bool IncludeArchived { get; set; }

        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        /// <value>
        /// The tags.
        /// </value>
        [DataMember(Order = 50, Name = "filterByTags")]
        public System.Collections.Generic.List<string> FilterByTags { get; set; }

        /// <summary>
        /// Gets or sets the tags filter connector.
        /// </summary>
        /// <value>
        /// The tags filter connector.
        /// </value>
        [DataMember(Order = 60, Name = "filterByTagsConnector")]
        public FilterConnector FilterByTagsConnector { get; set; }
    }
}