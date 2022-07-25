/************************************************************
 * Code formatted by SoftTree SQL Assistant � v11.0.35
 * Time: 20/06/2022 00:32:25
 ************************************************************/

--DESATIVAR IDENTITY TABELA
SET IDENTITY_INSERT usuarios ON

INSERT INTO usuarios (id, nome, senha, ativo, dataCriacao)
VALUES (1, 'geverson', '123456', 1, GETDATE());

--ATIVAR IDENTITY TABELA
SET IDENTITY_INSERT usuarios OFF

DECLARE @count INT = 9;
DECLARE @index INT = 0;
WHILE @index < @count
BEGIN
    INSERT INTO usuarios
      (
        -- id -- this column value is auto-generated
        nome,
        senha,
        ativo,
        dataCriacao
      )
    VALUES
      (
        CAST('user' + CONVERT(VARCHAR(2), @index) AS VARCHAR(220)),
        NEWID(),
        CASE 
             WHEN @index % 2 = 0 THEN 1
             ELSE 0
        END,
        GETDATE() - @index
      )
    
    SET @index = @index + 1;
END  