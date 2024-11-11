IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Tarefas] (
    [Id] int NOT NULL IDENTITY,
    [Titulo] nvarchar(50) NULL,
    [Descricao] nvarchar(500) NULL,
    [DataConclusaoPrevista] datetime2 NULL,
    [DataConclusaoEfetiva] datetime2 NULL,
    [Situacao] int NOT NULL,
    CONSTRAINT [PK_Tarefas] PRIMARY KEY ([Id])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20241108214604_inicial', N'8.0.8');
GO

COMMIT;
GO

