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

        public UsuarioLoginResponse ToLoginResponse(Usuario? usuario)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Usuario, UsuarioResponse>());
            var mapper = new Mapper(config);
            return mapper.Map<UsuarioLoginResponse>(usuario);
        }

        public List<UsuarioResponse> ToListResponse(List<Usuario> usuarios)
        {
            var usuarioResponses = new List<UsuarioResponse>();
            usuarios.ForEach(usuario => usuarioResponses.Add(ToResponse(usuario)));
            return usuarioResponses;
        }

        public Usuario ToModel(UsuarioRequest? usuarioRequest)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UsuarioRequest, Usuario>()
                    .ForMember(dest => dest.Ativo, act => act.Ignore());
            });
            var mapper = new Mapper(config);
            return mapper.Map<Usuario>(usuarioRequest);
        }

        public List<Usuario> ToListModel(List<UsuarioRequest> usuarioRequests)
        {
            var usuarios = new List<Usuario>();
            usuarioRequests.ForEach(usuarioRequest => usuarios.Add(ToModel(usuarioRequest)));
            return usuarios;
        }
    }
}
