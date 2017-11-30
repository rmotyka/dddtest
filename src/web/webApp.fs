module Router

open System
open Users.Models
open service
open Users.Repository
open web.Models
open Giraffe.HttpHandlers
open Giraffe.Razor.HttpHandlers
open Microsoft.AspNetCore.Http
open Users.Service
open Users

// ---------------------------------
// Web app
// ---------------------------------

let userHandler email =
    fun (next : HttpFunc) (ctx : HttpContext) ->
        let userMaybe = getNewUser "xxx" email
        match userMaybe with
        | Some user ->
            saveUser user |> ignore
            let anotherUser = loadUser user.id
            printfn "%A" anotherUser
            text (anotherUser.id.ToString()) next ctx
        | None -> 
            text "No user" next ctx
            
let newUserHandler =
    fun (next : HttpFunc) (ctx : HttpContext) ->
        
        text "Saved" next ctx

let webApp : HttpHandler =
    choose [
        GET >=>
            choose [
                routef "/%s"      userHandler
                route "/save" >=> newUserHandler
            ]
        setStatusCode 404 >=> text "Not Found" ]