namespace crgolden.Abstractions
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.AspNet.OData.Builder;
    using Microsoft.AspNetCore.Mvc;

    [ExcludeFromCodeCoverage]
    public abstract class ModelConfiguration<TModel> : IModelConfiguration
        where TModel : Model
    {
        protected abstract EntityTypeConfiguration<TModel> ConfigureCurrent(ODataModelBuilder builder);

        public virtual void Apply(ODataModelBuilder builder, ApiVersion apiVersion)
        {
            switch (apiVersion.MajorVersion)
            {
                case 1:
                default:
                    ConfigureCurrent(builder);
                    break;
            }
        }
    }
}
