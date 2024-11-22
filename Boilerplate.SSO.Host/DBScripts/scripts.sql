CREATE DATABASE SSO;
GO
USE SSO;
GO
CREATE TABLE Clients (
    Id INT PRIMARY KEY IDENTITY(1,1),
    ClientId NVARCHAR(200) NOT NULL,
    ClientName NVARCHAR(200) NULL,
    AllowedGrantTypes NVARCHAR(200) NULL,
    RedirectUris NVARCHAR(2000) NULL,
    PostLogoutRedirectUris NVARCHAR(2000) NULL,
    AllowedCorsOrigins NVARCHAR(2000) NULL,
    RequireClientSecret BIT NOT NULL,
    RequirePkce BIT NOT NULL,
    AllowPlainTextPkce BIT NOT NULL,
    AllowAccessTokensViaBrowser BIT NOT NULL,
    AllowOfflineAccess BIT NOT NULL,
    AccessTokenLifetime INT NOT NULL,
    IdentityTokenLifetime INT NOT NULL,
    AuthorizationCodeLifetime INT NOT NULL,
    AbsoluteRefreshTokenLifetime INT NOT NULL,
    SlidingRefreshTokenLifetime INT NOT NULL,
    RefreshTokenUsage INT NOT NULL,
    RefreshTokenExpiration INT NOT NULL,
    UpdateAccessTokenClaimsOnRefresh BIT NOT NULL,
    EnableLocalLogin BIT NOT NULL,
    IncludeJwtId BIT NOT NULL,
    AlwaysSendClientClaims BIT NOT NULL,
    AlwaysIncludeUserClaimsInIdToken BIT NOT NULL,
    ClientClaimsPrefix NVARCHAR(200) NULL,
    PairWiseSubjectSalt NVARCHAR(200) NULL,
    UserSsoLifetime INT NULL,
    UserCodeType NVARCHAR(100) NULL,
    DeviceCodeLifetime INT NOT NULL,
    NonEditable BIT NOT NULL
);

CREATE TABLE ClientSecrets (
    Id INT PRIMARY KEY IDENTITY(1,1),
    ClientId NVARCHAR(200) NOT NULL,
    Description NVARCHAR(2000) NULL,
    Value NVARCHAR(2000) NOT NULL,
    Expiration DATETIME NULL,
    Type NVARCHAR(200) NOT NULL
);

CREATE TABLE ClientScopes (
    Id INT PRIMARY KEY IDENTITY(1,1),
    ClientId NVARCHAR(200) NOT NULL,
    Scope NVARCHAR(200) NOT NULL
);

-- Sample data
INSERT INTO Clients (ClientId, ClientName, AllowedGrantTypes, RequireClientSecret, RequirePkce, AllowPlainTextPkce, AllowAccessTokensViaBrowser, AllowOfflineAccess, AccessTokenLifetime, IdentityTokenLifetime, AuthorizationCodeLifetime, AbsoluteRefreshTokenLifetime, SlidingRefreshTokenLifetime, RefreshTokenUsage, RefreshTokenExpiration, UpdateAccessTokenClaimsOnRefresh, EnableLocalLogin, IncludeJwtId, AlwaysSendClientClaims, AlwaysIncludeUserClaimsInIdToken, ClientClaimsPrefix, PairWiseSubjectSalt, UserSsoLifetime, UserCodeType, DeviceCodeLifetime, NonEditable)
VALUES ('client', 'Sample Client', 'client_credentials', 1, 0, 0, 0, 0, 3600, 300, 300, 2592000, 1296000, 1, 1, 0, 1, 0, 0, 0, 'client_', NULL, NULL, NULL, 300, 0);

INSERT INTO ClientSecrets (ClientId, Description, Value, Expiration, Type)
VALUES ('client', 'Sample Secret', 'secret', NULL, 'SharedSecret');

INSERT INTO ClientScopes (ClientId, Scope)
VALUES ('client', 'api1');

CREATE TABLE IdentityResourceScopes (
    Id INT PRIMARY KEY IDENTITY(1,1),
    IdentityResourceName NVARCHAR(200) NOT NULL,
    ScopeName NVARCHAR(200) NOT NULL
);

-- Sample data
INSERT INTO IdentityResourceScopes (IdentityResourceName, ScopeName)
VALUES ('openid', 'openid'),
       ('profile', 'profile');

CREATE TABLE ApiResources (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(200) NOT NULL,
    DisplayName NVARCHAR(200) NULL,
    Description NVARCHAR(1000) NULL,
    Enabled BIT NOT NULL
);

CREATE TABLE ApiResourceScopes (
    Id INT PRIMARY KEY IDENTITY(1,1),
    ApiResourceName NVARCHAR(200) NOT NULL,
    ScopeName NVARCHAR(200) NOT NULL
);

CREATE TABLE IdentityResources (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(200) NOT NULL,
    DisplayName NVARCHAR(200) NULL,
    Description NVARCHAR(1000) NULL,
    Enabled BIT NOT NULL
);

-- Sample data
INSERT INTO ApiResources (Name, DisplayName, Description, Enabled)
VALUES ('api1', 'My API', 'Access to My API', 1);

INSERT INTO ApiResourceScopes (ApiResourceName, ScopeName)
VALUES ('api1', 'api1');

INSERT INTO IdentityResources (Name, DisplayName, Description, Enabled)
VALUES ('openid', 'OpenID', 'OpenID Connect scope', 1),
       ('profile', 'Profile', 'User profile information', 1);

CREATE TABLE ApiScopes (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(200) NOT NULL,
    DisplayName NVARCHAR(200) NULL,
    Description NVARCHAR(1000) NULL,
    Required BIT NOT NULL DEFAULT 0,
    Emphasize BIT NOT NULL DEFAULT 0,
    ShowInDiscoveryDocument BIT NOT NULL DEFAULT 1,
    Created DATETIME NOT NULL DEFAULT GETDATE(),
    Updated DATETIME NULL,
    LastAccessed DATETIME NULL,
    Enabled BIT NOT NULL DEFAULT 1
);

CREATE TABLE ApiScopeClaims (
    Id INT PRIMARY KEY IDENTITY(1,1),
    ApiScopeId INT NOT NULL,
    Type NVARCHAR(200) NOT NULL,
    FOREIGN KEY (ApiScopeId) REFERENCES ApiScopes(Id) ON DELETE CASCADE
);

CREATE TABLE ApiScopeProperties (
    Id INT PRIMARY KEY IDENTITY(1,1),
    ApiScopeId INT NOT NULL,
    [Key] NVARCHAR(250) NOT NULL,
    Value NVARCHAR(2000) NOT NULL,
    FOREIGN KEY (ApiScopeId) REFERENCES ApiScopes(Id) ON DELETE CASCADE
);

-- Insert sample data into ApiScopes
INSERT INTO ApiScopes (Name, DisplayName, Description, Required, Emphasize, ShowInDiscoveryDocument, Created, Enabled)
VALUES 
('api1.read', 'Read access to API 1', 'Allows read access to API 1', 0, 0, 1, GETDATE(), 1),
('api1.write', 'Write access to API 1', 'Allows write access to API 1', 0, 1, 1, GETDATE(), 1),
('api2.full_access', 'Full access to API 2', 'Allows full access to API 2', 1, 1, 1, GETDATE(), 1);

-- Insert sample data into ApiScopeClaims
INSERT INTO ApiScopeClaims (ApiScopeId, Type)
VALUES 
((SELECT Id FROM ApiScopes WHERE Name = 'api1.read'), 'role'),
((SELECT Id FROM ApiScopes WHERE Name = 'api1.read'), 'email'),
((SELECT Id FROM ApiScopes WHERE Name = 'api1.write'), 'role'),
((SELECT Id FROM ApiScopes WHERE Name = 'api2.full_access'), 'role'),
((SELECT Id FROM ApiScopes WHERE Name = 'api2.full_access'), 'email'),
((SELECT Id FROM ApiScopes WHERE Name = 'api2.full_access'), 'profile');

-- Insert sample data into ApiScopeProperties
INSERT INTO ApiScopeProperties (ApiScopeId, [Key], [Value])
VALUES 
((SELECT Id FROM ApiScopes WHERE Name = 'api1.read'), 'property1', 'value1'),
((SELECT Id FROM ApiScopes WHERE Name = 'api1.read'), 'property2', 'value2'),
((SELECT Id FROM ApiScopes WHERE Name = 'api1.write'), 'property1', 'value1'),
((SELECT Id FROM ApiScopes WHERE Name = 'api2.full_access'), 'property1', 'value1'),
((SELECT Id FROM ApiScopes WHERE Name = 'api2.full_access'), 'property2', 'value2'),
((SELECT Id FROM ApiScopes WHERE Name = 'api2.full_access'), 'property3', 'value3');