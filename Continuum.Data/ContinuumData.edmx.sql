
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 08/18/2015 16:44:46
-- Generated from EDMX file: C:\Users\nickmck\Source\Repos\continuum-api\Continuum.Data\ContinuumData.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [continuumwebapi_db];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_CapabiltyLevel]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Capabilties] DROP CONSTRAINT [FK_CapabiltyLevel];
GO
IF OBJECT_ID(N'[dbo].[FK_DimensionCapabilty]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Capabilties] DROP CONSTRAINT [FK_DimensionCapabilty];
GO
IF OBJECT_ID(N'[dbo].[FK_OrganisationTeam]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Teams] DROP CONSTRAINT [FK_OrganisationTeam];
GO
IF OBJECT_ID(N'[dbo].[FK_AssessmentAssessmentStatus]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Assessments] DROP CONSTRAINT [FK_AssessmentAssessmentStatus];
GO
IF OBJECT_ID(N'[dbo].[FK_TeamAssessment]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Assessments] DROP CONSTRAINT [FK_TeamAssessment];
GO
IF OBJECT_ID(N'[dbo].[FK_TeamGoal]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Goals] DROP CONSTRAINT [FK_TeamGoal];
GO
IF OBJECT_ID(N'[dbo].[FK_AssessmentAssessmentItem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AssessmentItems] DROP CONSTRAINT [FK_AssessmentAssessmentItem];
GO
IF OBJECT_ID(N'[dbo].[FK_TeamTeamMember]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TeamMembers] DROP CONSTRAINT [FK_TeamTeamMember];
GO
IF OBJECT_ID(N'[dbo].[FK_AssessmentItemTeamMember]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AssessmentItems] DROP CONSTRAINT [FK_AssessmentItemTeamMember];
GO
IF OBJECT_ID(N'[dbo].[FK_AssessmentItemCapabilty]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AssessmentItems] DROP CONSTRAINT [FK_AssessmentItemCapabilty];
GO
IF OBJECT_ID(N'[dbo].[FK_AssessmentAssessmentResult]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AssessmentResults] DROP CONSTRAINT [FK_AssessmentAssessmentResult];
GO
IF OBJECT_ID(N'[dbo].[FK_AssessmentResultDimension]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AssessmentResults] DROP CONSTRAINT [FK_AssessmentResultDimension];
GO
IF OBJECT_ID(N'[dbo].[FK_GoalCapabilty]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Goals] DROP CONSTRAINT [FK_GoalCapabilty];
GO
IF OBJECT_ID(N'[dbo].[FK_TeamAvatarType]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Teams] DROP CONSTRAINT [FK_TeamAvatarType];
GO
IF OBJECT_ID(N'[dbo].[FK_AssessmentStatus_inherits_Lookup]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Lookups_AssessmentStatus] DROP CONSTRAINT [FK_AssessmentStatus_inherits_Lookup];
GO
IF OBJECT_ID(N'[dbo].[FK_AvatarType_inherits_Lookup]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Lookups_AvatarType] DROP CONSTRAINT [FK_AvatarType_inherits_Lookup];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Dimensions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Dimensions];
GO
IF OBJECT_ID(N'[dbo].[Capabilties]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Capabilties];
GO
IF OBJECT_ID(N'[dbo].[Levels]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Levels];
GO
IF OBJECT_ID(N'[dbo].[Organisations]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Organisations];
GO
IF OBJECT_ID(N'[dbo].[Teams]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Teams];
GO
IF OBJECT_ID(N'[dbo].[Assessments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Assessments];
GO
IF OBJECT_ID(N'[dbo].[Lookups]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Lookups];
GO
IF OBJECT_ID(N'[dbo].[Goals]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Goals];
GO
IF OBJECT_ID(N'[dbo].[AssessmentItems]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AssessmentItems];
GO
IF OBJECT_ID(N'[dbo].[TeamMembers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TeamMembers];
GO
IF OBJECT_ID(N'[dbo].[AssessmentResults]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AssessmentResults];
GO
IF OBJECT_ID(N'[dbo].[Lookups_AssessmentStatus]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Lookups_AssessmentStatus];
GO
IF OBJECT_ID(N'[dbo].[Lookups_AvatarType]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Lookups_AvatarType];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Dimensions'
CREATE TABLE [dbo].[Dimensions] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Active] bit  NOT NULL
);
GO

-- Creating table 'Capabilties'
CREATE TABLE [dbo].[Capabilties] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [LevelId] int  NOT NULL,
    [DimensionId] int  NOT NULL,
    [Active] bit  NOT NULL
);
GO

-- Creating table 'Levels'
CREATE TABLE [dbo].[Levels] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [DisplayName] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Organisations'
CREATE TABLE [dbo].[Organisations] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Teams'
CREATE TABLE [dbo].[Teams] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [OrganisationId] int  NOT NULL,
    [AvatarTypeId] int  NOT NULL
);
GO

-- Creating table 'Assessments'
CREATE TABLE [dbo].[Assessments] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [AssessmentStatusId] int  NOT NULL,
    [TeamId] int  NOT NULL
);
GO

-- Creating table 'Lookups'
CREATE TABLE [dbo].[Lookups] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Value] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Goals'
CREATE TABLE [dbo].[Goals] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Title] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [DueDate] datetime  NOT NULL,
    [Completed] bit  NOT NULL,
    [TeamId] int  NOT NULL,
    [CapabiltyId] int  NOT NULL
);
GO

-- Creating table 'AssessmentItems'
CREATE TABLE [dbo].[AssessmentItems] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [AssessmentId] int  NOT NULL,
    [TeamMemberId] int  NOT NULL,
    [CapabiltyId] int  NOT NULL,
    [CapabilityAchieved] bit  NOT NULL
);
GO

-- Creating table 'TeamMembers'
CREATE TABLE [dbo].[TeamMembers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UserId] nvarchar(max)  NOT NULL,
    [TeamId] int  NOT NULL
);
GO

-- Creating table 'AssessmentResults'
CREATE TABLE [dbo].[AssessmentResults] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [AssessmentId] int  NOT NULL,
    [Rating] nvarchar(max)  NOT NULL,
    [DimensionId] int  NOT NULL
);
GO

-- Creating table 'Lookups_AssessmentStatus'
CREATE TABLE [dbo].[Lookups_AssessmentStatus] (
    [Id] int  NOT NULL
);
GO

-- Creating table 'Lookups_AvatarType'
CREATE TABLE [dbo].[Lookups_AvatarType] (
    [Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Dimensions'
ALTER TABLE [dbo].[Dimensions]
ADD CONSTRAINT [PK_Dimensions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Capabilties'
ALTER TABLE [dbo].[Capabilties]
ADD CONSTRAINT [PK_Capabilties]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Levels'
ALTER TABLE [dbo].[Levels]
ADD CONSTRAINT [PK_Levels]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Organisations'
ALTER TABLE [dbo].[Organisations]
ADD CONSTRAINT [PK_Organisations]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Teams'
ALTER TABLE [dbo].[Teams]
ADD CONSTRAINT [PK_Teams]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Assessments'
ALTER TABLE [dbo].[Assessments]
ADD CONSTRAINT [PK_Assessments]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Lookups'
ALTER TABLE [dbo].[Lookups]
ADD CONSTRAINT [PK_Lookups]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Goals'
ALTER TABLE [dbo].[Goals]
ADD CONSTRAINT [PK_Goals]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AssessmentItems'
ALTER TABLE [dbo].[AssessmentItems]
ADD CONSTRAINT [PK_AssessmentItems]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TeamMembers'
ALTER TABLE [dbo].[TeamMembers]
ADD CONSTRAINT [PK_TeamMembers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AssessmentResults'
ALTER TABLE [dbo].[AssessmentResults]
ADD CONSTRAINT [PK_AssessmentResults]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Lookups_AssessmentStatus'
ALTER TABLE [dbo].[Lookups_AssessmentStatus]
ADD CONSTRAINT [PK_Lookups_AssessmentStatus]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Lookups_AvatarType'
ALTER TABLE [dbo].[Lookups_AvatarType]
ADD CONSTRAINT [PK_Lookups_AvatarType]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [LevelId] in table 'Capabilties'
ALTER TABLE [dbo].[Capabilties]
ADD CONSTRAINT [FK_CapabiltyLevel]
    FOREIGN KEY ([LevelId])
    REFERENCES [dbo].[Levels]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CapabiltyLevel'
CREATE INDEX [IX_FK_CapabiltyLevel]
ON [dbo].[Capabilties]
    ([LevelId]);
GO

-- Creating foreign key on [DimensionId] in table 'Capabilties'
ALTER TABLE [dbo].[Capabilties]
ADD CONSTRAINT [FK_DimensionCapabilty]
    FOREIGN KEY ([DimensionId])
    REFERENCES [dbo].[Dimensions]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DimensionCapabilty'
CREATE INDEX [IX_FK_DimensionCapabilty]
ON [dbo].[Capabilties]
    ([DimensionId]);
GO

-- Creating foreign key on [OrganisationId] in table 'Teams'
ALTER TABLE [dbo].[Teams]
ADD CONSTRAINT [FK_OrganisationTeam]
    FOREIGN KEY ([OrganisationId])
    REFERENCES [dbo].[Organisations]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_OrganisationTeam'
CREATE INDEX [IX_FK_OrganisationTeam]
ON [dbo].[Teams]
    ([OrganisationId]);
GO

-- Creating foreign key on [AssessmentStatusId] in table 'Assessments'
ALTER TABLE [dbo].[Assessments]
ADD CONSTRAINT [FK_AssessmentAssessmentStatus]
    FOREIGN KEY ([AssessmentStatusId])
    REFERENCES [dbo].[Lookups_AssessmentStatus]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AssessmentAssessmentStatus'
CREATE INDEX [IX_FK_AssessmentAssessmentStatus]
ON [dbo].[Assessments]
    ([AssessmentStatusId]);
GO

-- Creating foreign key on [TeamId] in table 'Assessments'
ALTER TABLE [dbo].[Assessments]
ADD CONSTRAINT [FK_TeamAssessment]
    FOREIGN KEY ([TeamId])
    REFERENCES [dbo].[Teams]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TeamAssessment'
CREATE INDEX [IX_FK_TeamAssessment]
ON [dbo].[Assessments]
    ([TeamId]);
GO

-- Creating foreign key on [TeamId] in table 'Goals'
ALTER TABLE [dbo].[Goals]
ADD CONSTRAINT [FK_TeamGoal]
    FOREIGN KEY ([TeamId])
    REFERENCES [dbo].[Teams]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TeamGoal'
CREATE INDEX [IX_FK_TeamGoal]
ON [dbo].[Goals]
    ([TeamId]);
GO

-- Creating foreign key on [AssessmentId] in table 'AssessmentItems'
ALTER TABLE [dbo].[AssessmentItems]
ADD CONSTRAINT [FK_AssessmentAssessmentItem]
    FOREIGN KEY ([AssessmentId])
    REFERENCES [dbo].[Assessments]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AssessmentAssessmentItem'
CREATE INDEX [IX_FK_AssessmentAssessmentItem]
ON [dbo].[AssessmentItems]
    ([AssessmentId]);
GO

-- Creating foreign key on [TeamId] in table 'TeamMembers'
ALTER TABLE [dbo].[TeamMembers]
ADD CONSTRAINT [FK_TeamTeamMember]
    FOREIGN KEY ([TeamId])
    REFERENCES [dbo].[Teams]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TeamTeamMember'
CREATE INDEX [IX_FK_TeamTeamMember]
ON [dbo].[TeamMembers]
    ([TeamId]);
GO

-- Creating foreign key on [TeamMemberId] in table 'AssessmentItems'
ALTER TABLE [dbo].[AssessmentItems]
ADD CONSTRAINT [FK_AssessmentItemTeamMember]
    FOREIGN KEY ([TeamMemberId])
    REFERENCES [dbo].[TeamMembers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AssessmentItemTeamMember'
CREATE INDEX [IX_FK_AssessmentItemTeamMember]
ON [dbo].[AssessmentItems]
    ([TeamMemberId]);
GO

-- Creating foreign key on [CapabiltyId] in table 'AssessmentItems'
ALTER TABLE [dbo].[AssessmentItems]
ADD CONSTRAINT [FK_AssessmentItemCapabilty]
    FOREIGN KEY ([CapabiltyId])
    REFERENCES [dbo].[Capabilties]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AssessmentItemCapabilty'
CREATE INDEX [IX_FK_AssessmentItemCapabilty]
ON [dbo].[AssessmentItems]
    ([CapabiltyId]);
GO

-- Creating foreign key on [AssessmentId] in table 'AssessmentResults'
ALTER TABLE [dbo].[AssessmentResults]
ADD CONSTRAINT [FK_AssessmentAssessmentResult]
    FOREIGN KEY ([AssessmentId])
    REFERENCES [dbo].[Assessments]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AssessmentAssessmentResult'
CREATE INDEX [IX_FK_AssessmentAssessmentResult]
ON [dbo].[AssessmentResults]
    ([AssessmentId]);
GO

-- Creating foreign key on [DimensionId] in table 'AssessmentResults'
ALTER TABLE [dbo].[AssessmentResults]
ADD CONSTRAINT [FK_AssessmentResultDimension]
    FOREIGN KEY ([DimensionId])
    REFERENCES [dbo].[Dimensions]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AssessmentResultDimension'
CREATE INDEX [IX_FK_AssessmentResultDimension]
ON [dbo].[AssessmentResults]
    ([DimensionId]);
GO

-- Creating foreign key on [CapabiltyId] in table 'Goals'
ALTER TABLE [dbo].[Goals]
ADD CONSTRAINT [FK_GoalCapabilty]
    FOREIGN KEY ([CapabiltyId])
    REFERENCES [dbo].[Capabilties]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GoalCapabilty'
CREATE INDEX [IX_FK_GoalCapabilty]
ON [dbo].[Goals]
    ([CapabiltyId]);
GO

-- Creating foreign key on [AvatarTypeId] in table 'Teams'
ALTER TABLE [dbo].[Teams]
ADD CONSTRAINT [FK_TeamAvatarType]
    FOREIGN KEY ([AvatarTypeId])
    REFERENCES [dbo].[Lookups_AvatarType]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TeamAvatarType'
CREATE INDEX [IX_FK_TeamAvatarType]
ON [dbo].[Teams]
    ([AvatarTypeId]);
GO

-- Creating foreign key on [Id] in table 'Lookups_AssessmentStatus'
ALTER TABLE [dbo].[Lookups_AssessmentStatus]
ADD CONSTRAINT [FK_AssessmentStatus_inherits_Lookup]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[Lookups]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'Lookups_AvatarType'
ALTER TABLE [dbo].[Lookups_AvatarType]
ADD CONSTRAINT [FK_AvatarType_inherits_Lookup]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[Lookups]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------