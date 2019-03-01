namespace Migrations
open SimpleMigrations

[<Migration(201903011606L, "Create Profiles")>]
type CreateProfiles() =
  inherit Migration()

  override __.Up() =
    base.Execute(@"CREATE TABLE Profiles(
      id INT NOT NULL,
      name TEXT NOT NULL,
      position TEXT NOT NULL,
      emailAddress TEXT NOT NULL,
      phoneNumber TEXT NOT NULL,
      contacted BOOLEAN NOT NULL
    )")

  override __.Down() =
    base.Execute(@"DROP TABLE Profiles")
