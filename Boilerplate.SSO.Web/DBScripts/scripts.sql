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