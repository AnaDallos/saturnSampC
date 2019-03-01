namespace Hello

open Saturn
open Giraffe.ResponseWriters

module Controller=
    
    let indexAction =
        htmlView(Views.index)

    let helloView = router{
        get "/" indexAction
    }
 
    let index2Action name =
        htmlView(Views.index2 name)

    let helloView2 = router{
        getf "/%s" index2Action 
    }

