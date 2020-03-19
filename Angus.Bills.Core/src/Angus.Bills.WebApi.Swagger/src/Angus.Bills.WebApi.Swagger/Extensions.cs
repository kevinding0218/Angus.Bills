using System;
using Angus.Bills.Docs.Swagger;
using Angus.Bills.Initializers;
using Angus.Bills.WebApi.Swagger.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Angus.Bills.WebApi.Swagger
{
    public static class Extensions
    {
        private const string SectionName = "swagger";

        public static IAngusBillsBuilder AddWebApiSwaggerDocs(this IAngusBillsBuilder builder,
            string sectionName = SectionName)
        {
            if (string.IsNullOrWhiteSpace(sectionName)) sectionName = SectionName;

            return builder.AddWebApiSwaggerDocs(b => b.AddSwaggerDocs(sectionName));
        }

        public static IAngusBillsBuilder AddWebApiSwaggerDocs(this IAngusBillsBuilder builder,
            Func<ISwaggerOptionsBuilder, ISwaggerOptionsBuilder> buildOptions)
        {
            return builder.AddWebApiSwaggerDocs(b => b.AddSwaggerDocs(buildOptions));
        }

        public static IAngusBillsBuilder AddWebApiSwaggerDocs(this IAngusBillsBuilder builder, SwaggerOptions options)
        {
            return builder.AddWebApiSwaggerDocs(b => b.AddSwaggerDocs(options));
        }

        private static IAngusBillsBuilder AddWebApiSwaggerDocs(this IAngusBillsBuilder builder,
            Action<IAngusBillsBuilder> registerSwagger)
        {
            registerSwagger(builder);
            builder.Services.AddSwaggerGen(c => c.DocumentFilter<WebApiDocumentFilter>());
            return builder;
        }
    }
}