module Router

open Saturn
open Giraffe.Core
open Giraffe.ResponseWriters


let browser = pipeline {
    plug acceptHtml
    plug putSecureBrowserHeaders
    plug fetchSession
    set_header "x-pipeline-type" "Browser"
}

let defaultView = router {
    get "/" (htmlView Index.layout)
    get "/index.html" (redirectTo false "/")
    get "/default.html" (redirectTo false "/")
}

let browserRouter = router {
    not_found_handler (htmlView NotFound.layout) //Use the default 404 webpage
    pipe_through browser //Use the default browser pipeline
    forward "" defaultView //Use the default view
    forward "/books" Books.Controller.resource //added by tutorial
    //forward "/hello" Hello.Controller.helloView 
    //forward "/hello2" Hello.Controller.helloView2 
    forward "/profile" Profiles.Controller.resource
    forward "/profile2" ApiProfiles.Controller.ApiProfileHW
    //forward "/api" Profiles.Controller.ProfileApiAction
    //forward "/api" appRouter
}

//Other scopes may use different pipelines and error handlers

let api = pipeline {
     //plug acceptJson
     plug acceptHtml   
     set_header "x-pipeline-type" "Api"
}

let apiRouter = router {
    pipe_through api
    forward "/hw" ApiProfiles.Controller.ApiProfileHW
    //forward "" 
}

let appRouter = router {
    forward "" browserRouter
    forward "/api" apiRouter
}