using BetterCms.Core.Models;

namespace BetterCms.Module.Root.Models.Maps
{
    public class PageOptionMap : EntityMapBase<PageOption>
    {
        public PageOptionMap() : base(RootModuleDescriptor.ModuleName)
        {
            Table("PageOptions");

            Map(x => x.Key, "[Key]").Length(MaxLength.Name).Not.Nullable();
            Map(x => x.Type).Not.Nullable();
            Map(x => x.Value).Length(MaxLength.Max).Nullable().LazyLoad();         
            
            References(x => x.Page).Cascade.SaveUpdate().LazyLoad();
        }
    }
}