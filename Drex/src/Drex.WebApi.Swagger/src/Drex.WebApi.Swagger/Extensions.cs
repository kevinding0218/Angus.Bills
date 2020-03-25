using System;
using Drex.Docs.Swagger;
using Drex.Initializers;
using Drex.WebApi.Swagger.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Drex.WebApi.Swagger
{
    public static class Extensions
    {
        private const string SectionName = "swagger";

        public static IDrexBuilder AddWebApiSwaggerDocs(this IDrexBuilder builder,
            string sectionName = SectionName)
        {
            if (string.IsNullOrWhiteSpace(sectionName)) sectionName = SectionName;

            return builder.AddWebApiSwaggerDocs(b => b.AddSwaggerDocs(sectionName));
        }

        public static IDrexBuilder AddWebApiSwaggerDocs(this IDrexBuilder builder,
            Func<ISwaggerOptionsBuilder, ISwaggerOptionsBuilder> buildOptions)
        {
            return builder.AddWebApiSwaggerDocs(b => b.AddSwaggerDocs(buildOptions));
        }

        public static IDrexBuilder AddWebApiSwaggerDocs(this IDrexBuilder builder, SwaggerOptions options)
        {
            return builder.AddWebApiSwaggerDocs(b => b.AddSwaggerDocs(options));
        }

        private static IDrexBuilder AddWebApiSwaggerDocs(this IDrexBuilder builder,
            Action<IDrexBuilder> registerSwagger)
        {
            registerSwagger(builder);
            builder.Services.AddSwaggerGen(c => c.DocumentFilter<WebApiDocumentFilter>());
            return builder;
        }
    }
}