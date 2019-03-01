namespace Profiles

open Database
open Microsoft.Data.Sqlite
open System.Threading.Tasks
open FSharp.Control.Tasks.ContextInsensitive

module Database =
  let getAll connectionString : Task<Result<Profile seq, exn>> =
    task {
      use connection = new SqliteConnection(connectionString)
      return! query connection "SELECT id, name, position, emailAddress, phoneNumber, contacted FROM Profiles" None
    }

  let getById connectionString id : Task<Result<Profile option, exn>> =
    task {
      use connection = new SqliteConnection(connectionString)
      return! querySingle connection "SELECT id, name, position, emailAddress, phoneNumber, contacted FROM Profiles WHERE id=@id" (Some <| dict ["id" => id])
    }

  let update connectionString v : Task<Result<int,exn>> =
    task {
      use connection = new SqliteConnection(connectionString)
      return! execute connection "UPDATE Profiles SET id = @id, name = @name, position = @position, emailAddress = @emailAddress, phoneNumber = @phoneNumber, contacted = @contacted WHERE id=@id" v
    }

  let insert connectionString v : Task<Result<int,exn>> =
    task {
      use connection = new SqliteConnection(connectionString)
      return! execute connection "INSERT INTO Profiles(id, name, position, emailAddress, phoneNumber, contacted) VALUES (@id, @name, @position, @emailAddress, @phoneNumber, @contacted)" v
    }

  let delete connectionString id : Task<Result<int,exn>> =
    task {
      use connection = new SqliteConnection(connectionString)
      return! execute connection "DELETE FROM Profiles WHERE id=@id" (dict ["id" => id])
    }

