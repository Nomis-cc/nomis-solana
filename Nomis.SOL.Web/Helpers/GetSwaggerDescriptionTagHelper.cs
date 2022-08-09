using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Swashbuckle.AspNetCore.Annotations;

namespace Nomis.SOL.Web.Helpers;

using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

[HtmlTargetElement("td", Attributes = "swagger-desc-for")]
public class GetSwaggerDescriptionTagHelper : TagHelper
{

    [HtmlAttributeName("swagger-desc-for")]
    public ModelExpression For { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {

        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        if (output == null)
        {
            throw new ArgumentNullException(nameof(output));
        }

        base.Process(context, output);

        var metadata = (DefaultModelMetadata) For.Metadata;
        if (metadata == null)
        {
            return;
        }

        var attribute = metadata.Attributes.Attributes.OfType<SwaggerSchemaAttribute>().FirstOrDefault();
        if (attribute == null)
        {
            return;
        }

        output.Content.SetHtmlContent(attribute.Description);
    }
}