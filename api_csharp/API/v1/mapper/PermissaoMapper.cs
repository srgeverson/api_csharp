using api_csharp.API.v1.model;
using AutoMapper;
using domain.model;

namespace api_csharp.API.v1.mapper
{
    public class PermissaoMapper : Profile
    {
        public PermissaoResponse ToResponse(Permissao? permissao)
        {
            var mapperConfiguration = new MapperConfiguration(onfiguration => onfiguration.CreateMap<Permissao, PermissaoResponse>());
            var mapper = new Mapper(mapperConfiguration);
            return mapper.Map<PermissaoResponse>(permissao);
        }

        public List<PermissaoResponse> ToListResponse(List<Permissao> permissaoes)
        {
            var permissaoResponses = new List<PermissaoResponse>();
            permissaoes.ForEach(permissao => permissaoResponses.Add(ToResponse(permissao)));
            return permissaoResponses;
        }
    }
}
