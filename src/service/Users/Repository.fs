module Users.Repository

open Marten
open Npgsql
open System
open Users.Models

let createConnString host user pass database =
    sprintf "Host=%s;Username=%s;Password=%s;Database=%s" host user pass database
    |> NpgsqlConnectionStringBuilder

let tesetConnectionString = 
    createConnString "localhost" "postgres" "cctechnology" "marten_test"

let getOptions (connectionString : string) (opt: StoreOptions) =
    opt.Connection(connectionString)
    opt.Schema.For<User>().DocumentAlias "user" |> ignore

let getStore (connectionString : string) = 
    connectionString |> getOptions|> DocumentStore.For

let testStore = getStore tesetConnectionString.ConnectionString

let create userName email =
    {
        id = 0
        email = email
        userName = userName
    }

let save (store : IDocumentStore) (user : User) =
    use session = store.OpenSession()
    session.Store(user)
    session.SaveChanges()
    user

let saveUser (user : User) =
    save testStore user