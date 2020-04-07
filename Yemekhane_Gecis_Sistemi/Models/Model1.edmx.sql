
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 03/23/2020 20:11:39
-- Generated from EDMX file: C:\Users\mrton\OneDrive\Belgeler\GitHub\yemekhane_gecis_system-master\Yemekhane_Gecis_Sistemi\Models\Model1.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [YEMEKHANE];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_gecis_loglari_islem_sonuc]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[gecis_loglari] DROP CONSTRAINT [FK_gecis_loglari_islem_sonuc];
GO
IF OBJECT_ID(N'[dbo].[FK_gecis_loglari_islem_tipleri]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[gecis_loglari] DROP CONSTRAINT [FK_gecis_loglari_islem_tipleri];
GO
IF OBJECT_ID(N'[dbo].[FK_gecis_loglari_kart_bilgileri]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[gecis_loglari] DROP CONSTRAINT [FK_gecis_loglari_kart_bilgileri];
GO
IF OBJECT_ID(N'[dbo].[FK_gecis_loglari_kullanicilar]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[gecis_loglari] DROP CONSTRAINT [FK_gecis_loglari_kullanicilar];
GO
IF OBJECT_ID(N'[dbo].[FK_kart_bilgileri_kart_bilgileri]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[kart_bilgileri] DROP CONSTRAINT [FK_kart_bilgileri_kart_bilgileri];
GO
IF OBJECT_ID(N'[dbo].[FK_kart_bilgileri_kullancilar]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[kart_bilgileri] DROP CONSTRAINT [FK_kart_bilgileri_kullancilar];
GO
IF OBJECT_ID(N'[dbo].[FK_sistem_log_islem_tipleri]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[sistem_log] DROP CONSTRAINT [FK_sistem_log_islem_tipleri];
GO
IF OBJECT_ID(N'[dbo].[FK_sistem_log_kullanicilar]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[sistem_log] DROP CONSTRAINT [FK_sistem_log_kullanicilar];
GO
IF OBJECT_ID(N'[dbo].[FK_ucretler_kart_tipleri]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ucretler] DROP CONSTRAINT [FK_ucretler_kart_tipleri];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[birimler]', 'U') IS NOT NULL
    DROP TABLE [dbo].[birimler];
GO
IF OBJECT_ID(N'[dbo].[gecis_loglari]', 'U') IS NOT NULL
    DROP TABLE [dbo].[gecis_loglari];
GO
IF OBJECT_ID(N'[dbo].[islem_sonuc]', 'U') IS NOT NULL
    DROP TABLE [dbo].[islem_sonuc];
GO
IF OBJECT_ID(N'[dbo].[islem_tipleri]', 'U') IS NOT NULL
    DROP TABLE [dbo].[islem_tipleri];
GO
IF OBJECT_ID(N'[dbo].[kart_bilgileri]', 'U') IS NOT NULL
    DROP TABLE [dbo].[kart_bilgileri];
GO
IF OBJECT_ID(N'[dbo].[kart_tipleri]', 'U') IS NOT NULL
    DROP TABLE [dbo].[kart_tipleri];
GO
IF OBJECT_ID(N'[dbo].[kullanicilar]', 'U') IS NOT NULL
    DROP TABLE [dbo].[kullanicilar];
GO
IF OBJECT_ID(N'[dbo].[sistem_log]', 'U') IS NOT NULL
    DROP TABLE [dbo].[sistem_log];
GO
IF OBJECT_ID(N'[dbo].[sysdiagrams]', 'U') IS NOT NULL
    DROP TABLE [dbo].[sysdiagrams];
GO
IF OBJECT_ID(N'[dbo].[ucretler]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ucretler];
GO
IF OBJECT_ID(N'[dbo].[unvanlar]', 'U') IS NOT NULL
    DROP TABLE [dbo].[unvanlar];
GO
IF OBJECT_ID(N'[YEMEKHANEModelStoreContainer].[View_gecis_loglari]', 'U') IS NOT NULL
    DROP TABLE [YEMEKHANEModelStoreContainer].[View_gecis_loglari];
GO
IF OBJECT_ID(N'[YEMEKHANEModelStoreContainer].[View_Kullanicilar1]', 'U') IS NOT NULL
    DROP TABLE [YEMEKHANEModelStoreContainer].[View_Kullanicilar1];
GO
IF OBJECT_ID(N'[YEMEKHANEModelStoreContainer].[view_sistem_loglari]', 'U') IS NOT NULL
    DROP TABLE [YEMEKHANEModelStoreContainer].[view_sistem_loglari];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'birimler'
CREATE TABLE [dbo].[birimler] (
    [birim_id] int IDENTITY(1,1) NOT NULL,
    [birim_adi] nvarchar(50)  NULL
);
GO

-- Creating table 'gecis_loglari'
CREATE TABLE [dbo].[gecis_loglari] (
    [log_id] int IDENTITY(1,1) NOT NULL,
    [kullanici_id] int  NULL,
    [kart_id] int  NULL,
    [islem_tipi_id] int  NULL,
    [islem_sonuc_id] int  NULL,
    [ucret] nvarchar(15)  NULL,
    [kalan_bakiye] nvarchar(15)  NULL,
    [mesaj] nvarchar(50)  NULL,
    [kayit_tarihi] datetime  NULL
);
GO

-- Creating table 'islem_sonuc'
CREATE TABLE [dbo].[islem_sonuc] (
    [sonuc_id] int IDENTITY(1,1) NOT NULL,
    [islem_mesaji] nvarchar(50)  NULL
);
GO

-- Creating table 'islem_tipleri'
CREATE TABLE [dbo].[islem_tipleri] (
    [islem_tipi_id] int IDENTITY(1,1) NOT NULL,
    [islem_adi] nvarchar(50)  NULL
);
GO

-- Creating table 'kart_bilgileri'
CREATE TABLE [dbo].[kart_bilgileri] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [kullanici_id] int  NULL,
    [kart_no] nvarchar(8)  NULL,
    [kart_tipi_id] int  NULL,
    [bakiye] nvarchar(25)  NULL,
    [kayit_tarihi] datetime  NULL
);
GO

-- Creating table 'kart_tipleri'
CREATE TABLE [dbo].[kart_tipleri] (
    [kart_tipi_id] int IDENTITY(1,1) NOT NULL,
    [kart_tipi] nvarchar(50)  NULL,
    [ucret] nvarchar(15)  NULL,
    [kayit_tarihi] datetime  NULL,
    [guncelleme_tarihi] datetime  NULL
);
GO

-- Creating table 'kullanicilar'
CREATE TABLE [dbo].[kullanicilar] (
    [kullanici_id] int IDENTITY(1,1) NOT NULL,
    [kullanici_adi] nvarchar(25)  NULL,
    [sifre] nvarchar(20)  NULL,
    [ad] nvarchar(30)  NULL,
    [soyad] nvarchar(30)  NULL,
    [email] nvarchar(25)  NULL,
    [tc] nvarchar(11)  NULL,
    [kayit_tarihi] datetime  NULL,
    [guncelleme_tarihi] datetime  NULL
);
GO

-- Creating table 'ucretler'
CREATE TABLE [dbo].[ucretler] (
    [ucret_id] int IDENTITY(1,1) NOT NULL,
    [kart_tipi_id] int  NULL,
    [ucret] nvarchar(10)  NULL,
    [kayit_tarihi] datetime  NULL,
    [guncelleme_tarihi] datetime  NULL
);
GO

-- Creating table 'unvanlar'
CREATE TABLE [dbo].[unvanlar] (
    [unvan_id] int IDENTITY(1,1) NOT NULL,
    [unvan_adi] nvarchar(25)  NULL
);
GO

-- Creating table 'View_Kullanicilar1'
CREATE TABLE [dbo].[View_Kullanicilar1] (
    [kullanici_id] int  NOT NULL,
    [kullanici_adi] nvarchar(25)  NULL,
    [ad] nvarchar(30)  NULL,
    [soyad] nvarchar(30)  NULL,
    [tc] nvarchar(11)  NULL,
    [kart_tipi] nvarchar(50)  NULL,
    [kart_no] nvarchar(8)  NULL,
    [bakiye] nvarchar(25)  NULL
);
GO

-- Creating table 'View_gecis_loglari'
CREATE TABLE [dbo].[View_gecis_loglari] (
    [log_id] int  NOT NULL,
    [kayit_tarihi] datetime  NULL,
    [kullanici_id] int  NOT NULL,
    [ad] nvarchar(30)  NULL,
    [soyad] nvarchar(30)  NULL,
    [kart_id] int  NULL,
    [islem_adi] nvarchar(50)  NULL,
    [islem_mesaji] nvarchar(50)  NULL,
    [ucret] nvarchar(15)  NULL,
    [kalan_bakiye] nvarchar(15)  NULL,
    [mesaj] nvarchar(50)  NULL
);
GO

-- Creating table 'sysdiagrams'
CREATE TABLE [dbo].[sysdiagrams] (
    [name] nvarchar(128)  NOT NULL,
    [principal_id] int  NOT NULL,
    [diagram_id] int IDENTITY(1,1) NOT NULL,
    [version] int  NULL,
    [definition] varbinary(max)  NULL
);
GO

-- Creating table 'view_sistem_loglari'
CREATE TABLE [dbo].[view_sistem_loglari] (
    [id] int  NOT NULL,
    [ad] nvarchar(30)  NULL,
    [soyad] nvarchar(30)  NULL,
    [islem_adi] nvarchar(50)  NULL,
    [islem_tarihi] datetime  NULL,
    [mesaj] nvarchar(25)  NULL
);
GO

-- Creating table 'sistem_log'
CREATE TABLE [dbo].[sistem_log] (
    [id] int IDENTITY(1,1) NOT NULL,
    [kullanici_id] int  NULL,
    [islem_tarihi] datetime  NULL,
    [islem_tipi_id] int  NULL,
    [mesaj] nvarchar(100)  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [birim_id] in table 'birimler'
ALTER TABLE [dbo].[birimler]
ADD CONSTRAINT [PK_birimler]
    PRIMARY KEY CLUSTERED ([birim_id] ASC);
GO

-- Creating primary key on [log_id] in table 'gecis_loglari'
ALTER TABLE [dbo].[gecis_loglari]
ADD CONSTRAINT [PK_gecis_loglari]
    PRIMARY KEY CLUSTERED ([log_id] ASC);
GO

-- Creating primary key on [sonuc_id] in table 'islem_sonuc'
ALTER TABLE [dbo].[islem_sonuc]
ADD CONSTRAINT [PK_islem_sonuc]
    PRIMARY KEY CLUSTERED ([sonuc_id] ASC);
GO

-- Creating primary key on [islem_tipi_id] in table 'islem_tipleri'
ALTER TABLE [dbo].[islem_tipleri]
ADD CONSTRAINT [PK_islem_tipleri]
    PRIMARY KEY CLUSTERED ([islem_tipi_id] ASC);
GO

-- Creating primary key on [ID] in table 'kart_bilgileri'
ALTER TABLE [dbo].[kart_bilgileri]
ADD CONSTRAINT [PK_kart_bilgileri]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [kart_tipi_id] in table 'kart_tipleri'
ALTER TABLE [dbo].[kart_tipleri]
ADD CONSTRAINT [PK_kart_tipleri]
    PRIMARY KEY CLUSTERED ([kart_tipi_id] ASC);
GO

-- Creating primary key on [kullanici_id] in table 'kullanicilar'
ALTER TABLE [dbo].[kullanicilar]
ADD CONSTRAINT [PK_kullanicilar]
    PRIMARY KEY CLUSTERED ([kullanici_id] ASC);
GO

-- Creating primary key on [ucret_id] in table 'ucretler'
ALTER TABLE [dbo].[ucretler]
ADD CONSTRAINT [PK_ucretler]
    PRIMARY KEY CLUSTERED ([ucret_id] ASC);
GO

-- Creating primary key on [unvan_id] in table 'unvanlar'
ALTER TABLE [dbo].[unvanlar]
ADD CONSTRAINT [PK_unvanlar]
    PRIMARY KEY CLUSTERED ([unvan_id] ASC);
GO

-- Creating primary key on [kullanici_id] in table 'View_Kullanicilar1'
ALTER TABLE [dbo].[View_Kullanicilar1]
ADD CONSTRAINT [PK_View_Kullanicilar1]
    PRIMARY KEY CLUSTERED ([kullanici_id] ASC);
GO

-- Creating primary key on [log_id], [kullanici_id] in table 'View_gecis_loglari'
ALTER TABLE [dbo].[View_gecis_loglari]
ADD CONSTRAINT [PK_View_gecis_loglari]
    PRIMARY KEY CLUSTERED ([log_id], [kullanici_id] ASC);
GO

-- Creating primary key on [diagram_id] in table 'sysdiagrams'
ALTER TABLE [dbo].[sysdiagrams]
ADD CONSTRAINT [PK_sysdiagrams]
    PRIMARY KEY CLUSTERED ([diagram_id] ASC);
GO

-- Creating primary key on [id] in table 'view_sistem_loglari'
ALTER TABLE [dbo].[view_sistem_loglari]
ADD CONSTRAINT [PK_view_sistem_loglari]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'sistem_log'
ALTER TABLE [dbo].[sistem_log]
ADD CONSTRAINT [PK_sistem_log]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [islem_sonuc_id] in table 'gecis_loglari'
ALTER TABLE [dbo].[gecis_loglari]
ADD CONSTRAINT [FK_gecis_loglari_islem_sonuc]
    FOREIGN KEY ([islem_sonuc_id])
    REFERENCES [dbo].[islem_sonuc]
        ([sonuc_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_gecis_loglari_islem_sonuc'
CREATE INDEX [IX_FK_gecis_loglari_islem_sonuc]
ON [dbo].[gecis_loglari]
    ([islem_sonuc_id]);
GO

-- Creating foreign key on [islem_tipi_id] in table 'gecis_loglari'
ALTER TABLE [dbo].[gecis_loglari]
ADD CONSTRAINT [FK_gecis_loglari_islem_tipleri]
    FOREIGN KEY ([islem_tipi_id])
    REFERENCES [dbo].[islem_tipleri]
        ([islem_tipi_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_gecis_loglari_islem_tipleri'
CREATE INDEX [IX_FK_gecis_loglari_islem_tipleri]
ON [dbo].[gecis_loglari]
    ([islem_tipi_id]);
GO

-- Creating foreign key on [kart_id] in table 'gecis_loglari'
ALTER TABLE [dbo].[gecis_loglari]
ADD CONSTRAINT [FK_gecis_loglari_kart_bilgileri]
    FOREIGN KEY ([kart_id])
    REFERENCES [dbo].[kart_bilgileri]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_gecis_loglari_kart_bilgileri'
CREATE INDEX [IX_FK_gecis_loglari_kart_bilgileri]
ON [dbo].[gecis_loglari]
    ([kart_id]);
GO

-- Creating foreign key on [kullanici_id] in table 'gecis_loglari'
ALTER TABLE [dbo].[gecis_loglari]
ADD CONSTRAINT [FK_gecis_loglari_kullanicilar]
    FOREIGN KEY ([kullanici_id])
    REFERENCES [dbo].[kullanicilar]
        ([kullanici_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_gecis_loglari_kullanicilar'
CREATE INDEX [IX_FK_gecis_loglari_kullanicilar]
ON [dbo].[gecis_loglari]
    ([kullanici_id]);
GO

-- Creating foreign key on [kart_tipi_id] in table 'kart_bilgileri'
ALTER TABLE [dbo].[kart_bilgileri]
ADD CONSTRAINT [FK_kart_bilgileri_kart_bilgileri]
    FOREIGN KEY ([kart_tipi_id])
    REFERENCES [dbo].[kart_tipleri]
        ([kart_tipi_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_kart_bilgileri_kart_bilgileri'
CREATE INDEX [IX_FK_kart_bilgileri_kart_bilgileri]
ON [dbo].[kart_bilgileri]
    ([kart_tipi_id]);
GO

-- Creating foreign key on [kullanici_id] in table 'kart_bilgileri'
ALTER TABLE [dbo].[kart_bilgileri]
ADD CONSTRAINT [FK_kart_bilgileri_kullancilar]
    FOREIGN KEY ([kullanici_id])
    REFERENCES [dbo].[kullanicilar]
        ([kullanici_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_kart_bilgileri_kullancilar'
CREATE INDEX [IX_FK_kart_bilgileri_kullancilar]
ON [dbo].[kart_bilgileri]
    ([kullanici_id]);
GO

-- Creating foreign key on [kart_tipi_id] in table 'ucretler'
ALTER TABLE [dbo].[ucretler]
ADD CONSTRAINT [FK_ucretler_kart_tipleri]
    FOREIGN KEY ([kart_tipi_id])
    REFERENCES [dbo].[kart_tipleri]
        ([kart_tipi_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ucretler_kart_tipleri'
CREATE INDEX [IX_FK_ucretler_kart_tipleri]
ON [dbo].[ucretler]
    ([kart_tipi_id]);
GO

-- Creating foreign key on [islem_tipi_id] in table 'sistem_log'
ALTER TABLE [dbo].[sistem_log]
ADD CONSTRAINT [FK_sistem_log_islem_tipleri]
    FOREIGN KEY ([islem_tipi_id])
    REFERENCES [dbo].[islem_tipleri]
        ([islem_tipi_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_sistem_log_islem_tipleri'
CREATE INDEX [IX_FK_sistem_log_islem_tipleri]
ON [dbo].[sistem_log]
    ([islem_tipi_id]);
GO

-- Creating foreign key on [kullanici_id] in table 'sistem_log'
ALTER TABLE [dbo].[sistem_log]
ADD CONSTRAINT [FK_sistem_log_kullanicilar]
    FOREIGN KEY ([kullanici_id])
    REFERENCES [dbo].[kullanicilar]
        ([kullanici_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_sistem_log_kullanicilar'
CREATE INDEX [IX_FK_sistem_log_kullanicilar]
ON [dbo].[sistem_log]
    ([kullanici_id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------