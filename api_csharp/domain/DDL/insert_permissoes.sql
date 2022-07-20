/************************************************************
 * Code formatted by SoftTree SQL Assistant © v11.0.35
 * Time: 20/06/2022 00:32:25
 ************************************************************/

--DESATIVAR IDENTITY TABELA
SET IDENTITY_INSERT permissoes ON

INSERT INTO permissoes (id,nome, descricao, ativo)
VALUES 
(1, UPPER('manager'), '123456', 1),
(2, UPPER('employee'), '123456', 1);

--ATIVAR IDENTITY TABELA
SET IDENTITY_INSERT permissoes OFF

INSERT INTO usuarios_permissoes
(usuarioId, permissaoId, ativo)
VALUES
(1, 1, 1),(1, 2, 1);