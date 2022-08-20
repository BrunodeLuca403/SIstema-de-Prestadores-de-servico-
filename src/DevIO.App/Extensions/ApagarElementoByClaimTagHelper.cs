using Microsoft.AspNetCore.Razor.TagHelpers;

namespace DevIO.App.Extensions
{
    [HtmlTargetElement("*", Attributes = "supress-by-claim-name")]
    [HtmlTargetElement("*", Attributes = "supress-by-claim-value")]
    public class ApagarElementoByClaimTagHelper : TagHelper
    {
        private readonly IHttpContextAccessor _contextAcessor;

        public ApagarElementoByClaimTagHelper(IHttpContextAccessor contextAcessor)
        {
            _contextAcessor = contextAcessor;    
        }

        [HtmlAttributeName("supress-by-claim-name")]
        public string IdentityClaimName { get; set; }

        [HtmlAttributeName("supress-by-claim-value")]
        public string IdentityClaimValue { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            if (output == null) throw new ArgumentNullException(nameof(output));

            var temAcesso = CustomAuthorization.ValidarClaimsUsuario(_contextAcessor.HttpContext, IdentityClaimName, IdentityClaimValue);

            if (temAcesso) return;

            output.SuppressOutput();    
        }
    }

    [HtmlTargetElement("a", Attributes = "supress-by-claim-name")]
    [HtmlTargetElement("a", Attributes = "supress-by-claim-value")]
    public class DesabilitarLinkByClaimTagHelper : TagHelper
    {
        private readonly IHttpContextAccessor _contextAcessor;

        public DesabilitarLinkByClaimTagHelper(IHttpContextAccessor contextAcessor)
        {
            _contextAcessor = contextAcessor;
        }

        [HtmlAttributeName("supress-by-claim-name")]
        public string IdentityClaimName { get; set; }

        [HtmlAttributeName("supress-by-claim-value")]
        public string IdentityClaimValue { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            if (output == null) throw new ArgumentNullException(nameof(output));

            var temAcesso = CustomAuthorization.ValidarClaimsUsuario(_contextAcessor.HttpContext, IdentityClaimName, IdentityClaimValue);

            if (temAcesso) return;

            output.Attributes.RemoveAll("href");
            output.Attributes.Add(new TagHelperAttribute("style", "cursor: not-allowed"));
            output.Attributes.Add(new TagHelperAttribute("title", "você não tem permissão"));
        }
    }
}
