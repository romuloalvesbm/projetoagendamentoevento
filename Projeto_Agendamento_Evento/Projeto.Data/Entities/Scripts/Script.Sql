USE MASTER
GO
DROP DATABASE "DB_AGE";
CREATE DATABASE "DB_AGE" 
GO
USE "DB_AGE";
GO

/*---------------------------------------------------------------------------------------
				TABELA DE AREA - AREA
---------------------------------------------------------------------------------------*/
CREATE TABLE AREA(
IDAREA		INT IDENTITY(1,1) NOT NULL,
NOME		VARCHAR(50) NOT NULL,
DESATIVAR	VARCHAR(1),
CONSTRAINT PK_AREA PRIMARY KEY(IDAREA),
CONSTRAINT UQ_AREA_NOME UNIQUE (NOME)
)
GO
/*----------------------------------FIM------------------------------------------------*/

/*---------------------------------------------------------------------------------------
				TABELA DE LOCALIDADE - LOCALIDADE
---------------------------------------------------------------------------------------*/
CREATE TABLE LOCALIDADE(
IDLOCALIDADE		INT IDENTITY(1,1) NOT NULL,
NOME				VARCHAR(50) NOT NULL,
DESATIVAR			VARCHAR(1),
CONSTRAINT PK_LOCALIDADE PRIMARY KEY(IDLOCALIDADE),
CONSTRAINT UQ_LOCALIDADE_NOME UNIQUE (NOME)
)
GO
/*----------------------------------FIM------------------------------------------------*/

/*---------------------------------------------------------------------------------------
						TABELA DE EVENTO - EVENTO
---------------------------------------------------------------------------------------*/
CREATE TABLE EVENTO(
IDEVENTO			INT IDENTITY(1,1) NOT NULL,
NOME				VARCHAR(50) NOT NULL,
DESCRICAO			NVARCHAR(MAX),
DESATIVAR			VARCHAR(1),
CONSTRAINT PK_EVENTO PRIMARY KEY(IDEVENTO),
CONSTRAINT UQ_EVENTO_NOME UNIQUE (NOME)
)
GO

/*---------------------------------------------------------------------------------------
				TABELA DE SALA - SALA
---------------------------------------------------------------------------------------*/
CREATE TABLE SALA(
IDSALA				INT IDENTITY(1,1) NOT NULL,
IDLOCALIDADE		INT NOT NULL,
NOME				VARCHAR(30) NOT NULL,
DESATIVAR			VARCHAR(1),
CONSTRAINT PK_SALA PRIMARY KEY(IDSALA),
CONSTRAINT FK_SALA_LOCAL	FOREIGN KEY (IDLOCALIDADE) REFERENCES LOCALIDADE(IDLOCALIDADE),
CONSTRAINT UQ_SALA_LOCALIDADE UNIQUE (IDLOCALIDADE, NOME)
)
GO
/*----------------------------------FIM------------------------------------------------*/

/*----------------------------------FIM------------------------------------------------*/

/*---------------------------------------------------------------------------------------
						TABELA DE AGENDAMENTO_TURMA - AGENDAMENTO_TURMA
---------------------------------------------------------------------------------------*/
CREATE TABLE AGENDAMENTO_TURMA(
IDAGETURMA		INT IDENTITY(1,1) NOT NULL,
IDEVENTO			INT NOT NULL,
IDAREA				INT NOT NULL,
IDSALA				INT NOT NULL,
TURMA				VARCHAR(30) NOT NULL,
DATA				DATETIME NOT NULL,
HORA_INICIO			TIME NOT NULL,
HORA_FIM			TIME NOT NULL,
DATA_LIMITE			DATETIME NOT NULL,
MAX_PARTICIPANTE	INT NOT NULL,
STATUS				VARCHAR(30) NOT NULL,
CONSTRAINT PK_AGENTURMA PRIMARY KEY(IDAGETURMA),
CONSTRAINT FK_AGENTURMA_EVENTO	FOREIGN KEY (IDEVENTO) REFERENCES EVENTO(IDEVENTO),
CONSTRAINT FK_AGENTURMA_AREA	FOREIGN KEY (IDAREA) REFERENCES AREA(IDAREA),
CONSTRAINT FK_AGENTURMA_SALA	FOREIGN KEY (IDSALA) REFERENCES SALA(IDSALA),
)
GO
/*----------------------------------FIM------------------------------------------------*/

/*---------------------------------------------------------------------------------------
				TABELA DE PERFIL - PERFIL
---------------------------------------------------------------------------------------*/
CREATE TABLE PERFIL(
    IDPERFIL			INT IDENTITY (1, 1) NOT NULL,
    DESCRICAO			VARCHAR (50) NOT NULL,
    BLOQUEADO			VARCHAR (1) DEFAULT ('N') NULL,
    DESATIVAR			VARCHAR(1),
    CONSTRAINT PK_PERFIL PRIMARY KEY (IDPERFIL)
);
GO
/*----------------------------------FIM------------------------------------------------*/

/*---------------------------------------------------------------------------------------
				TABELA DE COLABORADOR - COLABORADOR
---------------------------------------------------------------------------------------*/
CREATE TABLE COLABORADOR (
 IDCOLABORADOR	INT IDENTITY(1,1) NOT NULL,
 IDPERFIL       INT,
 CHAPA			VARCHAR(7) NOT NULL,
 NOME			VARCHAR(100) NOT NULL,
 USUARIO		VARCHAR(30) NULL,
 EMAIL			VARCHAR(50) NOT NULL,
 RAMAL			VARCHAR(20),
 SENHA          VARCHAR(50),
CONSTRAINT PK_COLABORADOR PRIMARY KEY(IDCOLABORADOR),
CONSTRAINT UQ_COLABORADOR_CHAPA UNIQUE (CHAPA),
CONSTRAINT FK_COLABORADOR_PERFIL	FOREIGN KEY (IDPERFIL) REFERENCES PERFIL(IDPERFIL),
)
GO
CREATE UNIQUE INDEX UQ_COLABORADOR_USUARIO ON COLABORADOR(USUARIO) WHERE USUARIO IS NOT NULL
GO
/*----------------------------------FIM------------------------------------------------*/

/*---------------------------------------------------------------------------------------
				TABELA DE AGENDAMENTO_COLABORADOR - AGENDAMENTO_COLABORADOR
---------------------------------------------------------------------------------------*/
CREATE TABLE AGENDAMENTO_COLABORADOR(
IDAGECOL			INT IDENTITY(1,1) NOT NULL,
IDAGETURMA			INT NOT NULL,
IDCOLABORADOR		INT NOT NULL,
CONSTRAINT PK_AGECOLAB PRIMARY KEY(IDAGECOL),
CONSTRAINT UQ_AGECOLAB_TURMA_COLAB UNIQUE (IDAGETURMA, IDCOLABORADOR),
CONSTRAINT FK_AGECOL_COLABORADOR	FOREIGN KEY (IDCOLABORADOR) REFERENCES COLABORADOR(IDCOLABORADOR),
CONSTRAINT FK_AGECOL_AGETURMA	FOREIGN KEY (IDAGETURMA) REFERENCES AGENDAMENTO_TURMA(IDAGETURMA),
)
GO

/*---------------------------------------------------------------------------------------
					TABELA DE PERMISSAO - PERMISSAO
---------------------------------------------------------------------------------------*/
CREATE TABLE PERMISSAO(
    IDPERMISSAO			INT IDENTITY (1, 1) NOT NULL,
    DESCRICAO			VARCHAR (50) NOT NULL,
    BLOQUEADO			VARCHAR (1)     DEFAULT ('N') NULL,
    IDPAI				INT NULL,
    CATEGORIA           VARCHAR(100),
    CONSTRAINT PK_PERMISSAO PRIMARY KEY (IDPERMISSAO),
    CONSTRAINT FK_PERMISSAO	FOREIGN KEY (IDPAI) REFERENCES PERMISSAO(IDPERMISSAO),
);
GO
/*----------------------------------FIM------------------------------------------------*/
/*---------------------------------------------------------------------------------------
					TABELA DE PERFIL_PERMISSAO - PERFIL_PERMISSAO
---------------------------------------------------------------------------------------*/
CREATE TABLE PERFIL_PERMISSAO (
    ID				INT IDENTITY (1, 1) NOT NULL,
    IDPERFIL		INT NOT NULL,
    IDPERMISSAO		INT NOT NULL,
    CONSTRAINT PK_PERFIL_PERMISSAO PRIMARY KEY (ID),
    CONSTRAINT UQ_PERFIL_PERMISSAO UNIQUE (IDPERFIL, IDPERMISSAO),
    CONSTRAINT FK_PERFIL_PERMISSAO_PERFIL FOREIGN KEY (IDPERFIL) REFERENCES PERFIL(IDPERFIL) ON DELETE CASCADE,
    CONSTRAINT FK_PERFIL_PERMISSAO_PERMISSAO FOREIGN KEY (IDPERMISSAO) REFERENCES PERMISSAO(IDPERMISSAO)
);
GO
/*---------------------------------------------------------------------------------------*/

/*---------------------------------------------------------------------------------------
							INSERT PERFIL
---------------------------------------------------------------------------------------*/

INSERT INTO PERFIL (DESCRICAO) VALUES ('Administrador');

/*----------------------------------FIM------------------------------------------------*/

/*---------------------------------------------------------------------------------------
							INSERT COLABORADOR
---------------------------------------------------------------------------------------*/

INSERT INTO COLABORADOR(IDPERFIL, CHAPA, USUARIO, NOME, EMAIL, RAMAL) VALUES (1,'310008', 'DML20AH', 'Rômulo de Souza Alves', 'ralves@grigroup.com.br', '1234');

/*----------------------------------FIM------------------------------------------------*/

/*----------------------------------FIM------------------------------------------------*/

/*---------------------------------------------------------------------------------------
								INSERT PERMISSAO
---------------------------------------------------------------------------------------*/

INSERT INTO permissao (DESCRICAO, BLOQUEADO, IDPAI) VALUES ('Area', 'N', NULL); /*1*/
INSERT INTO permissao (DESCRICAO, BLOQUEADO, IDPAI) VALUES ('Cadastrar Area', 'N', 1);
INSERT INTO permissao (DESCRICAO, BLOQUEADO, IDPAI) VALUES ('Alterar Area', 'N', 1);
INSERT INTO permissao (DESCRICAO, BLOQUEADO, IDPAI) VALUES ('Excluir Area', 'N', 1);

INSERT INTO permissao (DESCRICAO, BLOQUEADO, IDPAI) VALUES ('Evento', 'N', NULL); /*5*/
INSERT INTO permissao (DESCRICAO, BLOQUEADO, IDPAI) VALUES ('Cadastrar Evento', 'N', 5);
INSERT INTO permissao (DESCRICAO, BLOQUEADO, IDPAI) VALUES ('Alterar Evento', 'N', 5);
INSERT INTO permissao (DESCRICAO, BLOQUEADO, IDPAI) VALUES ('Excluir Evento', 'N', 5);

INSERT INTO permissao (DESCRICAO, BLOQUEADO, IDPAI) VALUES ('Localidade', 'N', NULL); /*9*/
INSERT INTO permissao (DESCRICAO, BLOQUEADO, IDPAI) VALUES ('Cadastrar Localidade', 'N', 9);
INSERT INTO permissao (DESCRICAO, BLOQUEADO, IDPAI) VALUES ('Alterar Localidade', 'N', 9);
INSERT INTO permissao (DESCRICAO, BLOQUEADO, IDPAI) VALUES ('Excluir Localidade', 'N', 9);

INSERT INTO permissao (DESCRICAO, BLOQUEADO, IDPAI) VALUES ('Sala', 'N', NULL); /*13*/
INSERT INTO permissao (DESCRICAO, BLOQUEADO, IDPAI) VALUES ('Cadastrar Sala', 'N', 13);
INSERT INTO permissao (DESCRICAO, BLOQUEADO, IDPAI) VALUES ('Alterar Sala', 'N', 13);
INSERT INTO permissao (DESCRICAO, BLOQUEADO, IDPAI) VALUES ('Excluir Sala', 'N', 13);

INSERT INTO permissao (DESCRICAO, BLOQUEADO, IDPAI) VALUES ('Colaborador', 'N', NULL); /*17*/
INSERT INTO permissao (DESCRICAO, BLOQUEADO, IDPAI) VALUES ('Cadastrar Colaborador', 'N', 17);
INSERT INTO permissao (DESCRICAO, BLOQUEADO, IDPAI) VALUES ('Alterar Colaborador', 'N', 17);
INSERT INTO permissao (DESCRICAO, BLOQUEADO, IDPAI) VALUES ('Excluir Colaborador', 'N', 17);

INSERT INTO permissao (DESCRICAO, BLOQUEADO, IDPAI) VALUES ('Agendamento Turma', 'N', NULL); /*21*/
INSERT INTO permissao (DESCRICAO, BLOQUEADO, IDPAI) VALUES ('Cadastrar Turma', 'N', 21);
INSERT INTO permissao (DESCRICAO, BLOQUEADO, IDPAI) VALUES ('Alterar Turma', 'N', 21);
INSERT INTO permissao (DESCRICAO, BLOQUEADO, IDPAI) VALUES ('Excluir Colaborador', 'N', 21);

INSERT INTO permissao (DESCRICAO, BLOQUEADO, IDPAI) VALUES ('Perfil', 'N', NULL); /*25*/
INSERT INTO permissao (DESCRICAO, BLOQUEADO, IDPAI) VALUES ('Cadastrar Perfil', 'N', 25);
INSERT INTO permissao (DESCRICAO, BLOQUEADO, IDPAI) VALUES ('Alterar Perfil', 'N', 25);
INSERT INTO permissao (DESCRICAO, BLOQUEADO, IDPAI) VALUES ('Excluir Perfil', 'N', 25);

INSERT INTO permissao (DESCRICAO, BLOQUEADO, IDPAI) VALUES ('Usuário', 'N', NULL); /*29*/
INSERT INTO permissao (DESCRICAO, BLOQUEADO, IDPAI) VALUES ('Cadastrar Usuário', 'N', 29);
INSERT INTO permissao (DESCRICAO, BLOQUEADO, IDPAI) VALUES ('Alterar Usuário', 'N', 29);
INSERT INTO permissao (DESCRICAO, BLOQUEADO, IDPAI) VALUES ('Excluir Usuário', 'N', 29);

INSERT INTO permissao (DESCRICAO, BLOQUEADO, IDPAI) VALUES ('Liberar Permissao', 'N', NULL);
INSERT INTO permissao (DESCRICAO, BLOQUEADO, IDPAI) VALUES ('Configuração Email', 'N', NULL);

/*---------------------------------------------------------------------------------------
								INSERT PERFIL PERMISSÃO
---------------------------------------------------------------------------------------*/

INSERT INTO PERFIL_PERMISSAO (IDPERFIL, IDPERMISSAO) VALUES (1, 1);
INSERT INTO PERFIL_PERMISSAO (IDPERFIL, IDPERMISSAO) VALUES (1, 2);
INSERT INTO PERFIL_PERMISSAO (IDPERFIL, IDPERMISSAO) VALUES (1, 3);
INSERT INTO PERFIL_PERMISSAO (IDPERFIL, IDPERMISSAO) VALUES (1, 4);
INSERT INTO PERFIL_PERMISSAO (IDPERFIL, IDPERMISSAO) VALUES (1, 5);
INSERT INTO PERFIL_PERMISSAO (IDPERFIL, IDPERMISSAO) VALUES (1, 6);
INSERT INTO PERFIL_PERMISSAO (IDPERFIL, IDPERMISSAO) VALUES (1, 7);
INSERT INTO PERFIL_PERMISSAO (IDPERFIL, IDPERMISSAO) VALUES (1, 8);
INSERT INTO PERFIL_PERMISSAO (IDPERFIL, IDPERMISSAO) VALUES (1, 9);
INSERT INTO PERFIL_PERMISSAO (IDPERFIL, IDPERMISSAO) VALUES (1, 10);
INSERT INTO PERFIL_PERMISSAO (IDPERFIL, IDPERMISSAO) VALUES (1, 11);
INSERT INTO PERFIL_PERMISSAO (IDPERFIL, IDPERMISSAO) VALUES (1, 12);
INSERT INTO PERFIL_PERMISSAO (IDPERFIL, IDPERMISSAO) VALUES (1, 13);
INSERT INTO PERFIL_PERMISSAO (IDPERFIL, IDPERMISSAO) VALUES (1, 14);
INSERT INTO PERFIL_PERMISSAO (IDPERFIL, IDPERMISSAO) VALUES (1, 15);
INSERT INTO PERFIL_PERMISSAO (IDPERFIL, IDPERMISSAO) VALUES (1, 16);
INSERT INTO PERFIL_PERMISSAO (IDPERFIL, IDPERMISSAO) VALUES (1, 17);
INSERT INTO PERFIL_PERMISSAO (IDPERFIL, IDPERMISSAO) VALUES (1, 18);
INSERT INTO PERFIL_PERMISSAO (IDPERFIL, IDPERMISSAO) VALUES (1, 19);
INSERT INTO PERFIL_PERMISSAO (IDPERFIL, IDPERMISSAO) VALUES (1, 20);
INSERT INTO PERFIL_PERMISSAO (IDPERFIL, IDPERMISSAO) VALUES (1, 21);
INSERT INTO PERFIL_PERMISSAO (IDPERFIL, IDPERMISSAO) VALUES (1, 22);
INSERT INTO PERFIL_PERMISSAO (IDPERFIL, IDPERMISSAO) VALUES (1, 23);
INSERT INTO PERFIL_PERMISSAO (IDPERFIL, IDPERMISSAO) VALUES (1, 24);
INSERT INTO PERFIL_PERMISSAO (IDPERFIL, IDPERMISSAO) VALUES (1, 25);
INSERT INTO PERFIL_PERMISSAO (IDPERFIL, IDPERMISSAO) VALUES (1, 26);
INSERT INTO PERFIL_PERMISSAO (IDPERFIL, IDPERMISSAO) VALUES (1, 27);
INSERT INTO PERFIL_PERMISSAO (IDPERFIL, IDPERMISSAO) VALUES (1, 28);
INSERT INTO PERFIL_PERMISSAO (IDPERFIL, IDPERMISSAO) VALUES (1, 29);
INSERT INTO PERFIL_PERMISSAO (IDPERFIL, IDPERMISSAO) VALUES (1, 30);
INSERT INTO PERFIL_PERMISSAO (IDPERFIL, IDPERMISSAO) VALUES (1, 31);
INSERT INTO PERFIL_PERMISSAO (IDPERFIL, IDPERMISSAO) VALUES (1, 32);
INSERT INTO PERFIL_PERMISSAO (IDPERFIL, IDPERMISSAO) VALUES (1, 33);
INSERT INTO PERFIL_PERMISSAO (IDPERFIL, IDPERMISSAO) VALUES (1, 34);
