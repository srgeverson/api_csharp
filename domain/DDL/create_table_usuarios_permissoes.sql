/************************************************************
 * Code formatted by SoftTree SQL Assistant � v11.0.35
 * Time: 19/06/2022 15:02:35
 ************************************************************/

CREATE TABLE usuarios_permissoes
(
	id        INT NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	usuarioId INT NOT NULL FOREIGN KEY REFERENCES usuarios(id),
	permissaoId INT NOT NULL FOREIGN KEY REFERENCES permissoes(id),
	ativo     TINYINT NOT NULL,
	dataCriacao DATETIME NOT NULL
);