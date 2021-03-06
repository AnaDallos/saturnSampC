namespace Profiles



open Microsoft.AspNetCore.Http
open FSharp.Control.Tasks.ContextInsensitive
open Config
open Saturn

open Giraffe.ResponseWriters
open Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http
open System.Net.Http.Headers

module Controller =

  let indexAction (ctx : HttpContext) =
    (*task {
      let cnf = Controller.getConfig ctx
      let! result = Database.getAll cnf.connectionString
      match result with
      | Ok result ->
        return Views.index ctx (List.ofSeq result)
      | Error ex ->
        return raise ex
    }*)

    task {
        let cnf = Controller.getConfig ctx
        let! result = Database.getAll cnf.connectionString
        match result with
        | Ok result ->
          return ctx.WriteJsonAsync (result)
        | Error ex ->
          return ctx.WriteJsonAsync ex
    }
  let showAction (ctx: HttpContext) (id : string) =
    task {
      let cnf = Controller.getConfig ctx
      let! result = Database.getById cnf.connectionString id
      match result with
      | Ok (Some result) ->
        return Views.show ctx result
      | Ok None ->
        return NotFound.layout
      | Error ex ->
        return raise ex
    }
  let addAction (ctx: HttpContext) =
    task {
      return Views.add ctx None Map.empty
    }
  let editAction (ctx: HttpContext) (id : string) =
    task {
      let cnf = Controller.getConfig ctx
      let! result = Database.getById cnf.connectionString id
      match result with
      | Ok (Some result) ->
        return Views.edit ctx result Map.empty
      | Ok None ->
        return NotFound.layout
      | Error ex ->
        return raise ex
    }
  let createAction (ctx: HttpContext) =
    task {
      let! input = Controller.getModel<Profile> ctx
      let validateResult = Validation.validate input
      if validateResult.IsEmpty then

        let cnf = Controller.getConfig ctx
        let! result = Database.insert cnf.connectionString input
        match result with
        | Ok _ ->
          return! Controller.redirect ctx (Links.index ctx)
        | Error ex ->
          return raise ex
      else
        return! Controller.renderXml ctx (Views.add ctx (Some input) validateResult)
    }
  let updateAction (ctx: HttpContext) (id : string) =
    task {
      let! input = Controller.getModel<Profile> ctx
      let validateResult = Validation.validate input
      if validateResult.IsEmpty then
        let cnf = Controller.getConfig ctx
        let! result = Database.update cnf.connectionString input
        match result with
        | Ok _ ->
          return! Controller.redirect ctx (Links.index ctx)
        | Error ex ->
          return raise ex
      else
        return! Controller.renderXml ctx (Views.edit ctx input validateResult)
    }
  let deleteAction (ctx: HttpContext) (id : string) =
    task {
      let cnf = Controller.getConfig ctx
      let! result = Database.delete cnf.connectionString id
      match result with
      | Ok _ ->
        return! Controller.redirect ctx (Links.index ctx)
      | Error ex ->
        return raise ex
    }
  
  let resource = controller {
    index indexAction
    show showAction
    add addAction
    edit editAction
    create createAction
    update updateAction
    delete deleteAction
  }

  type B = {text:string}
  let ApiAction=
    json { text= "Hello world" }

  let ProfileApiAction (ctx: HttpContext) =
    task {
        let cnf = Controller.getConfig ctx
        let! result = Database.getAll cnf.connectionString
        match result with
        | Ok result ->
          return ctx.WriteJsonAsync (result)
        | Error ex ->
          return ctx.WriteJsonAsync ex
    }
   
  let profileRouter = router {
    not_found_handler (htmlView NotFound.layout)
    get "/api" ApiAction
  }


