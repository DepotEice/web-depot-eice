using AutoMapper;
using System.Text.RegularExpressions;
using Web.DepotEice.BLL.Models;
using Web.DepotEice.UIL.Helpers;
using Web.DepotEice.UIL.Models;
using Web.DepotEice.UIL.Models.Forms;

namespace Web.DepotEice.UIL.Profiles
{
    public partial class ArticleProfile : Profile
    {
        public ArticleProfile()
        {
            CreateMap<ArticleModel, ArticleForm>().
                ForMember(
                    dest => dest.Pinned,
                    opt => opt.MapFrom(
                        src => src.IsPinned));

            CreateMap<ArticleForm, ArticleCreateModel>();

            CreateMap<ArticleModel, ArticleIndexModel>()
                .ForMember(
                    dest => dest.MainImageUrl,
                    opt => opt.MapFrom(
                        src => $"{Utils.API_BASE_URL}/Files/ById/{src.MainImageId}"))
                .ForMember(
                    dest => dest.BodyNoHTML,
                    opt => opt.MapFrom(
                        src => LimitCharacters(RemoveParasitesFromBody(src.Body), 255)));

            CreateMap<ArticleModel, ArticleDetailsModel>();
        }

        private string RemoveParasitesFromBody(string body)
        {
            if (string.IsNullOrEmpty(body))
            {
                return body;
            }

            string removedImg = Utils.HTMLImgRegex.Replace(body, string.Empty);
            string removedKeys = Utils.HtmlEntityRemovalRegex.Replace(removedImg, string.Empty);

            return removedKeys;
        }

        private string LimitCharacters(string text, int limit)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            if (limit > text.Length)
            {
                limit = text.Length;
            }

            return $"{text[..limit]}...";
        }
    }
}
