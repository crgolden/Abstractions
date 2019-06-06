namespace crgolden.Abstractions
{
    using AutoMapper;

    public class EntityProfile : Profile
    {
        public EntityProfile()
        {
            CreateMap<Model, Entity>(MemberList.Destination)
                .IncludeAllDerived()
                .ForMember(dest => dest.Created, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
