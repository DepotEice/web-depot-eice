using AutoMapper;
using Web.DepotEice.BLL.Models;
using Web.DepotEice.UIL.Models.Forms;

namespace Web.DepotEice.UIL.Profiles
{
    public class ArticleProfile : Profile
    {
        public ArticleProfile()
        {
            CreateMap<ArticleModel, ArticleForm>().
                ForMember(
                    dest => dest.Pinned,
                    opt => opt.MapFrom(
                        src => src.IsPinned));
        }
    }
}
