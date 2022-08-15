/************************************************************
 * Code formatted by SoftTree SQL Assistant © v11.0.35
 * Time: 19/06/2022 21:07:39
 ************************************************************/

--Criando o Login
CREATE LOGIN [user_teste] 
WITH PASSWORD = '12345678';
GO
--Criando o Usuário para o Login
CREATE USER [user_teste] FOR LOGIN [user_teste];  
GO 
--Habilitando o Login
ALTER LOGIN [user_teste] ENABLE
GO
--Acessando DB
USE [db_teste];
GO
--Criando as permissões
GRANT INSERT TO [user_teste];
GO
GRANT DELETE TO [user_teste];
GO
GRANT UPDATE TO [user_teste];
GO
GRANT SELECT TO [user_teste];
GO
GRANT EXECUTE TO [user_teste];
GO
GRANT ALTER TO [user_teste];
GO
GRANT CREATE TABLE TO [user_teste];
GO
GRANT CREATE FUNCTION TO [user_teste];
GO
GRANT CREATE PROCEDURE TO [user_teste];
GO
GRANT CREATE VIEW TO [user_teste];
GO