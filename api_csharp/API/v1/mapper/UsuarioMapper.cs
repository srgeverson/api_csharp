using api_csharp.API.v1.model;
using AutoMapper;
using domain.model;

namespace api_csharp.API.v1.mapper
{
    public class UsuarioMapper : Profile
    {
        public UsuarioResponse ToResponse(Usuario? usuario)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Usuario, UsuarioResponse>());
            var mapper = new Mapper(config);
            return mapper.Map<UsuarioResponse>(usuario);
        }

        public List<UsuarioResponse> ToListResponse(List<Usuario> usuarios)
        {
            var usuarioResponses = new List<UsuarioResponse>();
            usuarios.ForEach(usuario => usuarioResponses.Add(ToResponse(usuario)));
            return usuarioResponses;
        }
    }
}
