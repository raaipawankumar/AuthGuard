# Single Sign On (OAuth 2.0 and OpenID Connect)

## DotNet Implementation
`IdentityServer4` is deprecated due to security issue. It has been replaced with `Duende.IdentityServer` that needs license.
### Required Packages

```bash
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
```

User Store Interfaces
IUserStore<TUser>: Basic user management.
IUserPasswordStore<TUser>: Password management.
IUserRoleStore<TUser>: Role management for users.
IUserEmailStore<TUser>: Email management.
IUserPhoneNumberStore<TUser>: Phone number management.
IUserTwoFactorStore<TUser>: Two-factor authentication management.
IUserLockoutStore<TUser>: Account lockout management.
IUserClaimStore<TUser>: User claims management.
IUserLoginStore<TUser>: External login management.
IUserAuthenticatorKeyStore<TUser>: Authenticator key management.
IUserTwoFactorRecoveryCodeStore<TUser>: Two-factor recovery code management.
IUserSecurityStampStore<TUser>: Security stamp management.
IUserTokenStore<TUser>: User token management.

Role Store Interfaces
IRoleStore<TRole>: Basic role management.
IRoleClaimStore<TRole>: Role claims management.

User and Role Validator Interfaces
IUserValidator<TUser>: Custom user validation.
IPasswordValidator<TUser>: Custom password validation.
IRoleValidator<TRole>: Custom role validation.
Custom Services
ILookupNormalizer: Normalizes user and role names.
IUserClaimsPrincipalFactory<TUser>: Creates a ClaimsPrincipal for a user.
IUserConfirmation<TUser>: Confirms user accounts.
IUserTwoFactorTokenProvider<TUser>: Provides two-factor authentication tokens.
IUserStore<TUser>: Provides storage for user information.